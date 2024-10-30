using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using Models;

public class UICharacterSelect : MonoBehaviour
{
    // UI元素引用
    public GameObject panelCreate; // 角色创建界面
    public GameObject panelSelect; // 角色选择界面
    public GameObject EnterGameButton; // 进入游戏按钮

    public InputField charName; // 输入角色名的输入框
    CharacterClass charClass; // 当前选择的角色类型

    public Transform uiCharList; // UI中展示角色列表的位置
    public GameObject uiCharInfo; // 单个角色信息的预制件

    public List<GameObject> uiChars = new List<GameObject>(); // 存储UI中的角色对象列表

    public Image[] titles; // 角色类别的标题图像
    public Image[] UnSelects; // 未选中的类别图像
    public Image[] Selects; // 选中的类别图像

    public Text descs; // 描述文本

    public Text[] names; // 名称文本

    private int selectCharacteridx = -1; // 当前选中的角色索引
    public UICharacterView characterView; // 角色视图
    private bool isSelect = false; // 是否已经选择了角色类型

    // 初始化
    void Start()
    {
        InitCharacterSelect(true);
        DataManager.Instance.Load(); // 加载数据管理器的数据
        UserService.Instance.OnCharacterCreate = OnCharacterCreate; // 设置角色创建完成时的回调
    }

    // 初始化角色选择界面
    public void InitCharacterSelect(bool init)
    {
        panelCreate.SetActive(false); // 关闭角色创建界面
        panelSelect.SetActive(true); // 打开角色选择界面

        characterView.CurrentCharacter = -1; // 清空当前显示的角色索引

        if (init)
        {
            foreach (var old in uiChars)
            {
                Destroy(old); // 销毁旧的角色UI元素
            }
            uiChars.Clear(); // 清空角色列表

            // 遍历用户的所有角色，并在UI上显示它们
            for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
            {
                GameObject go = Instantiate(uiCharInfo, this.uiCharList);
                UICharInfo chrInfo = go.GetComponent<UICharInfo>();
                chrInfo.info = User.Instance.Info.Player.Characters[i]; // 设置角色信息

                Button button = go.GetComponent<Button>();
                int idx = i;
                button.onClick.AddListener(() => // 添加点击事件
                {
                    OnSelectCharacter(idx);
                });
                uiChars.Add(go); // 添加到角色列表
                go.SetActive(true); // 显示角色UI元素
            }
        }
    }

    // 初始化角色创建界面
    public void InitCharacterCreate()
    {
        panelCreate.SetActive(true); // 打开角色创建界面
        panelSelect.SetActive(false); // 关闭角色选择界面
        charName.text = ""; // 清空角色名输入框

        // 配置角色类型的UI显示
        for (int i = 0; i < 3; i++)
        {
            titles[i].gameObject.SetActive(i == 2); // 设置默认显示最后一个类别
            Selects[i].gameObject.SetActive(false); // 默认不选中任何类别
            UnSelects[i].gameObject.SetActive(true); // 默认显示未选中状态
            names[i].text = DataManager.Instance.Characters[i + 1].Name; // 设置类别名称
        }

        descs.text = ""; // 清空描述文本
        isSelect = false; // 设置默认没有选择角色类型
    }

    // 更新函数，每帧调用一次
    void Update()
    {
        // 空实现，这里可以添加更新逻辑
    }

    // 角色创建按钮点击事件
    public void OnClickCreate()
    {
        if (!isSelect)
        {
            MessageBox.Show("请选择角色类别!"); // 提示选择角色类别
        }
        else if (string.IsNullOrEmpty(this.charName.text))
        {
            MessageBox.Show("请输入角色名称!"); // 提示输入角色名称
        }
        else
        {
            UserService.Instance.SendCharacterCreate(this.charName.text, this.charClass); // 发送创建角色请求
        }
    }

    // 选择角色类别事件
    public void OnSelectClass(int charClass)
    {
        this.charClass = (CharacterClass)charClass; // 设置当前选择的角色类型

        isSelect = true; // 标记已选择角色类型

        characterView.CurrentCharacter = charClass - 1; // 设置当前显示的角色类型

        // 更新UI显示
        for (int i = 0; i < 3; i++)
        {
            titles[i].gameObject.SetActive(i == charClass - 1); // 显示选中的角色类别
            Selects[i].gameObject.SetActive(i == charClass - 1); // 显示选中状态
            UnSelects[i].gameObject.SetActive(i != charClass - 1); // 隐藏未选中状态
            names[i].text = DataManager.Instance.Characters[i + 1].Name; // 设置类别名称
        }

        descs.text = DataManager.Instance.Characters[charClass].Description; // 设置描述文本
    }

    // 角色创建响应事件
    void OnCharacterCreate(Result result, string message)
    {
        if (result == Result.Success)
        {
            InitCharacterSelect(true); // 创建成功后重新初始化角色选择界面
        }
        else
        {
            MessageBox.Show(message, "错误", MessageBoxType.Error); // 显示错误信息
        }
    }

    // 选择角色事件
    public void OnSelectCharacter(int idx)
    {
        this.selectCharacteridx = idx; // 设置当前选中的角色索引
        var cha = User.Instance.Info.Player.Characters[idx]; // 获取选中的角色信息
        Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class); // 输出日志信息
        characterView.CurrentCharacter = (int)cha.Class - 1; // 设置角色视图

        // 更新UI显示
        for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
        {
            UICharInfo ci = this.uiChars[i].GetComponent<UICharInfo>();
            ci.Selectd = (idx == i); // 设置UI显示选中状态
        }
    }

    // 进入游戏按钮点击事件
    public void OnClickPlay()
    {
        if (selectCharacteridx >= 0)
        {
            UserService.Instance.SendGameEnter(selectCharacteridx); // 发送进入游戏请求
        }
    }
}
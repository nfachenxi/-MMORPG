using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using Models;

public class UICharacterSelect : MonoBehaviour {

	public GameObject panelCreate;
	public GameObject panelSelect;

	public GameObject EnterGameButton;

	public InputField charName;
	CharacterClass charClass;

	public Transform uiCharList;
	public GameObject uiCharInfo;

	public List<GameObject> uiChars = new List<GameObject>();

	public Image[] titles;
	public Image[] UnSelects;
	public Image[] Selects;

	public Text descs;

	public Text[] names;

	private int selectCharacteridx = -1;

	public UICharacterView characterView;
	private bool isSelect = false;

	// Use this for initialization
	void Start () {
		InitCharacterSelect(true);
        DataManager.Instance.Load();
        UserService.Instance.OnCharacterCreate = OnCharacterCreate;
	}
	public void InitCharacterSelect(bool init)
	{
		panelCreate.SetActive(false);
		panelSelect.SetActive(true);

		characterView.CurrentCharacter = -1;

		if(init)
		{
			foreach(var old in uiChars)
			{
				Destroy(old);
			}
			uiChars.Clear();

		for(int i = 0; i < User.Instance.Info.Player.Characters.Count; i ++)
			{
				GameObject go = Instantiate(uiCharInfo, this.uiCharList);
				UICharInfo chrInfo = go.GetComponent<UICharInfo>();
				chrInfo.info = User.Instance.Info.Player.Characters[i];

				Button button = go.GetComponent<Button>();
				int idx = i;
				button.onClick.AddListener(() =>{
					OnSelectCharacter(idx);
				});
				uiChars.Add(go);
				go.SetActive(true);
			}
		}
	}
	
	public void InitCharacterCreate()
	{
		panelCreate.SetActive(true);
		panelSelect.SetActive(false);
		charName.text = "";
        for (int i = 0; i < 3; i++)
        {
            titles[i].gameObject.SetActive(i == 2);
            Selects[i].gameObject.SetActive(false);
            UnSelects[i].gameObject.SetActive(true);
            names[i].text = DataManager.Instance.Characters[i + 1].Name;
        }

        descs.text = "";

        isSelect = false;
	}


	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickCreate()
	{
		if(!isSelect)
		{
			MessageBox.Show("请选择角色类别!");
		}
		else if(string.IsNullOrEmpty(this.charName.text))
		{
            MessageBox.Show("请输入角色名称!");
        }
		else 
		{
			UserService.Instance.SendCharacterCreate(this.charName.text, this.charClass);
        }
    }

	public void OnSelectClass(int charClass)
	{
		this.charClass = (CharacterClass)charClass;

		isSelect = true;

		characterView.CurrentCharacter = charClass - 1;

		for(int i = 0; i < 3; i ++)
		{
			titles[i].gameObject.SetActive(i == charClass - 1);
			Selects[i].gameObject.SetActive(i == charClass - 1);
            UnSelects[i].gameObject.SetActive(i != charClass - 1);
			names[i].text = DataManager.Instance.Characters[i + 1].Name;
		}

		descs.text = DataManager.Instance.Characters[charClass].Description;
	}

	void OnCharacterCreate(Result result, string message)
	{
		if (result == Result.Success)
		{
			InitCharacterSelect(true);
        }
		else
			MessageBox.Show(message, "错误", MessageBoxType.Error);
	}

	public void OnSelectCharacter(int idx)
	{
		this.selectCharacteridx = idx;
		var cha = User.Instance.Info.Player.Characters[idx];
		Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class);
		User.Instance.CurrentCharacter = cha;
		int test = (int)cha.Class - 1;
		characterView.CurrentCharacter = test;

        for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
        {
			UICharInfo ci = this.uiChars[i].GetComponent<UICharInfo>();
			ci.Selectd = (idx == i);
        }


    }

	public void OnClickPlay()
	{
		if(selectCharacteridx >= 0)
		{
			MessageBox.Show("进入游戏", "进入游戏", MessageBoxType.Confirm);
		}
	}

}

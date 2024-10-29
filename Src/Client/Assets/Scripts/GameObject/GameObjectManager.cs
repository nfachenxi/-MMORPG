using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 引入自定义命名空间
using Entities;
using Managers;
using SkillBridge.Message;
using Models;

// 单例管理器用于管理游戏中的物体
public class GameObjectManager : MonoSingleton<GameObjectManager>
{
    // 字典用于存储游戏中所有角色的引用
    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();

    // 当MonoSingleton基类初始化完成后调用
    protected override void OnStart()
    {
        // 初始化游戏物体协程
        StartCoroutine(InitGameObjects());
        // 订阅角色进入场景事件
        CharacterManager.Instance.OnCharacterEnter += OnCharacterEnter;
        // 订阅角色离开场景事件
        CharacterManager.Instance.OnCharacterLeave += OnCharacterLeave;
    }

    // 在对象销毁时取消订阅事件
    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter -= OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave -= OnCharacterLeave;
    }

    // 每帧调用一次，这里没有实现逻辑
    void Update()
    {

    }

    // 角色进入场景时创建角色物体
    void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);
    }

    // 角色离开场景时移除角色物体
    void OnCharacterLeave(Character cha)
    {
        if (!Characters.ContainsKey(cha.entityId))
            return; // 如果字典中不存在该角色，则直接返回

        if (Characters[cha.entityId] != null)
        {
            // 销毁角色物体
            Destroy(Characters[cha.entityId]);
            // 移除字典中对该物体的引用
            this.Characters.Remove(cha.entityId);
        }
    }

    // 初始化游戏物体的协程
    IEnumerator InitGameObjects()
    {
        // 遍历所有角色并创建物体
        foreach (var cha in CharacterManager.Instance.Characters.Values)
        {
            CreateCharacterObject(cha);
            yield return null; // 每次创建后yield null以允许其他操作执行
        }
    }

    // 创建角色物体的方法
    private void CreateCharacterObject(Character character)
    {
        // 检查是否已存在该角色物体或物体是否已被销毁
        if (!Characters.ContainsKey(character.entityId) || Characters[character.entityId] == null)
        {
            // 加载角色资源
            Object obj = Resloader.Load<Object>(character.Define.Resource);
            if (obj == null)
            {
                // 资源未找到时打印错误信息
                Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed.", character.Define.TID, character.Define.Resource);
                return;
            }
            // 实例化角色物体
            GameObject go = (GameObject)Instantiate(obj, this.transform);
            // 设置物体名称
            go.name = "Character_" + character.Info.Id + "_" + character.Info.Name;
            // 存储物体引用到字典中
            Characters[character.entityId] = go;

            // 添加角色名字栏UI元素
            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);
        }
        // 初始化物体
        this.InitGameObject(Characters[character.entityId], character);
    }

    // 初始化游戏物体的位置和其他组件
    private void InitGameObject(GameObject go, Character character)
    {
        // 设置物体位置
        go.transform.position = GameObjectTool.LogicToWorld(character.position);
        // 设置物体朝向
        go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
        // 获取实体控制器组件
        EntityController ec = go.GetComponent<EntityController>();
        if (ec != null)
        {
            // 设置控制器的角色属性
            ec.entity = character;
            ec.isPlayer = character.IsPlayer;
        }
        // 获取玩家输入控制器组件
        PlayerInputController pc = go.GetComponent<PlayerInputController>();
        if (pc != null)
        {
            // 检查是否是当前玩家的角色
            if (character.Info.Id == Models.User.Instance.CurrentCharacter.Id)
            {
                // 设置当前玩家角色的物体
                User.Instance.CurrentCharacterObject = go;
                // 设置主玩家摄像机的目标物体
                MainPlayerCamera.Instance.player = go;
                // 启用玩家输入控制器
                pc.enabled = true;
                pc.character = character;
                pc.entityController = ec;
            }
            else
            {
                // 禁用非玩家角色的输入控制器
                pc.enabled = false;
            }
        }
    }
}
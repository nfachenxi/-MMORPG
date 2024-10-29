using Entities; // 引入实体相关的命名空间
using SkillBridge.Message; // 可能是用于消息传递或协议定义的命名空间
using System; // 基础系统类库
using System.Collections; // 基本集合接口
using System.Collections.Generic; // 泛型集合框架
using System.Linq; // LINQ 相关操作
using System.Security.Policy; // 安全策略相关类（看起来在这个上下文中可能没有直接使用）
using UnityEngine; // Unity 游戏引擎的基础类库
using UnityEngine.Events; // Unity 事件系统

namespace Managers // 定义一个名为 Managers 的命名空间，可能是存放各种管理器类的地方
{
    // 定义一个角色管理器类，继承自 Singleton<CharacterManager>，确保其为单例
    public class CharacterManager : Singleton<CharacterManager>, IDisposable
    {
        // 存储所有角色的字典
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();

        // 角色进入场景时触发的委托
        public UnityAction<Character> OnCharacterEnter;
        // 角色离开场景时触发的委托
        public UnityAction<Character> OnCharacterLeave;

        // 默认构造函数
        public CharacterManager()
        {
        }

        // 实现 IDisposable 接口的 Dispose 方法，用于清理资源
        public void Dispose()
        {
            // 清理资源的逻辑可以在这里实现
        }

        // 初始化方法
        public void Init()
        {
            // 初始化逻辑可以在这里实现
        }

        // 清空所有角色
        public void Clear()
        {
            int[] keys = this.Characters.Keys.ToArray(); // 获取当前所有角色的键（ID）并转换为数组
            foreach (var key in keys) // 遍历所有的键
            {
                this.RemoveCharacter(key); // 移除对应的角色
            }
            this.Characters.Clear(); // 清空角色字典
        }

        // 添加一个新角色
        public void AddCharacter(SkillBridge.Message.NCharacterInfo cha)
        {
            Debug.LogFormat("AddCharacter: {0} : {1} Map: {2} Entity: {3}",
                            cha.Id, cha.Name, cha.mapId, cha.Entity.String()); // 日志记录添加角色的信息
            Character character = new Character(cha); // 创建新的角色实例
            this.Characters[cha.Id] = character; // 使用角色ID作为键，将角色存入字典
            EntityManager.Instance.AddEntity(character); // 向全局的实体管理器添加这个角色

            if (OnCharacterEnter != null) // 如果有监听角色进入的委托
            {
                OnCharacterEnter(character); // 调用监听委托
            }
        }

        // 移除一个角色
        public void RemoveCharacter(int characterId)
        {
            Debug.LogFormat("RemoveCharacter: {0}", characterId); // 日志记录移除角色的信息
            if (this.Characters.ContainsKey(characterId)) // 检查是否有此角色
            {
                EntityManager.Instance.RemoveEntity(this.Characters[characterId].Info.Entity); // 从全局实体管理器中移除角色
                if (OnCharacterLeave != null) // 如果有监听角色离开的委托
                {
                    OnCharacterLeave(this.Characters[characterId]); // 调用监听委托
                }
                this.Characters.Remove(characterId); // 从角色字典中移除此角色
            }
        }
    }
}
using Entities; // 引用实体类所在的命名空间
using SkillBridge.Message; // 可能是用于消息或通信相关的命名空间
using System; // 基础系统类和基础服务
using System.Collections.Generic; // 集合框架
using System.Linq; // LINQ 相关操作
using System.Text; // 字符串处理类

namespace Managers // 定义一个名为 Managers 的命名空间，可能是存放各种管理器类的地方
{
    // 定义一个接口，用于接收实体更新的通知
    interface IEntityNotify
    {
        void OnEntityRemoved(); // 实体移除时调用的方法
        void OnEntityChanged(Entity entity); // 实体改变时调用的方法
        void OnEntityEvent(EntityEvent @event); // 实体事件发生时调用的方法
    }

    // 定义一个单例类，用于管理游戏中的所有实体
    class EntityManager : Singleton<EntityManager>
    {
        // 存储游戏内所有实体的字典
        Dictionary<int, Entity> entities = new Dictionary<int, Entity>();

        // 存储所有订阅了实体更新通知的对象的字典
        Dictionary<int, IEntityNotify> notifiers = new Dictionary<int, IEntityNotify>();

        // 注册一个实体更新的观察者
        public void RegisterEntityChangeNotify(int entityId, IEntityNotify notify)
        {
            this.notifiers[entityId] = notify; // 将观察者与实体ID关联起来
        }

        // 添加一个新的实体到管理器中
        public void AddEntity(Entity entity)
        {
            entities[entity.entityId] = entity; // 使用实体的唯一ID作为键值存储实体
        }

        // 移除一个实体，并通知观察者
        public void RemoveEntity(NEntity entity) // 注意这里参数类型是NEntity而不是Entity，可能是一个网络数据传输对象
        {
            this.entities.Remove(entity.Id); // 从实体字典中移除该实体
            if (notifiers.ContainsKey(entity.Id)) // 如果有订阅者
            {
                notifiers[entity.Id].OnEntityRemoved(); // 通知观察者实体已被移除
                notifiers.Remove(entity.Id); // 从观察者字典中移除该观察者
            }
        }

        // 处理从网络同步过来的实体数据
        internal void OnEntitySync(NEntitySync data) // NEntitySync可能是一个包含同步数据的消息对象
        {
            Entity entity = null; // 初始化实体引用
            entities.TryGetValue(data.Id, out entity); // 尝试获取已存在的实体
            if (entity != null) // 如果实体存在
            {
                if (data.Entity != null)
                    entity.EntityData = data.Entity; // 更新实体的数据
                if (notifiers.ContainsKey(data.Id)) // 如果有订阅者
                {
                    notifiers[data.Id].OnEntityChanged(entity); // 通知观察者实体已更改
                    notifiers[data.Id].OnEntityEvent(data.Event); // 通知观察者实体事件已发生
                }
            }
        }
    }
}
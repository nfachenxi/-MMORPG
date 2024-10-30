using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SkillBridge.Message;

namespace Entities
{
    // 基础实体类，提供基本的实体行为和数据
    public class Entity
    {
        // 实体的唯一标识符
        public int entityId;

        // 实体的位置（整型向量）
        public Vector3Int position;

        // 实体的方向（整型向量）
        public Vector3Int direction;

        // 实体的速度
        public int speed;

        // 实体的数据包装
        private NEntity entityData;

        // 属性封装了实体的数据
        public NEntity EntityData
        {
            get
            {
                UpdateEntityData();
                return entityData;
            }
            set
            {
                entityData = value;
                // 设置实体数据时更新内部状态
                this.SetEntityData(value);
            }
        }

        // 构造函数，初始化实体
        public Entity(NEntity entity)
        {
            // 设置实体ID
            this.entityId = entity.Id;
            // 设置实体数据
            this.entityData = entity;
            // 设置实体状态
            this.SetEntityData(entity);
        }

        // 更新方法，在每帧调用
        public virtual void OnUpdate(float delta)
        {
            if (this.speed != 0)
            {
                // 计算移动方向
                Vector3 dir = this.direction;
                // 根据速度和时间增量更新位置
                this.position += Vector3Int.RoundToInt(dir * speed * delta / 100f);
            }
            // 更新数据包装中的位置、方向和速度
            entityData.Position.FromVector3Int(this.position);
            entityData.Direction.FromVector3Int(this.direction);
            entityData.Speed = this.speed;
        }

        // 设置实体的数据
        public void SetEntityData(NEntity entity)
        {
            // 将网络数据转换为本地数据格式
            this.position = this.position.FromNVector3(entity.Position);
            this.direction = this.direction.FromNVector3(entity.Direction);
            this.speed = entity.Speed;
        }

        private void UpdateEntityData()
        {
            entityData.Position.FromVector3Int(this.position);
            entityData.Direction.FromVector3Int(this.direction);
            entityData.Speed = this.speed;
        }
    }
}
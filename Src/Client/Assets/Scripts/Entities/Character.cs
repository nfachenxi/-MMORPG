using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;
using UnityEngine;

namespace Entities
{
    // 定义一个Character类继承自Entity类
    public class Character : Entity
    {
        // 存储角色的信息
        public NCharacterInfo Info;

        // 存储角色的定义数据
        public Common.Data.CharacterDefine Define;

        // 获取角色的名字
        public string Name
        {
            get
            {
                // 如果是玩家类型，则返回玩家的名字；否则返回定义的名字
                if (this.Info.Type == CharacterType.Player)
                    return this.Info.Name;
                else
                    return this.Define.Name;
            }
        }

        // 判断是否是当前玩家的角色
        public bool IsPlayer
        {
            get { return this.Info.Id == Models.User.Instance.CurrentCharacter.Id; }
        }

        // 构造函数
        public Character(NCharacterInfo info) : base(info.Entity)
        {
            // 初始化角色信息
            this.Info = info;
            // 从数据管理器获取角色定义数据
            this.Define = DataManager.Instance.Characters[info.Tid];
        }

        // 角色向前移动
        public void MoveForward()
        {
            Debug.LogFormat("MoveForward");
            // 设置速度为正向速度
            this.speed = this.Define.Speed;
        }

        // 角色向后移动
        public void MoveBack()
        {
            Debug.LogFormat("MoveBack");
            // 设置速度为负向速度
            this.speed = -this.Define.Speed;
        }

        // 停止移动
        public void Stop()
        {
            Debug.LogFormat("Stop");
            // 设置速度为0
            this.speed = 0;
        }

        // 设置角色的方向
        public void SetDirection(Vector3Int direction)
        {
            Debug.LogFormat("SetDirection:{0}", direction);
            // 设置角色的方向
            this.direction = direction;
        }

        // 设置角色的位置
        public void SetPosition(Vector3Int position)
        {
            Debug.LogFormat("SetPosition:{0}", position);
            // 设置角色的位置
            this.position = position;
        }
    }
}
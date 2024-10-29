using System.Collections; // 提供非泛型集合的命名空间
using System.Collections.Generic; // 泛型集合的命名空间
using UnityEngine; // Unity游戏引擎的API

using Entities; // 自定义实体类所在的命名空间
using SkillBridge.Message; // 自定义消息结构的命名空间
using Services; // 自定义服务类所在的命名空间

// 玩家输入控制器类，用于处理玩家的输入并控制角色的行为
public class PlayerInputController : MonoBehaviour
{
    public Rigidbody rb; // 角色的刚体组件
    SkillBridge.Message.CharacterState state; // 角色状态

    public Character character; // 角色实例

    public float rotateSpeed = 2.0f; // 角色旋转速度
    public float turnAngle = 10; // 转向角度限制

    public int speed; // 速度（可能用于UI显示）

    public EntityController entityController; // 实体控制器，用于控制实体行为

    public bool onAir = false; // 是否在空中

    // 初始化
    void Start()
    {
        state = SkillBridge.Message.CharacterState.Idle; // 设置初始状态为闲置
        if (this.character == null) // 如果没有角色实例，则创建一个
        {
            DataManager.Instance.Load(); // 加载数据
            NCharacterInfo cinfo = new NCharacterInfo(); // 创建角色信息
            cinfo.Id = 1; // 设置ID
            cinfo.Name = "Test"; // 设置名字
            cinfo.Tid = 1; // 设置模板ID
            cinfo.Entity = new NEntity(); // 创建实体信息
            cinfo.Entity.Position = new NVector3(); // 创建位置信息
            cinfo.Entity.Direction = new NVector3(); // 创建方向信息
            cinfo.Entity.Direction.X = 0; // 方向X轴
            cinfo.Entity.Direction.Y = 100; // 方向Y轴
            cinfo.Entity.Direction.Z = 0; // 方向Z轴
            this.character = new Character(cinfo); // 创建角色实例

            if (entityController != null) entityController.entity = this.character; // 关联实体控制器
        }
    }

    void FixedUpdate()
    {
        if (character == null) // 如果没有角色，则退出
            return;

        float v = Input.GetAxis("Vertical"); // 获取垂直输入（前后移动）
        if (v > 0.01) // 如果向前移动
        {
            if (state != SkillBridge.Message.CharacterState.Move) // 如果不是移动状态
            {
                state = SkillBridge.Message.CharacterState.Move; // 更改状态为移动
                this.character.MoveForward(); // 让角色向前移动
                this.SendEntityEvent(EntityEvent.MoveFwd); // 发送向前移动事件
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f; // 设置速度
        }
        else if (v < -0.01) // 如果向后移动
        {
            if (state != SkillBridge.Message.CharacterState.Move) // 如果不是移动状态
            {
                state = SkillBridge.Message.CharacterState.Move; // 更改状态为移动
                this.character.MoveBack(); // 让角色向后移动
                this.SendEntityEvent(EntityEvent.MoveBack); // 发送向后移动事件
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f; // 设置速度
        }
        else // 如果没有移动
        {
            if (state != SkillBridge.Message.CharacterState.Idle) // 如果不是闲置状态
            {
                state = SkillBridge.Message.CharacterState.Idle; // 更改状态为闲置
                this.rb.velocity = Vector3.zero; // 停止角色
                this.character.Stop(); // 让角色停止
                this.SendEntityEvent(EntityEvent.Idle); // 发送闲置事件
            }
        }

        if (Input.GetButtonDown("Jump")) // 如果按下跳跃键
        {
            this.SendEntityEvent(EntityEvent.Jump); // 发送跳跃事件
        }

        float h = Input.GetAxis("Horizontal"); // 获取水平输入（左右移动）
        if (h < -0.1 || h > 0.1) // 如果有水平移动
        {
            this.transform.Rotate(0, h * rotateSpeed, 0); // 旋转角色
            Vector3 dir = GameObjectTool.LogicToWorld(character.direction); // 获取世界坐标下的方向
            Quaternion rot = new Quaternion(); // 创建四元数
            rot.SetFromToRotation(dir, this.transform.forward); // 设置四元数从dir到forward的旋转

            if (rot.eulerAngles.y > this.turnAngle && rot.eulerAngles.y < (360 - this.turnAngle)) // 如果角度在限制范围内
            {
                character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward)); // 设置角色的新方向
                rb.transform.forward = this.transform.forward; // 更新刚体的方向
                this.SendEntityEvent(EntityEvent.None); // 发送无事件
            }
        }
    }

    Vector3 lastPos; // 上一帧的位置
    float lastSync = 0; // 上一次同步的时间戳

    private void LateUpdate()
    {
        Vector3 offset = this.rb.transform.position - lastPos; // 计算位移
        this.speed = (int)(offset.magnitude * 100f / Time.deltaTime); // 计算速度
        this.lastPos = this.rb.transform.position; // 更新位置

        if ((GameObjectTool.WorldToLogic(this.rb.transform.position) - this.character.position).magnitude > 50) // 如果位置差距大于50单位
        {
            this.character.SetPosition(GameObjectTool.WorldToLogic(this.rb.transform.position)); // 更新角色的位置
            this.SendEntityEvent(EntityEvent.None); // 发送无事件
        }
        this.transform.position = this.rb.transform.position; // 更新角色的位置
    }

    void SendEntityEvent(EntityEvent entityEvent)
    {
        if (entityController != null) // 如果有实体控制器
            entityController.OnEntityEvent(entityEvent); // 触发实体事件
        MapService.Instance.SendMapEntitySync(entityEvent, this.character.EntityData); // 同步实体事件到地图服务
    }
}
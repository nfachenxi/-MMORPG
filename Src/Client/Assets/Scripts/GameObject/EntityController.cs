using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Managers;

// 控制器类，用于处理实体的行为
public class EntityController : MonoBehaviour, IEntityNotify
{
    // 动画控制器
    public Animator anim;
    // 刚体组件
    public Rigidbody rb;
    // 当前基础状态信息
    private AnimatorStateInfo currentBaseState;

    // 实体引用
    public Entity entity;

    // 实体位置
    public Vector3 position;
    // 实体方向
    public Vector3 direction;
    // 旋转四元数
    Quaternion rotation;

    // 上一帧的位置
    public Vector3 lastPosition;
    // 上一帧的旋转
    Quaternion lastRotation;

    // 移动速度
    public float speed;
    // 动画播放速度
    public float animSpeed = 1.5f;
    // 跳跃力
    public float jumpPower = 3.0f;

    // 是否是玩家控制的实体
    public bool isPlayer = false;

    // 初始化时调用
    void Start()
    {
        if (entity != null)
        {
            // 注册实体变更通知
            EntityManager.Instance.RegisterEntityChangeNotify(entity.entityId, this);
            // 更新变换
            this.UpdateTransform();
        }

        // 如果不是玩家控制的实体则关闭重力
        if (!this.isPlayer)
            rb.useGravity = false;
    }

    // 更新变换
    void UpdateTransform()
    {
        // 根据逻辑坐标更新位置
        this.position = GameObjectTool.LogicToWorld(entity.position);
        // 根据逻辑坐标更新方向
        this.direction = GameObjectTool.LogicToWorld(entity.direction);

        // 使用刚体移动到新位置
        this.rb.MovePosition(this.position);
        // 设置变换的方向
        this.transform.forward = this.direction;
        // 记录上一帧的位置和旋转
        this.lastPosition = this.position;
        this.lastRotation = this.rotation;
    }

    // 对象销毁时调用
    void OnDestroy()
    {
        if (entity != null)
        {
            // 输出销毁信息的日志
            Debug.LogFormat("{0} OnDestroy :ID:{1} POS:{2} DIR:{3} SPD:{4} ", this.name, entity.entityId, entity.position, entity.direction, entity.speed);
        }

        // 移除UI上的名字栏
        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveCharacterNameBar(this.transform);
        }
    }

    // 固定更新频率调用
    void FixedUpdate()
    {
        if (this.entity == null)
            return;

        // 更新实体状态
        this.entity.OnUpdate(Time.fixedDeltaTime);

        if (!this.isPlayer)
        {
            // 非玩家控制实体更新变换
            this.UpdateTransform();
        }
    }

    // 处理实体事件
    public void OnEntityEvent(EntityEvent entityEvent)
    {
        switch (entityEvent)
        {
            case EntityEvent.Idle:
                anim.SetBool("Move", false);
                anim.SetTrigger("Idle");
                break;
            case EntityEvent.MoveFwd:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.MoveBack:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.Jump:
                anim.SetTrigger("Jump");
                break;
            case EntityEvent.None:
                break;
        }
    }

    // 实体被移除时调用
    public void OnEntityRemoved()
    {
        if (UIWorldElementManager.Instance != null)
        {
            // 移除UI上的名字栏
            UIWorldElementManager.Instance.RemoveCharacterNameBar(this.transform);
        }
        // 销毁游戏对象
        Destroy(this.gameObject);
    }

    // 实体变更时调用
    public void OnEntityChanged(Entity entity)
    {
        Debug.LogFormat("OnEntityChanged : ID: {0}  POS:{1}  DIR:{2}  SPD: {3}", entity.entityId, entity.position, entity.direction, entity.speed);
    }
}
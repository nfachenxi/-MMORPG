using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEditor; // 注意：这个命名空间通常用于编辑器相关的代码，在运行时并不需要。
using UnityEngine;
using UnityEngine.UI;

public class UIMinimap : MonoBehaviour
{
    // 公开的组件引用
    public Collider MinimapBoundingBox; // 小地图的边界框Collider
    public Image minimap; // 小地图的Image组件
    public Image arrow; // 玩家指向箭头的Image组件
    public Text mapName; // 地图名称的Text组件

    private Transform playerTransform; // 玩家的Transform组件

    // 在游戏开始时初始化地图信息
    void Start()
    {
        MinimapManager.Instance.minimap = this;
        UpdateMap();
    }

    // 初始化地图
    public void UpdateMap()
    {
        // 设置地图名称
        this.mapName.text = User.Instance.CurrentMapData.Name;

        // 加载当前地图的小地图Sprite
        this.minimap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();

        // 设置Sprite的真实大小
        this.minimap.SetNativeSize();

        // 重置Image的位置为零点
        this.minimap.transform.localPosition = Vector3.zero;

        //获取管理器的包围盒
        this.MinimapBoundingBox = MinimapManager.Instance.MinimapBoundingBox;

        // 清空玩家位置
        this.playerTransform = null;
    }

    // 每帧更新
    void Update()
    {
        if(playerTransform == null)
        {
            playerTransform = MinimapManager.Instance.PlayerTransform;
        }


        // 如果边界框或玩家Transform为空则返回
        if (MinimapBoundingBox == null || playerTransform == null) return;

        // 计算真实宽度和高度
        float realWidth = MinimapBoundingBox.bounds.size.x;
        float realHeight = MinimapBoundingBox.bounds.size.z;

        // 计算相对位置
        float relaX = playerTransform.position.x - MinimapBoundingBox.bounds.min.x;
        float relaY = playerTransform.position.z - MinimapBoundingBox.bounds.min.z;

        // 计算枢轴点位置（即玩家在小地图上的位置）
        float pivotX = relaX / realWidth;
        float pivotY = relaY / realHeight;

        // 设置小地图Image的枢轴点
        this.minimap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.minimap.rectTransform.localPosition = Vector2.zero; // 设置局部位置为零点

        // 设置箭头的方向，根据玩家的方向
        this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);
    }
}
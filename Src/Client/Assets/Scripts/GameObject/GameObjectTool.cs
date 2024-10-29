using SkillBridge.Message; // 自定义消息结构的命名空间
using System; // 基础系统类库
using System.Collections.Generic; // 泛型集合框架
using System.Linq; // LINQ 相关操作
using System.Text; // 字符串处理类
using UnityEngine; // Unity游戏引擎的API

public class GameObjectTool
{
    // 将自定义的NVector3结构转换为Unity的Vector3，比例为1/100
    public static Vector3 LogicToWorld(NVector3 vector)
    {
        return new Vector3(vector.X / 100f, vector.Z / 100f, vector.Y / 100f);
    }

    // 将Vector3Int转换为Unity的Vector3，比例为1/100
    public static Vector3 LogicToWorld(Vector3Int vector)
    {
        return new Vector3(vector.x / 100f, vector.z / 100f, vector.y / 100f);
    }

    // 将整数值转换为Unity的浮点数，比例为1/100
    public static float LogicToWorld(int val)
    {
        return val / 100f;
    }

    // 将Unity的浮点数转换为整数值，比例为*100
    public static int WorldToLogic(float val)
    {
        return Mathf.RoundToInt(val * 100f);
    }

    // 将Unity的Vector3转换为自定义的NVector3结构，比例为*100
    public static NVector3 WorldToLogicN(Vector3 vector)
    {
        return new NVector3()
        {
            X = Mathf.RoundToInt(vector.x * 100),
            Y = Mathf.RoundToInt(vector.z * 100), // 注意：这里z变成了Y，y变成了Z，表明NVector3的坐标系可能与Unity的不同
            Z = Mathf.RoundToInt(vector.y * 100)
        };
    }

    // 将Unity的Vector3转换为Vector3Int，比例为*100
    public static Vector3Int WorldToLogic(Vector3 vector)
    {
        return new Vector3Int()
        {
            x = Mathf.RoundToInt(vector.x * 100),
            y = Mathf.RoundToInt(vector.z * 100), // 同样注意：z变成了y
            z = Mathf.RoundToInt(vector.y * 100)
        };
    }

    // 更新实体的位置、方向和速度，并返回是否发生了变更
    public static bool EntityUpdate(NEntity entity, UnityEngine.Vector3 position, Quaternion rotation, float speed)
    {
        NVector3 pos = WorldToLogicN(position); // 将位置转换为NVector3
        NVector3 dir = WorldToLogicN(rotation.eulerAngles); // 将旋转转换为NVector3
        int spd = WorldToLogic(speed); // 将速度转换为整数

        bool updated = false; // 标记是否进行了更新

        if (!entity.Position.Equal(pos)) // 检查位置是否发生变化
        {
            entity.Position = pos; // 更新位置
            updated = true; // 标记更新
        }
        if (!entity.Direction.Equal(dir)) // 检查方向是否发生变化
        {
            entity.Direction = dir; // 更新方向
            updated = true; // 标记更新
        }
        if (entity.Speed != spd) // 检查速度是否发生变化
        {
            entity.Speed = spd; // 更新速度
            updated = true; // 标记更新
        }

        return updated; // 返回是否更新
    }
}
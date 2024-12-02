using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 阵容类型
/// </summary>
public enum CampType
{
    None = 1000,//无阵营
    Right = 1001,//右边阵营
    Left = 1002,//左边阵营
   
}
//单位类型
public enum UnitType
{
    HostPlayer,//主机玩家
    OnLinePlayer,//联机玩家
    Town,//炮台
    Solider,//士兵
    Spawn,//主城
}

using System;
using System.Collections;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using UnityEngine;
using If = Unity.VisualScripting.If;

public class SeekManager : MonoBehaviour
{
    public Unit unit;
    public bool IfFindEnemy = false;
    private Unit index;
    private int roadSeekNum = 1; //只能寻一条路上的敌人

    /// <summary>
    /// 是否在同一条路
    /// </summary>
    /// <returns></returns>
    public bool IfOnTheRoad(Unit other)
    {
        if (other.exData.ut == UnitType.Spawn) return true;
        MapRoad road = unit.data.road;
        var gap = MathF.Abs(other.data.road - road);
        return gap < roadSeekNum;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //如果索引没有人，说明目前是一个人也没，进来一个立马赋值变成有人
        if (index == null)
        {
            if (other.gameObject.name == "FindEnemy") return;
            var otherUnit = other.GetComponentInParent<Unit>();
            if (otherUnit == null) return;
            
            if (unit == otherUnit) return;
            if (unit == null) return;
            if (unit.exData.ct == otherUnit.exData.ct) return; //如果是自己阵容的，那不算
            if (!IfOnTheRoad(otherUnit)) return;
            index = otherUnit;
            IfFindEnemy = true;
        }
        if (index != null)
        {
            if (index.data.currentHp <= 0)
            {
                index = null;
                IfFindEnemy = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //如果没人，退出也肯定没人，不需要运行
        if (index != null)
        {
            var otherUnit = other.GetComponentInParent<Unit>();
            //如果有人，并且等于目前锁定的这个，离开了那么立马判断为无人，为下一个人做准备
            if (index == otherUnit)
            {
                index = null;
                IfFindEnemy = false;
            }
        }
    }
}
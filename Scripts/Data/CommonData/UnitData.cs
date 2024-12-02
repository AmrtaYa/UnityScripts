using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 单位类型信息
/// </summary>
[Serializable]
public class UnitData
{
    //————————————————————————————城镇所需要的属性——————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
    /// <summary>
    /// 等级  城镇默认1级
    /// </summary>
    public short level;

    /// <summary>
    /// 移速
    /// </summary>
    public float Speed;


    //————————————————————————————城镇所需要的属性——————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————

    public MapRoad road; //在哪条道路上
    public float MaxHp;
    public float currentHp;
    public float mysteryBar; //奥义槽
    public float MaxMysteryBar; //放奥义所需要的条
    public float attack;
    public float defence; //防御力
    public float MaxExp; //升级所需要的经验值
    public float CurrentExp; //当前经验值
    public Vector2 forwardOffsetRange; //比如  刀砍人  需要向前偏移这个距离

    public float attackReach; //能够攻击到的距离
    public float attkInterval; //普通攻击之间的间隔
}

[Serializable]
public class UnitExData
{
    public CampType ct;
    public UnitType ut;
    public bool ifPool; //是否对象池生成的
    public bool IfEnemey = true;
    public int SlashMaxNum = 1;
    public SeekType commonFindWay; //寻敌（多路，还是单路）
}
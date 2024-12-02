using System;
using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;

public class SaveEntity : IDataBase
{
    [PrimaryKey] public int ID { get; set; }
    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime lastLoginDate { get; set; }

    /// <summary>
    /// 玩家名字  后期根据steam获取
    /// </summary>
    public string playerName { get; set; }

    /// <summary>
    /// 游玩时长， 记得转换成时间类型
    /// </summary>
    public int playTime { get; set; }

    /// <summary>
    /// 玩家等级
    /// </summary>
    public int level { get; set; }
    
    public int GetPrimKey()
    {
        return ID;
    }

    public Type[] dependEntity()
    {
        return null;
    }

    public IDataBase GetDefaultEntity(int id)
    {
        return new SaveEntity() { };
    }
}

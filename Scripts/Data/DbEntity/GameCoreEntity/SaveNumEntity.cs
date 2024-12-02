using System;
using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;

/// <summary>
/// 这个是用来记载目前有几个存档的，并非是玩家存档实体
/// </summary>
public struct SaveNumEntity : IDataBase
{
    [PrimaryKey] public int ID { get; set; }
    /// <summary>
    /// 存档数量   
    /// </summary>
    public int saveNum { get; set; }
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
        return new SaveNumEntity() {  ID = id,saveNum = 0};
    }
}

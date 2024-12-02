using System;
using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;

/// <summary>
/// 系统解锁Entity
/// </summary>
public struct SystemLockEntity : IDataBase
{
    [PrimaryKey] public int ID { get; set; }
    public string SystemName { get; set; }
    public bool Lock { get; set; }

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
        return new SystemLockEntity() { ID = id, Lock = true};
    }
}
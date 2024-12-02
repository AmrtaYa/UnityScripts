using System;
using System.Collections.Generic;
using SQLite4Unity3d;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public interface IDataBase
{
    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public int GetPrimKey();
    /// <summary>
    /// 依赖关系
    /// </summary>
    /// <returns></returns>
    public Type[] dependEntity();
    /// <summary>
    /// 默认值，当插入该实体到数据库时，插入默认值
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IDataBase GetDefaultEntity(int id);
}
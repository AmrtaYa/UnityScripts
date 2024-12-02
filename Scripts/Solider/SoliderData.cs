using System;
using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;

public class SoliderDataEntity : IDataBase
{
    [PrimaryKey] public int ID { get; set; }
    [PrimaryKey] public int soliderID { get; set; }
    public KeyCode kcSet { get; set; }
    public string SoliderName { get; set; }
    public string prefabPath;
    public string iconPath;//图标加载路径
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
        return new SoliderDataEntity()
        {
            ID = id,
            kcSet = KeyCode.None,
        };
    }

    public static SoliderDataEntity GetEmptyEntity()
    {
        return new SoliderDataEntity()
        {
            ID = -999,
            iconPath = "UI/Solider/EmptySoliderIcon",
            prefabPath = String.Empty,
            soliderID = -999,
            SoliderName = String.Empty,
            kcSet = KeyCode.None
        };
    }
}

public class SoliderManager
{
    public Unit host;
    public int maxSoliderNum = 10;
    private Dictionary<KeyCode, SoliderDataEntity> sGrids;
    protected static string excelConfigPath = "ExcelConfig/SoliderConfig.xlsx";

    public SoliderManager(Unit host)
    {
        this.host = host;
        sGrids = new Dictionary<KeyCode, SoliderDataEntity>(maxSoliderNum);
        DBManager.CreateTable<SoliderDataEntity>(GameMainEngine.Instance.saveID);
        UpdateGrids();
    }

    public SoliderDataEntity GetData(KeyCode kc)
    {
        return sGrids[kc];
    }

    public Dictionary<KeyCode, SoliderDataEntity> GetDirs()
    {
        return this.sGrids;
    }

    /// <summary>
    /// 更新里面的信息
    /// </summary>
    public void UpdateGrids()
    {
        sGrids.Clear();
        var gridsInfo = DBManager.GetAllEntity<SoliderDataEntity>(
            (e) => { return e.kcSet != KeyCode.None; },
            GameMainEngine.Instance.saveID);
        ExcelConfigFactory<SoliderDataEntity> factory = new ExcelConfigFactory<SoliderDataEntity>(excelConfigPath);
        var soliderConfigs = factory.LoadData(ConfigToSoliderDataEntity, 1);

        foreach (var v in gridsInfo)
        {
            soliderConfigs[v.soliderID].kcSet = v.kcSet;
            soliderConfigs[v.soliderID].ID = v.ID;
            sGrids.Add(v.kcSet, soliderConfigs[v.soliderID]);
        }
    }

    protected SoliderDataEntity ConfigToSoliderDataEntity(List<object> arg)
    {
        if (arg[0] == null) return null;
        SoliderDataEntity e = new SoliderDataEntity();
        e.soliderID = Convert.ToInt32(arg[0]);
        e.SoliderName = arg[1].ToString();
        e.prefabPath = "Prefab/Solider/" + arg[2].ToString();
        e.iconPath = "UI/Solider/" + arg[3];
        return e;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISoliderManager : SoliderManager
{
    public Dictionary<int, SoliderDataEntity> spawnHasSoliders;
    

    public AISoliderManager(Unit host) : base(host)
    {
        InitDir();
    }

    private void InitDir()
    {
        spawnHasSoliders = new Dictionary<int, SoliderDataEntity>();
        ExcelConfigFactory<SoliderDataEntity> factory = new ExcelConfigFactory<SoliderDataEntity>(excelConfigPath);
        var soliderConfigs = factory.LoadData(ConfigToSoliderDataEntity, 1);
        
        spawnHasSoliders.Add(0,soliderConfigs[0]);//后面需要自己手动配置
    }


    public SoliderDataEntity GetRandomData()
    {
        return spawnHasSoliders[Random.Range(0, spawnHasSoliders.Count)];
    }
}

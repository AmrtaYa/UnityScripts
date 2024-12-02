using System;
using System.Collections;
using System.Collections.Generic;
using GJC.Helper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitManager : SingleTon<UnitManager>
{
    private PlayerSpawnUnit playerSpawnPri;

    public PlayerSpawnUnit playerSpawn
    {
        get
        {
            if (playerSpawnPri == null)
                playerSpawnPri = GameObject.FindWithTag("PlayerSpawn").GetComponent<PlayerSpawnUnit>();
            return playerSpawnPri;
        }
    }

    private EnemySpawnUnit enemySpawnPri;

    public EnemySpawnUnit enemySpawn
    {
        get
        {
            if (enemySpawnPri == null)
                enemySpawnPri = GameObject.FindWithTag("EnemySpawn").GetComponent<EnemySpawnUnit>();
            return enemySpawnPri;
        }
    }


    public override void init()
    {
        base.init();
    }


    //添加Unit
    public Unit AddUnit(SoliderDataEntity data, CampType CT, MapRoad road, Vector2 pos = default, GameObject go = null)
    {
        //从PlayerUnit根据ID取出Unit信息
        if (go == null)
            go = LoadWay.NormalLoad(data.prefabPath);
        Unit u = go.GetComponent<Unit>();
        u.soliderData = data;//这个一定要在init之前
        u.Init();
#if UNITY_EDITOR
        if (u == null) Debug.LogError(go.name + "未找到Unit组件");
#endif
        Unit spawnTr = CampToSpawn(CT);
        bool IfEnemy = spawnTr.exData.IfEnemey;
        if (pos == default)
            u.entity.transform.position = new Vector3(spawnTr.transform.position.x, (float)road, SpiteLayer.LayerOne);
        else
            u.entity.transform.position = new Vector3(pos.x, pos.y, SpiteLayer.LayerOne);

        u.spawnUnit = spawnTr;
        u.data.road = road;
        u.exData.IfEnemey = IfEnemy;
        u.exData.ct = CT;
        if (IfEnemy) 
            enemySpawn.UnitDir[road].Add(u);
        else
            playerSpawn.UnitDir[road].Add(u);
       
       
        
        return u;
    }

    /// <summary>
    /// 通过左右获取主城
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private Unit CampToSpawn(CampType ct)
    {
        if (ct == playerSpawn.exData.ct)
            return playerSpawn;
        else
            return enemySpawn;
    }

    public Unit AddUnitWithPool(SoliderDataEntity data, CampType exDataCt, MapRoad road, Vector2 pos = default)
    {
        GameObject prefab = LoadWay.ResLoad<GameObject>(data.prefabPath);
        GameObject go = GameObjectPool.Instance.Get(data.SoliderName, prefab, pos, Quaternion.identity);
        Unit unit = AddUnit(data, exDataCt, road, pos, go);
        unit.exData.ifPool = true;
        return unit;
    }

    public void RemoveUnit(Unit unit,float delay=0.0f)
    {
        var dir = unit.exData.IfEnemey ? enemySpawn.UnitDir : playerSpawn.UnitDir;
        dir[unit.data.road].Remove(unit);
        if (unit.exData.ifPool)
            GameObjectPool.Instance.Release(unit.gameObject,delay);
        else
            Destroy(unit.gameObject,delay);
    }
}

public class SpiteLayer
{
    public static float LayerOne = -0.1f;
}
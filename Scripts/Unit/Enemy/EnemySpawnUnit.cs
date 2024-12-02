using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnUnit : AIUnit
{
   //这个只负责小兵，其余的还有  主城，主角的角色等。
   public Dictionary<MapRoad, List<Unit>> UnitDir; //活着的单位列表
   public MoneyManager mMgr;
   public AISoliderManager soliderManager;
   public float stepSpeed = 5.0f;
   public override void Init()
   {
      data = new UnitData() { MaxHp = 5000,currentHp = 5000};
      exData = new UnitExData() { ct = CampType.Right,ut = UnitType.Spawn};
      mMgr = new MoneyManager(this);
      defenceC = new SpawnDefenceComponent(this);
      soliderManager = new AISoliderManager(this);
      InitUnitDir();
      base.Init();
   }

   protected override void CreateFsm()
   {
      //方便测试，暂且不使用
      fsmBase = new EnemySpawnFSMBase(this);
   }

   private void InitUnitDir()
   {
      UnitDir = new Dictionary<MapRoad, List<Unit>>();
      var roads = Enum.GetValues(typeof(MapRoad));
      foreach (MapRoad r in roads)
      {
         UnitDir.Add(r, new List<Unit>());
      }
   }

   public Unit CreateRandomSolider()
   {
      MapRoad road = (MapRoad)Random.Range(-3,2);
      SoliderDataEntity solider = soliderManager.GetRandomData();
      var go = UnitManager.Instance.AddUnitWithPool(solider, exData.ct,road);
      return go;
   }
}

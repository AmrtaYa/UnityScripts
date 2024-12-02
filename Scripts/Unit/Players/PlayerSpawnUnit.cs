using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using UnityEngine;
using UnityEngine.UI;
using If = Unity.VisualScripting.If;

/// <summary>
/// 主城Unit
/// </summary>
public class PlayerSpawnUnit : Unit
{
    public KeyCode currentUnitChoose = KeyCode.None; //挂起的名字  目前是选择了哪个按键的士兵

    public bool ifPutUp = false;

    //这个只负责小兵，其余的还有  主城，主角的角色等。
    public Dictionary<MapRoad, List<Unit>> UnitDir; //活着的单位列表
    
    public override void Init()
    {
        base.Init();
        data = new UnitData() { MaxHp = 5000, currentHp = 5000 };
        exData = new UnitExData() { ct = CampType.Left, ut = UnitType.Spawn };
        defenceC = new SpawnDefenceComponent(this);
        InitUnitDir();
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

    public void PutUpSolider(KeyCode unitKc)
    {
        if (unitKc == KeyCode.None) return;
       var targetBtnUI = GameMainEngine.Instance.player.fightUI.shows.solider_kcToBtn[unitKc];


        if (currentUnitChoose == unitKc)
        {
            Log.EditorLog("取消了挂起角色,按键为:" + unitKc);
            currentUnitChoose = KeyCode.None;
            ifPutUp = false;
            SoliderGridsUIAnim.SoliderGridsSmall(targetBtnUI.transform.parent);
            return;
        }
        else if(currentUnitChoose!=KeyCode.None)
        {
            var lastBtnUI = GameMainEngine.Instance.player.fightUI.shows.solider_kcToBtn[currentUnitChoose];
            ifPutUp = false;
            SoliderGridsUIAnim.SoliderGridsSmall(lastBtnUI.transform.parent);
        }

        Log.EditorLog("挂起了角色,按键为:" + unitKc);
        this.currentUnitChoose = unitKc;
        ifPutUp = true;
        SoliderGridsUIAnim.SoliderGridsBigger(targetBtnUI.transform.parent);
       
    }

    /// <summary>
    /// 生成士兵
    /// </summary>
    /// <param name="CardID">士兵类型ID</param>
    /// <param name="road">出现在哪个道路</param>
    public void CreateSoldier(Vector2 pos = default)
    {
        if (!ifPutUp) return; //如果没有挂起的角色信息，就不生成
        SoliderDataEntity data = GameMainEngine.Instance.player.sManager.GetData(currentUnitChoose);
        MapRoad road = MapManager.Instance.MouseToInput(Input.mousePosition);
        if ((int)road > 1 || (int)road < -3) //超出范围，没点到路线上，那么就不生成
            return;

        UnitManager.Instance.AddUnitWithPool(data, exData.ct, road, pos);

        ifPutUp = false;
        var targetBtnUI = GameMainEngine.Instance.player.fightUI.shows.solider_kcToBtn[currentUnitChoose];
        SoliderGridsUIAnim.SoliderGridsSmall(targetBtnUI.transform.parent);
        //Log.EditorLog("取消了挂起角色,按键为:" + currentUnitChoose);
        currentUnitChoose = KeyCode.None;
      
      
    }

    public void CancelSolider()
    {
        if (currentUnitChoose == KeyCode.None) return;
       // Log.EditorLog("取消了挂起角色,按键为:" + currentUnitChoose);
        var targetBtnUI = GameMainEngine.Instance.player.fightUI.shows.solider_kcToBtn[currentUnitChoose];
        currentUnitChoose = KeyCode.None;
        ifPutUp = false;
        SoliderGridsUIAnim.SoliderGridsSmall(targetBtnUI.transform.parent);
    }
}
using GJC.Helper;
using Unity.VisualScripting;
using UnityEngine;
using If = OfficeOpenXml.FormulaParsing.Excel.Functions.Logical.If;


public class HostPlayerlMouseSelect : BaseManualSelect
{
    public override void SelectUpdate(Skill skill)
    {
        base.SelectUpdate(skill);
        var Pos3D = GameMainEngine.Instance.player.playerCamera.ScreenToWorldPoint(MapManager.Instance.MousePosition());
        skill.transform.position = new Vector3(Pos3D.x, Pos3D.y, -5);
    }
}

/// <summary>
/// 玩家自己鼠标选区算法
/// </summary>
public class BaseManualSelect : IRangeSelect
{
    private AreaType aeroType;



    public void SelectInit(Skill skill)
    {
        aeroType = skill.data.area;
        var sprite = LoadWay.ResLoad<Sprite>("Skill/SkillArea/" + skill.data.area);

        skill.renderer.sprite = sprite;
    }


    public virtual void SelectUpdate(Skill skill)
    {
        //var selectUIPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -5);
    }
}
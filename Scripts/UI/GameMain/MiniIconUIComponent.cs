using GJC.Helper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MiniIconUIComponent : UIComponent, IRsetable,IRelease
{
    public Unit owner;
    private Transform entity;
    private Image iconImg;

    public override void Init()
    {
        base.Init();
       
    }

    public void UpdateByPool()
    {
        if (owner == null) return;
        entity = owner.transform.FindTheTfByName("Entity");
        iconImg = transform.FindTheTfByName<Image>("Icon");
        if (owner.soliderData != null)
            iconImg.sprite = LoadWay.ResLoad<Sprite>(owner.soliderData.iconPath);
    }

    protected override void _Update()
    {
        base._Update();
        if (entity == null) return;
        //这边到时候建议换成 jobs多线程更新计算位置
        transform.position = MapManager.Instance.MapPosToMiniMapPos(entity.position);
    }

    public void OnRest()
    {
     
    }

    public void OnRelease()
    {
        owner = null;
    }
}
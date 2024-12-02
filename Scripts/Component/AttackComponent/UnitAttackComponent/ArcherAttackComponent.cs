using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GJC.Helper;
using UnityEngine;

public class ArcherAttackComponent : AttackComponent
{
    private GameObject arrow;
    private Transform armL;
    private Transform armR;
    private Transform arrowPos;
    private bool IfReadyBow = false;

    public ArcherAttackComponent(Unit unit) : base(unit)
    {
    }

    public override void Init()
    {
        arrow = LoadWay.ResLoad<GameObject>("Prefab/AtkComponent/" + unit.GetType() + "Arrow");
        var upper = unit.character.transform.FindTheTfByName("Upper");
        arrowPos = unit.transform.FindTheTfByName("ArrowPos");
        armL = upper.FindTheTfByName("ArmL");
        armR = upper.FindTheTfByName("ArmR[1]");
    }

    public override void AddEvent()
    {
        string str = unit.GetType().ToString();
        string bowFireAnim = "ReadyBowRelease" + str;
        string bowReadyAnim = "ReadyBow" + str;
        // unit.animMgr.AddStartAction(slashMelee1h,
        //     () => { unit.IfHegemony = true; });
        unit.animMgr.AddStartAction(bowFireAnim, FireArrow);
        unit.animMgr.AddStartAction(bowReadyAnim, RotateBowAnim);
        unit.animMgr.AddEndAction(bowReadyAnim, RotateBowBack);
    }

    //射箭
    public void FireArrow()
    {
        Vector2 pos = unit.entity.transform.position;
        var go = GameObjectPool.Instance.Get("BaseArcherArrow", arrow, arrowPos.position,
            Quaternion.identity).GetComponent<BowCharge>();
        go.owner = unit;
        IfReadyBow = false;
    }

    //旋转弓的角度
    public void RotateBowAnim()
    {
        if (IfReadyBow) return;
        RotateArmToReadyAttack();
        IfReadyBow = true;
    }

    public void RotateBowBack()
    {
        if (!IfReadyBow) return;
        RotateArmToReadyAttack(true);
    }

    //因为不是每个角色都需要旋转手臂的，所以就单独放在弓箭手攻击组件中
    public void RotateArmToReadyAttack(bool ifBack = false)
    {
        int angle = ifBack ? -15 : 15;
        armL.Rotate(new Vector3(0, 0, angle));
        armR.Rotate(new Vector3(0, 0, angle));
    }

    //这是调用动画
    public override void AICommonAttack()
    {
        unit.character.GetReady();
        unit.StartCoroutine(unit.character.Shoot());
    }


    private static float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;

        return angle;
    }
}
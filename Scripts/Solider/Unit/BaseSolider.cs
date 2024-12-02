using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseSolider : AIUnit
{
    public override void Init()
    {
        base.Init();
        data = new UnitData()
        {
            Speed = 0.5f,
            currentHp = 1,
            MaxHp = 500,
            attackReach = 2.5F,
            attkInterval = 3.5f,
            attack = 1
        };
        exData = new UnitExData()
        {
            commonFindWay = SeekType.Ray,
            ut =  UnitType.Solider,
        };
        attackC = new AttackComponent(this);
        animMgr = new UnitAnimManager(this);
        defenceC = new DefenceComponent(this);
        moveC = new AIMoveComponent(this);
        
        animMgr.Init();
        attackC.Init();
        
        GameMainEngine.Instance.player.fightUI.miniMap.AddMiniMapIcon(this);
    }

    protected override void _Update()
    {
        base._Update();
    }
}
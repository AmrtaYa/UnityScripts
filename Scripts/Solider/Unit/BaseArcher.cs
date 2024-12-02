using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseArcher : AIUnit
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
            attack = 3,
            forwardOffsetRange = new Vector2(0.5f,0.7f)
        };
        exData = new UnitExData()
        {
            commonFindWay = SeekType.Ray,
            ut =  UnitType.Solider,
            SlashMaxNum = 1
        };
        attackC = new ArcherAttackComponent(this);
        animMgr = new UnitAnimManager(this);
        defenceC = new DefenceComponent(this);
        moveC = new AIMoveComponent(this);
        
        attackC.Init();
        animMgr.Init();
        GameMainEngine.Instance.player.fightUI.miniMap.AddMiniMapIcon(this);
    }

    protected override void _Update()
    {
        base._Update();
    }
}

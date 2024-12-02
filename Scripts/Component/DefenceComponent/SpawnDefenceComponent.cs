using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDefenceComponent : DefenceComponent
{
    public override void Damage(float atk)
    {
        base.Damage(atk);
        Debug.Log("spawnAttacked");
    }

    public override void Dead()
    {
        base.Dead();
        //GameOver
    }

    public SpawnDefenceComponent(Unit unit) : base(unit)
    {
    }
}

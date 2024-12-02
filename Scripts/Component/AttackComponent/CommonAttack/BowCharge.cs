using System;
using System.Collections;
using System.Collections.Generic;
using GJC.Helper;
using UnityEngine;

public class BowCharge : MonoBehaviour, IRsetable, IRelease
{
    public Unit owner;
    private int currentNum = 0; //当前选中敌人数量
    private bool canBeDamage = true;

    private void Update()
    {
        this.transform.position += transform.right*Time.deltaTime*2.0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (owner == null) return;
        if (other.gameObject.name !="Entity")
            return;
        if (other.gameObject == owner.entity.gameObject) return;
        if (!canBeDamage) return;
        var unit = other.transform.parent.GetComponent<Unit>();
        if (unit == null) return;

        if (unit.exData.ct == owner.exData.ct) return;

        if (!((AIUnit)owner).seekManager.IfOnTheRoad(unit)) return;
        if (unit.data.currentHp > 0)
            currentNum++;
        if (currentNum > owner.exData.SlashMaxNum)
        {
            canBeDamage = false;
          
        }

        if (canBeDamage)
            unit.defenceC.Damage(owner.data.attack);
        if (currentNum == owner.exData.SlashMaxNum)
        {
            GameObjectPool.Instance.Release(this.gameObject);
        }
    }
    public void OnRest()
    {
        GameObjectPool.Instance.CancelRelease(gameObject);
        GameObjectPool.Instance.Release(this.gameObject, 5f);
    }

    public void OnRelease()
    {
        owner = null;
        currentNum = 0;
        canBeDamage = true;
    }
}

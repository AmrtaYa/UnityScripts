using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using GJC.Helper;
using UnityEngine;

public class DefenceComponent
{
    public Unit unit;
    public DamageShowUIComponent HpUI;
    public bool IfDamaged = false;
    public DefenceComponent(Unit unit)
    {
        this.unit = unit;
    }

    public virtual void Damage(float atk)
    {
        if (unit.data.currentHp <= 0) return;
        float result = atk;
        if (!unit.IfHegemony) //如果没有霸体，就被打断
            unit.character?.Hited(); //被击打的动画
        //UI调用数值
        var uiComponent = UIManager.Instance.AddUIWithPool
            <DamageShowUIComponent>("Prefab/UI/Common/DamageShow", UILayer.Top, default,
                (ui) =>
                {
                    ui.owner = unit;
                    ui.damageNum = result;
                });
        if (HpUI == null)
        {
            //生成临时血条
            
        }

        unit.data.currentHp -= result;
        IfDamaged = true;
        //状态机里面调用死亡逻辑,并非在这调用
    }

    public virtual void Dead()
    {
        int num = Random.Range(6, 8);
        unit.character.SetState((CharacterState)num);
    }
}
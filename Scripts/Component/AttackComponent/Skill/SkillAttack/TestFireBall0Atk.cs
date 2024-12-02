using System;
using GJC.Helper;
using Unity.VisualScripting;
using UnityEngine;

public class TestFireBall0Atk : SkillAttack
{
    private Transform fireBall;
    private Transform bombEffect;
    private float fireBallHeight = 8.0f;
    private bool IfLand = false; //火球是否落地
    private CircleCollider2D cod;
    public override void OnRest()
    {
        base.OnRest();
        if (owner == null) return;
        speed = 15;
        IfLand = false;
        fireBall = transform.FindTheTfByName("MysticOrangeOBJ");
        bombEffect = transform.FindTheTfByName("MysticExplosionOrange");
        cod = GetComponent<CircleCollider2D>();
        fireBall.localPosition = new Vector3(0, fireBallHeight, 0);
        fireBall.gameObject.SetActive(true);
        bombEffect.gameObject.SetActive(false);
        cod.enabled = false;
    }

    protected override void Update()
    {
        base.Update();
        if (owner == null) return;
        fireBall.position -= new Vector3(0, Time.deltaTime * speed, 0);
        if (fireBall.position.y <= 0 && !IfLand)
        {
            fireBall.gameObject.SetActive(false);
            bombEffect.gameObject.SetActive(true);
            SkillLogic();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!bombEffect.gameObject.activeSelf)
            return;
        if(!IfLand)return;
        if (owner == null) return;
        if (other.gameObject.name != "Entity")
            return;
        if (other.gameObject == owner.entity.gameObject) return;
        var unit = other.transform.parent.GetComponent<Unit>();
        if (unit == null) return;
        if (unit.exData.ct == owner.exData.ct) return;
        if (unit.data.currentHp <= 0) return;
        unit.defenceC.Damage(owner.data.attack);
        
    }

    public override void OnRelease()
    {
        base.OnRelease();
        cod.enabled = false;
        owner = null;
    }

    private void SkillLogic()
    {
        IfLand = true;
        cod.enabled = true;
        GameObjectPool.Instance.Release(gameObject, 0.75f);
    }
}
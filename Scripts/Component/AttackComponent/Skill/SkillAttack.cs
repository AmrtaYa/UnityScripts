using System;
using GJC.Helper;
using UnityEngine;

public class SkillAttack : MonoBehaviour , IRsetable,IRelease
{
    public Unit owner;
    protected float speed = 1.0f;
    public Skill skill;

    protected virtual void Update()
    {
        
    }

    public virtual void OnRest()
    {
        
        
    }

    public virtual void OnRelease()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using GJC.Helper;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour,IRelease,IRsetable
{
    /// <summary>
    /// 无需new，会在manager内自动new
    /// </summary>
    public UnitData data;

    public UnitExData exData; //额外属性
    private bool ifInit = false;
    public bool IfHegemony = false;//是否霸气
    public PhysicController pyhsic;
    public MoveComponent moveC;
    public AttackComponent attackC;
    public DefenceComponent defenceC;
    [HideInInspector] public Unit spawnUnit; //属于的主城
#if UNITY_EDITOR
    public Vector2 move; //当前移动速度
#endif
    //AI士兵专用Entity
    public SoliderDataEntity soliderData;
   [HideInInspector] public Character character;
   [HideInInspector] public UnitAnimManager animMgr;
   [HideInInspector] public Transform entity;
   [HideInInspector] public Transform damageUIShowPos;
   [HideInInspector]  public Transform body;
    private void Start()
    {
        if (!ifInit) Init();
    }
    
    public virtual void Init()
    {
        ifInit = true;
        entity = gameObject.transform.FindTheTfByName("Entity");
        damageUIShowPos = entity.FindTheTfByName("DamageShowPos");
        body = entity.transform.FindTheTfByName("Body");
        character = this.GetComponentInChildren<Character>();
        
    }
    

    private void Update()
    {
        if (pyhsic != null)
            pyhsic.Update();
        _Update();

#if UNITY_EDITOR
        UnitTestShow();
#endif
    }

    private void UnitTestShow()
    {
        if (moveC == null) return;
#if UNITY_EDITOR
        move = moveC.moveVec;

#endif
    }

    protected virtual void _Update()
    {
    }


    public virtual void OnRelease()
    {
        if (character == null) return;
        if (character.Animator == null) return;
        if (body == null) return;
        //下次使用能够重置位置，避免transform出错无法修改
        character.Animator.enabled = false;
        body.localRotation = Quaternion.Euler(Vector3.zero);
        body.transform.localPosition = Vector3.zero;
    }

    public virtual void OnRest()
    {
        if (character == null) return;
        if (character.Animator == null) return;
        character.SetState(CharacterState.Idle);
        character.SetExpression("Default");
        character.Animator.enabled = true;
        
    }
}
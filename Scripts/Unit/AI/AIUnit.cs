using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using FSM;
using GJC.Helper;
using Unity.VisualScripting;
using UnityEngine;

//AI单位
public class AIUnit : Unit
{
    [SerializeField] protected FSMBase fsmBase; //AI状态机
    public SeekManager seekManager;

    public override void Init()
    {
        base.Init();
        var finderTr= transform.FindTheTfByName("FindEnemy");
        if (finderTr != null)
        {
            seekManager = finderTr.GetOrAddComponent<SeekManager>();
            seekManager.unit = this;
        }
        CreateFsm();
    }

    protected virtual void CreateFsm()
    {
        fsmBase = new FSMBase(this);
    }

    protected override void _Update()
    {
        base._Update();
        fsmBase?._Update();
    }

    public override void OnRelease()
    {
        base.OnRelease();
        fsmBase.eInfo.firstInit = true;
    }
}
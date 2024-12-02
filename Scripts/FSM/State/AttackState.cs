using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using UnityEngine;

namespace FSM
{
    public class AttackState : FSMState
    {
        public override void Init()
        {
            base.Init();
            this.ID = StateID.Attack;
        }

        public override void EnterState(FSMBase fsm)
        {
            base.EnterState(fsm);
            //如果是第一次生成
            fsm.eInfo.firstInit = false;
            fsm.UnitInfo.character.SetState(CharacterState.Idle);
        }

        public override void ActionState(FSMBase fsm)
        {
            base.ActionState(fsm);
            if (fsm.eInfo. intervalTime >= fsm.UnitInfo.data.attkInterval)
            {
                fsm.UnitInfo.attackC.AICommonAttack();
                fsm.eInfo. intervalTime = 0.0f;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using UnityEngine;

namespace FSM
{
    public class MoveFowardState : FSMState
    {
        public override void Init()
        {
            base.Init();
            this.ID = StateID.MoveFoward;
        }

        public override void EnterState(FSMBase fsm)
        {
            base.EnterState(fsm);
            fsm.UnitInfo.character.SetState(CharacterState.Walk);
        }

        public override void ActionState(FSMBase fsm)
        {
            base.ActionState(fsm);
            Vector2 dir = new Vector2(1, 0);
            fsm.UnitInfo.moveC?.Move(dir);
           
        }
    }
}
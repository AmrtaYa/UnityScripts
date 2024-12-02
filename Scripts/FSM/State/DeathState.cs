using System.Collections;
using System.Collections.Generic;
using FSM;
using GJC.Helper;
using UnityEngine;

namespace FSM
{
    public class DeathState : FSMState
    {
        public override void Init()
        {
            base.Init();
            ID = StateID.Death;
        }

        public override void EnterState(FSMBase fsm)
        {
            base.EnterState(fsm);
            fsm.UnitInfo.defenceC.Dead();
            UnitManager.Instance.RemoveUnit(fsm.UnitInfo,3.0f);
            GameMainEngine.Instance.player.fightUI.miniMap.ReleaseMiniMapIcon(fsm.UnitInfo,3.0f);
        }
    }
}
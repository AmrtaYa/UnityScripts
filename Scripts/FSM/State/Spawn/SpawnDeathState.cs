using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class SpawnDeathState : FSMState
    {
        public override void Init()
        {
            base.Init();
            ID = StateID.SpawnDeath;
        }

        public override void EnterState(FSMBase fsm)
        {
            base.EnterState(fsm);
            //结算计算掉落物
        }
    }
}
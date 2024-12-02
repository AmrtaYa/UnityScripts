using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class GotoSoliderTrigger : FSMTrigger
    {
        public override void Init()
        {
            ID = TriggerID.GotoSolider;
        }

        public override bool Trigger(FSMBase fsm)
        {
            return ((EnemySpawnFSMBase)fsm).sData.GetStep()== SpawnStep.BuildSolider;
        }
    }
}


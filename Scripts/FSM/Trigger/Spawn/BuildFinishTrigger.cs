using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class BuildFinishTrigger : FSMTrigger
    {
        public override void Init()
        {
            ID = TriggerID.BuildFinish;
        }

        public override bool Trigger(FSMBase fsm)
        {
            return ((EnemySpawnFSMBase)fsm).sData.GetStep()== SpawnStep.None;
        }
    }
}


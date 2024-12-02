using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    /// <summary>
    /// 主城AI选择如何操作状态
    /// </summary>
    public class SpawnWhereDoState : FSMState
    {
        public override void Init()
        {
            base.Init();
            ID = StateID.SpawnWhereDo;
        }

        public override void EnterState(FSMBase fsm)
        {
            base.EnterState(fsm);
            EnemySpawnFSMBase f = (EnemySpawnFSMBase)fsm;
            f.sData.GetPolicyStep();
        }
    }
}
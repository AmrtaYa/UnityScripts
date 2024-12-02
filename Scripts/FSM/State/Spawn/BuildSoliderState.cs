using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 出兵状态AI
/// </summary>
namespace FSM
{
    //属于step类
    public class BuildSoliderState : FSMState
    {
        public override void Init()
        {
            base.Init();
            ID = StateID.BuildSolider;
        }

        public override void EnterState(FSMBase fsm)
        {
            base.EnterState(fsm);
            EnemySpawnFSMBase f = (EnemySpawnFSMBase)fsm;
            f.spawn.CreateRandomSolider();
            //重置策略步数
            f.sData.ResetStep();
        }
    }
}
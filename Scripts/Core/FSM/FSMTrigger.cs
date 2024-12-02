using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public abstract class FSMTrigger
    {
        public FSMTrigger()
        {
            Init();
        }

        public TriggerID ID;
        public abstract void Init();
        public abstract bool Trigger(FSMBase fsm);
    }
    public enum TriggerID
    {
        NoHeath,

        ReachAttack,
        ReachMystery,
        ReleaseMystery,
        LostAttack,
        
        //Spawn
        GotoSolider = 1001,//允许创建士兵
        BuildFinish = 10012,//指令执行结束
    }
}


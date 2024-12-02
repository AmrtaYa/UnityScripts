using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace FSM
{
    /// <summary>
    /// 状态机种的单个状态
    /// </summary>
    [Serializable]
    public abstract class FSMState
    {
        private Dictionary<FSMTrigger, FSMState> Triggers = new Dictionary<FSMTrigger, FSMState>();
        private FSMTrigger[] allTrigger; //所有的key，防止多次toArray消耗性能
        public StateID ID;

        public FSMState()
        {
        }

        public virtual void Init()
        {
            allTrigger = Triggers.Keys.ToArray();
        }

        public Dictionary<FSMTrigger, FSMState> GetTriggers()
        {
            return Triggers;
        }

        public void AddTriggers(FSMTrigger trigger, FSMState state)
        {
            Triggers.Add(trigger, state);
        }

        public void CheckTrigger(FSMBase fsm)
        {
            int? len =
                allTrigger?.Length;
            for (int i = 0; i < len; i++)
            {
                if (allTrigger[i].Trigger(fsm))
                {
                    fsm.currentState.ExitState(fsm);
                    Triggers.TryGetValue(allTrigger[i], out fsm.currentState);
                    fsm.currentState.EnterState(fsm);
                    break;
                }
            }
        }

        public virtual void EnterState(FSMBase fsm)
        {
        }

        public virtual void ActionState(FSMBase fsm)
        {
        }

        public virtual void ExitState(FSMBase fsm)
        {
        }
    }

    public enum StateID
    {
        //UnitState
        MoveFoward = 0, //向前移动状态
        Mystery = 1, //奥义
        Attack = 2, //普通攻击
        Death = 3, //死亡状态


        //SpawnState
        SpawnWhereDo = 1001,

        BuildSolider = 1002, //主城出兵选择策略状态

        SpawnDeath = 1100,

        //Extern
        AnyState = 9999,
    }
}
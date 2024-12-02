using System;
using System.Collections;
using System.Collections.Generic;
using GJC.Helper;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace FSM
{
    /// <summary>
    /// AI状态机
    /// </summary>
    [Serializable]
    public class FSMBase
    {
        /// <summary>
        /// 角色信息
        /// </summary>
        public AIUnit UnitInfo;

        /// <summary>
        /// 额外需要的信息
        /// </summary>
        public ExternFSMInfo eInfo;

        /// <summary>
        /// 寻路组件
        /// </summary>
        [HideInInspector] public NavMeshAgent agent;

        public FSMState currentState; //当前状态
        [SerializeField] private StateID currentID;

        [SerializeField] protected StateID DefaultStateID = StateID.MoveFoward; //默认状态，可在Unity可视化内调整

        private FSMState[] allStates;

        public FSMBase(AIUnit unit)
        {
            UnitInfo = unit;
            FSMInit(unit);
        }

        protected virtual void FSMInit(AIUnit unit)
        {
            //-1代表未初始化
            eInfo = new ExternFSMInfo() { intervalTime =  -1,firstInit = true};
            agent = UnitInfo.GetComponent<NavMeshAgent>();
#if UNITY_EDITOR
            //if(agent==null)Debug.LogError("未找到agent组件，请添加寻路组件");
#endif
            //通过状态机生产工厂初始化所有状态
            allStates = FSMFactory.GetState(unit);

            SetDefaultState();
            //初始化默认状态
            currentState = allStates.FindObject((state) => { return state.ID == DefaultStateID; }); //找到默认状态
            currentState.EnterState(this); //执行进入默认状态方法
        }

        protected virtual void SetDefaultState()
        {
            DefaultStateID = StateID.MoveFoward;
        }

        /// <summary>
        ///
        /// </summary>
        public void _Update()
        {
            if (currentState == null) return;

            if (eInfo.intervalTime <= UnitInfo.data.attkInterval + 1)
            {
                eInfo.intervalTime += Time.deltaTime;
                if (eInfo.firstInit)
                    eInfo.intervalTime = UnitInfo.data.attkInterval + 1;
            }

            currentID = currentState.ID;
            currentState.CheckTrigger(this);
            currentState.ActionState(this);
        }
    }

    [Serializable]
    public struct ExternFSMInfo
    {
        /// <summary>
        /// 巡逻目标点
        /// </summary>
        //public Transform[] loiterTarget;

        public float intervalTime;

        public bool firstInit ;
    }
}
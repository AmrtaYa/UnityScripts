using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FSM
{
    public class EnemySpawnFSMBase : FSMBase
    {
        public EnemySpawnData sData;
        public EnemySpawnUnit spawn;

        public EnemySpawnFSMBase(EnemySpawnUnit unit):base(unit)
        {
           
        }
        protected override void FSMInit(AIUnit unit)
        {
            spawn = (EnemySpawnUnit)unit;
            sData = new EnemySpawnData(spawn);
            base.FSMInit(unit);
        }

        protected override void SetDefaultState()
        {
            DefaultStateID = StateID.SpawnWhereDo;
        }
    }

    public class EnemySpawnData
    {
        private SpawnStep step;
        private int startNum = 1;
        private int stepAllNum = 1; //后期多的话改成自动识别
        private EnemySpawnUnit spawn;

        public EnemySpawnData(EnemySpawnUnit unit)
        {
            spawn = unit;
            step = SpawnStep.None;
        }

        public void GetPolicyStep()
        {
            //目前以随机为主
            int sIndex = Random.Range(startNum, stepAllNum);
            step = (SpawnStep)sIndex;
        }

        public SpawnStep GetStep()
        {
            return step;
        }

        public async void ResetStep()
        {
            await UniTask.WaitForSeconds(spawn.stepSpeed);
            step = SpawnStep.None;
        }
    }

    public enum SpawnStep
    {
        None = 0,
        BuildSolider = 1,
    }
}
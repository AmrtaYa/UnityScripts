using UnityEngine;

namespace FSM
{
    public class NoHeathTrigger : FSMTrigger
    {
        public override void Init()
        {
            ID = TriggerID.NoHeath;
        }

        public override bool Trigger(FSMBase fsm)
        {
            return fsm.UnitInfo.data.currentHp<=0;
        }
    }
}
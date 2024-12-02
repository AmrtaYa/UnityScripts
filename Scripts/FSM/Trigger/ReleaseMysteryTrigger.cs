using UnityEngine;

namespace FSM
{
    public class ReleaseMysteryTrigger : FSMTrigger
    {
        public override void Init()
        {
            ID = TriggerID.ReleaseMystery;
        }

        public override bool Trigger(FSMBase fsm)
        {
            return false;
        }
    }
}
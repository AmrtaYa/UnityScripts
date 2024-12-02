using UnityEngine;

namespace FSM
{
    public class LostAttackTrigger : FSMTrigger
    {
        public override void Init()
        {
            ID = TriggerID.LostAttack;
        }

        public override bool Trigger(FSMBase fsm)
        {
            if (fsm.UnitInfo.attackC == null) return false;
            return !fsm.UnitInfo.attackC.FinderEnemy();
        }
    }
}
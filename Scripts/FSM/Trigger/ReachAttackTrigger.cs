using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using UnityEngine;

namespace FSM
{
    public class ReachAttackTrigger : FSMTrigger
    {
        public override void Init()
        {
            ID = TriggerID.ReachAttack;
            
            
        }

        public override bool Trigger(FSMBase fsm)
        {
            if (fsm.UnitInfo.attackC == null) return false;
            return fsm.UnitInfo.attackC.FinderEnemy();
        }
    }
}
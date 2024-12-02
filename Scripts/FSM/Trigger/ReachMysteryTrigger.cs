namespace FSM
{
    public class ReachMysteryTrigger: FSMTrigger
    {
        public override void Init()
        {
            ID = TriggerID.ReachMystery;
        }

        public override bool Trigger(FSMBase fsm)
        {
            return false;
        }
    }
}
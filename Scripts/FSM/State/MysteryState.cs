using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class MysteryState : FSMState
    {
        public override void Init()
        {
            base.Init();
            ID = StateID.Mystery;
        }
    }
}
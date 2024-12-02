using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 传送门
/// </summary>
public class WarpUnit : Unit
{
    public CommonWarpMoveComponent wmc;

    public override void Init()
    {
        base.Init();
        wmc = new CommonWarpMoveComponent(this);
        wmc.SetTriggerEnter2D((c) => {print(c); });
    }
}

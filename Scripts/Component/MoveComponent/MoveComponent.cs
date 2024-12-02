using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂在在mono上的移动组件  考虑到有联机 会分为NetMove和Move
/// </summary>
[Serializable]
public abstract class MoveComponent
{
    protected Unit unit;

    /// <summary>
    /// 移动加速度
    /// </summary>
    public Vector2 moveVec;

    /// <summary>
    /// 加速度的提升降低速度
    /// </summary>
    protected float vecSpeed =10.0f;

    public MoveComponent(Unit unit)
    {
        this.unit = unit;
    }

    public abstract void Move(Vector2 dir);
}
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using UnityEngine;
[Serializable]
public class HostPlayerMoveC : MoveComponent
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dir"></param>
    public override void Move(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            unit.character.SetState(CharacterState.Idle);
        }
        else
        {
            var characterTowards = dir.x > 0 ? 0 : 180;
            unit.character.SetState(CharacterState.Run);
            unit.entity.transform.rotation = Quaternion.Euler(0,characterTowards,0);
        }

        dir.x = Mathf.Clamp(dir.x, -1.0f, 1.0f);
        dir.y = Mathf.Clamp(dir.y, -1.0f, 1.0f);
        
        moveVec = Vector2.Lerp(moveVec, dir, vecSpeed*Time.deltaTime);
        unit.pyhsic.AddForce(  moveVec * Time.deltaTime * 500 * unit.data.Speed);
    }
    public HostPlayerMoveC(Unit unit) : base(unit)
    {
        moveVec = Vector2.zero;
    }
}

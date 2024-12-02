using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveComponent : MoveComponent
{
    private Quaternion rightQua;
    private Quaternion LeftQua;
    public AIMoveComponent(Unit unit) : base(unit)
    {
        LeftQua = Quaternion.Euler(0, 0, 0);
        rightQua = Quaternion.Euler(0, 180, 0);
    }

    public override void Move(Vector2 dir)
    {
        Vector3 vec = Vector3.zero;
        Quaternion quaternion = Quaternion.identity;
        var speed = unit.data.Speed;
        if (unit.exData.ct == CampType.Right)
        {
            vec = new Vector3(-speed, 0, 0);
            quaternion =rightQua;

        }
        else
        {
            vec = new Vector3(speed, 0, 0);
            quaternion =LeftQua;
        }

        unit.entity.transform.position += vec * Time.deltaTime;
        unit.entity.transform.rotation = quaternion;
    }
}
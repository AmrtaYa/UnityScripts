using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// unit组件  管理物理系统
/// </summary>
public class PhysicController
{
   private Unit unit;
   private Rigidbody2D body;
   public PhysicController(Unit unit)
   {
      this.unit = unit;
      body = unit.GetComponentInChildren<Rigidbody2D>();
      
   }

   public void Update()
   {
     
      
   }
   

   public void AddForce(Vector2 vec2)
   {
      body.linearVelocity = Vector2.zero;
      body.AddForce(vec2);
      
   }

}

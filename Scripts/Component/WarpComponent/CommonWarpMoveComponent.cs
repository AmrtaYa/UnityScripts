using System;
using System.Collections;
using System.Collections.Generic;
using GJC.Helper;
using Unity.VisualScripting;
using UnityEngine;

public class WarpMove : MonoBehaviour
{
   public Action<Collider2D> OnTriggerEnter;
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (OnTriggerEnter != null) OnTriggerEnter(other);
   }
}

public class CommonWarpMoveComponent
{
   
   public WarpUnit unit;
   private Collider2D col2D;
   private WarpMove WM;
   public CommonWarpMoveComponent(WarpUnit unit)
   {
      this.unit = unit;
      col2D = unit.transform.GetComponentInChildren<CapsuleCollider2D>();
      col2D.gameObject.AddComponent<WarpMove>();
   }

   public void SetTriggerEnter2D(Action<Collider2D> action)
   {
      WM.OnTriggerEnter = action;
   }

}

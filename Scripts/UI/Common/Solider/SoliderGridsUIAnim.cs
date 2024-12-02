using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SoliderGridsUIAnim 
{
  public static Vector3 oriVec = Vector3.one;
  public static Vector3 desVec = Vector3.one*1.2f;
  private static bool IfBigger = false;
  public static void SoliderGridsBigger(Transform tr)
  {
    if(IfBigger) return;
    DOTween.To(() => tr.localScale, value => tr.localScale = value,
      desVec, 0.3f);
    IfBigger = true;
  }
  public static void SoliderGridsSmall(Transform tr)
  {
    if(UnitManager.Instance.playerSpawn.ifPutUp) return;
    DOTween.To(() => tr.localScale, value => tr.localScale = value,
      oriVec, 0.3f);
    IfBigger = false;
  }
}

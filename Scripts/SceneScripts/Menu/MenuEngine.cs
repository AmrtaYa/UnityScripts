using System.Collections;
using System.Collections.Generic;
using GJC.Helper;
using UnityEngine;

public class MenuEngine : SingleTon<MenuEngine>
{
   public override void init()
   {
      base.init();
      MySceneManager.LoadScene(SceneType.LV01_01);
   }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEngine : MonoBehaviour
{
   private void Awake()
   {
      MySceneManager.LoadScene(SceneType.Menu);
   }
}

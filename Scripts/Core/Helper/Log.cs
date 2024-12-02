using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log 
{
    public static void EditorLog(string info)
    {
#if UNITY_EDITOR
        Debug.Log(info);
#endif
    }
}

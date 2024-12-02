using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Start=0,
    Menu=1,
    GameMain=2,
    
    LV01_01,
    // Test1,
    // Test2,
    // Test3,
    // Test4,
    // Test5,
    // Test6,
    // Test7,
    // Test8,
}
/// <summary>
/// 场景切换器
/// </summary>
public static class MySceneManager
{
    public static async void LoadScene(SceneType st)
    {
#if UNITY_EDITOR
        Debug.Log(st.ToString());
#endif
        await SceneManager.LoadSceneAsync(st.ToString());
        UIManager.Instance.ClearUI();
        UIManager3D .Instance.ClearUI();
    }
}


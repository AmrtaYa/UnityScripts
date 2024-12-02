using System;
using System.Collections;
using System.Collections.Generic;
using GJC.Helper;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

public class GameMainEngine : ResSingleTon<GameMainEngine>
{
    public DataBaseType saveID;
    public GameSettingEntity gameSetting;
    public List<KeyCode> AmriesKeyCodes;
    
    private HostPlayer oriPlayer;
    public HostPlayer player
    {
        set
        {
            if (oriPlayer == null) oriPlayer = value;
        }
        get
        {
            if (oriPlayer == null)
            {
                oriPlayer = GameObject.FindObjectOfType<HostPlayer>();
                oriPlayer.Init();
            }
            return oriPlayer;
        }
    }

    public PlayNetMode NetMode = PlayNetMode.SinglePlayer;

    protected override void init()
    {
        base.init();
        AmriesKeyCodes = new List<KeyCode>(11);
        DontDestroyOnLoad(gameObject);
        DBManager.Init();
        UpdateGameSetting();
    }

    private void OnApplicationQuit()
    {
        DBManager.Dispose();
    }

    /// <summary>
    /// 更新游戏设置数据
    /// </summary>
    public void UpdateGameSetting()
    {
        gameSetting = DBManager.GetEntity<GameSettingEntity>(0, DataBaseType.GameSetting);
        AmriesKeyCodes.Clear();
        AmriesKeyCodes.Add(gameSetting.Army00);
        AmriesKeyCodes.Add(gameSetting.Army01);
        AmriesKeyCodes.Add(gameSetting.Army02);
        AmriesKeyCodes.Add(gameSetting.Army03);
        AmriesKeyCodes.Add(gameSetting.Army04);
        AmriesKeyCodes.Add(gameSetting.Army05);
        AmriesKeyCodes.Add(gameSetting.Army06);
        AmriesKeyCodes.Add(gameSetting.Army07);
        AmriesKeyCodes.Add(gameSetting.Army08);
        AmriesKeyCodes.Add(gameSetting.Army09);
        //player?.sManager.UpdateGrids();
    }
}

#if !UNITY_EDITOR
[Preserve]
public class SkipUnityLogo
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void BeforeSplashScreen()
    {
#if UNITY_WEBGL
        Application.focusChanged += Application_focusChanged;
#else
        System.Threading.Tasks.Task.Run(AsyncSkip);
#endif
    }

#if UNITY_WEBGL
    private static void Application_focusChanged(bool obj)
    {
        Application.focusChanged -= Application_focusChanged;
        SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
    }
#else
    private static void AsyncSkip()
    {
        SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
    }
#endif
}
#endif
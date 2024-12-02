using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 视角控制器 HostPlayer专用！
/// </summary>
public class HorController
{
    /// <summary>
    /// 水平限制  到了这个限制视角会移动
    /// </summary>
    private Vector2 horMoveLimit;

    private HostPlayer Player;
    private bool LockHor = false;
    private float horRatio = 1.0f;

    /// <summary>
    /// 限制左右最大移动位
    /// </summary>
    public float deltaBroder = 5.0f;

    public bool IfLockHor
    {
        get { return LockHor; }
        set
        {
            LockHor = value;
            //防止画面颤抖
            if (value) horRatio = 100.0f;
            else
                horRatio = 1.0f;
        }
    }

    public HorController(HostPlayer hostPlayer)
    {
        CalcScreenMoveLimit();
        Player = hostPlayer;
    }

    /// <summary>
    /// 计算屏幕移动限制  修改屏幕分辨率后记得重新计算
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void CalcScreenMoveLimit()
    {
        var width = (float)Screen.width * 0.01f;
        horMoveLimit = new Vector2(width, Screen.width - width);
    }

    public void BackToPlayerEntity()
    {
        var ePos = Player.playerEntity.transform.position;
        var cameraPos = Player.playerCamera.transform.position;
        float xLerp = Mathf.Lerp(cameraPos.x, ePos.x,
            GameMainEngine.Instance.gameSetting.horSensitive * Time.deltaTime * horRatio);
        cameraPos.x = xLerp;
        Player.playerCamera.transform.position = cameraPos;
    }

    /// <summary>
    /// 屏幕视角移动
    /// </summary>
    public void ScreenMoveChecked()
    {
        var mousePos =MapManager.Instance.MousePosition();
        float screenMoveValue = 0;
        //检测鼠标到哪里，屏幕才会向左向右移动
        if (mousePos.x < horMoveLimit.x) screenMoveValue = -1.0f;
        if (mousePos.x > horMoveLimit.y) screenMoveValue = 1.0f;
        var playerSpawn = UnitManager.Instance.playerSpawn;
        var enemySpawn = UnitManager.Instance.enemySpawn;
        //左加 右减 边界
        float leftBroder = 0;
        float rightBroder = 0;
        if (playerSpawn.exData.ct == CampType.Left)
        {
            leftBroder = playerSpawn.transform.position.x + deltaBroder;
            rightBroder = enemySpawn.transform.position.x - deltaBroder;
        }
        else
        {
            leftBroder = enemySpawn.transform.position.x + deltaBroder;
            rightBroder = playerSpawn.transform.position.x - deltaBroder;
        }

        if (Player.playerCamera.transform.position.x < leftBroder&& screenMoveValue<0) return;
        if (Player.playerCamera.transform.position.x > rightBroder&& screenMoveValue>0) return;
        
        
        Player.playerCamera.transform.position += (new Vector3(1.0f, 0, 0) * screenMoveValue * Time.deltaTime *
                                                   GameMainEngine.Instance.gameSetting.horSensitive * 0.3f);
    }
}
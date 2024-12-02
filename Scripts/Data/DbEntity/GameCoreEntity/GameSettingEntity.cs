using System;
using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;
using Menu;

/// <summary>
/// 游戏设置数据库实体类
/// </summary>
public struct GameSettingEntity : IDataBase
{
    [PrimaryKey] public int ID { get; set; }

    /// <summary>
    /// 是否全屏
    /// </summary>
    public MenuWindows windows { get; set; }

    /// <summary>
    /// 帧率限制
    /// </summary>
    public float frameLimit { get; set; }

    /// <summary>
    /// 视距
    /// </summary>
    public float horizon { get; set; }


    /// <summary>
    /// 地图缩放
    /// </summary>
    public float mapRatio { get; set; }

    /// <summary>
    /// 阴影
    /// </summary>
    public MenuSwtich shadow { get; set; }

    /// <summary>
    /// 细节
    /// </summary>
    public MenuSwtich detail { get; set; }

    /// <summary>
    /// 特效
    /// </summary>
    public MenuSwtich effect { get; set; }

    /// <summary>
    /// 抗锯齿
    /// </summary>
    /// <returns></returns>
    public MenuSwtich aliasing { get; set; }


    /// <summary>
    /// 后处理
    /// </summary>
    /// <returns></returns>
    public MenuSwtich postProcessing { get; set; }

    /// <summary>
    /// 鼠标灵敏度
    /// </summary>
    public float mouseSensitive { get; set; }

    /// <summary>
    /// 视距灵敏度
    /// </summary>
    public float horSensitive { get; set; }

    /// <summary>
    /// 视距反转
    /// </summary>

    public MenuToggle horReverse { get; set; }

    /// <summary>
    /// 主音量
    /// </summary>
    public float mainVolume { get; set; }

    /// <summary>
    /// 背景音量
    /// </summary>
    public float bgVolume { get; set; }

    /// <summary>
    /// 特效音量
    /// </summary>
    public float effectVolume { get; set; }

    /// <summary>
    /// 角色音量
    /// </summary>
    public float charterVolume { get; set; }

    /// <summary>
    ///语言类型
    /// </summary>

    public LanuageType lanuage { get; set; }

    /// <summary>
    /// 伤害显示
    /// </summary>
    public MenuToggle damageShow { get; set; }

    /// <summary>
    /// 兵种的血条显示
    /// </summary>
    public MenuToggle hpUIShow { get; set; }

    /// <summary>
    /// 向前移动
    /// </summary>
    public KeyCode mForward{ get; set; }

    /// <summary>
    /// 向后移动
    /// </summary>
    public KeyCode mBack{ get; set; }

    /// <summary>
    /// 向左移动
    /// </summary>
    public KeyCode mLeft{ get; set; }

    /// <summary>
    /// 向右移动
    /// </summary>
    public KeyCode mRight{ get; set; }

    /// <summary>
    /// 打开商店
    /// </summary>
    public KeyCode openShop{ get; set; }

    /// <summary>
    /// 打开地图
    /// </summary>
    public KeyCode openMap{ get; set; }

    /// <summary>
    /// 技能1
    /// </summary>
    public KeyCode Skill01{ get; set; }

    /// <summary>
    /// 技能2
    /// </summary>
    public KeyCode Skill02{ get; set; }

    /// <summary>
    /// 技能3
    /// </summary>
    public KeyCode Skill03{ get; set; }

    /// <summary>
    /// 技能4
    /// </summary>
    public KeyCode Skill04{ get; set; }

    /// <summary>
    /// 携带兵种1
    /// </summary>
    public KeyCode Army00{ get; set; }

    /// <summary>
    /// 携带兵种2
    /// </summary>
    public KeyCode Army01{ get; set; }

    /// <summary>
    /// 携带兵种3
    /// </summary>
    public KeyCode Army02{ get; set; }

    /// <summary>
    /// 携带兵种4
    /// </summary>
    public KeyCode Army03{ get; set; }

    /// <summary>
    /// 携带兵种5
    /// </summary>
    public KeyCode Army04{ get; set; }

    /// <summary>
    /// 携带兵种6
    /// </summary>
    public KeyCode Army05{ get; set; }

    /// <summary>
    /// 携带兵种7
    /// </summary>
    public KeyCode Army06{ get; set; }

    /// <summary>
    /// 携带兵种8
    /// </summary>
    public KeyCode Army07{ get; set; }

    /// <summary>
    /// 携带兵种9
    /// </summary>
    public KeyCode Army08{ get; set; }

    /// <summary>
    /// 携带兵种10
    /// </summary>
    public KeyCode Army09{ get; set; }

    /// <summary>
    /// 切换前武器
    /// </summary>
    public KeyCode sWeaponLeft{ get; set; }

    /// <summary>
    /// 切换后武器
    /// </summary>
    public KeyCode sWeaponRgiht{ get; set; }

    /// <summary>
    /// 枪械换弹(如果不是枪械武器，这个按键就没用了，是摆设)
    /// </summary>
    public KeyCode reload{ get; set; }

    /// <summary>
    /// 视角往左边移动
    /// </summary>
    public KeyCode mapHorMoveLeft{ get; set; }
    /// <summary>
    /// 视角往右边移动
    /// </summary>
    public KeyCode mapHorMoveRight{ get; set; }
    
    /// <summary>
    /// 视角回到主角
    /// </summary>
    public KeyCode backCharacterHor{ get; set; }
    
    /// <summary>
    /// 锁定视角
    /// </summary>
    public KeyCode lockHor{ get; set; }
    public int GetPrimKey()
    {
        return ID;
    }

    public Type[] dependEntity()
    {
        return null;
    }

    public IDataBase GetDefaultEntity(int id)
    {
        return new GameSettingEntity()
        {
            windows = MenuWindows.FullScreen,
            frameLimit = 120,
            horizon = 50,
            mapRatio = 50,
            shadow = MenuSwtich.Mid,
            detail = MenuSwtich.Mid,
            effect = MenuSwtich.Mid,
            aliasing = MenuSwtich.Mid,
            postProcessing = MenuSwtich.Mid,
            mouseSensitive = 50,
            horSensitive = 50,
            horReverse = MenuToggle.No,
            mainVolume = 100,
            bgVolume = 100,
            effectVolume = 100,
            charterVolume = 100,
            lanuage = LanuageType.Chinese,
            damageShow = MenuToggle.Yes,
            hpUIShow = MenuToggle.Yes,
            mForward = KeyCode.W,
            mBack = KeyCode.S,
            mLeft = KeyCode.A,
            mRight = KeyCode.D,
            openShop = KeyCode.P,
            openMap = KeyCode.M,
            Skill01 = KeyCode.Z,
            Skill02 = KeyCode.X,
            Skill03 = KeyCode.C,
            Skill04 = KeyCode.V,
            Army00 = KeyCode.Alpha1,
            Army01 = KeyCode.Alpha2,
            Army02 = KeyCode.Alpha3,
            Army03 = KeyCode.Alpha4,
            Army04 = KeyCode.Alpha5,
            Army05 = KeyCode.Alpha6,
            Army06 = KeyCode.Alpha7,
            Army07 = KeyCode.Alpha8,
            Army08 = KeyCode.Alpha9,
            Army09 = KeyCode.Alpha0,
            sWeaponLeft = KeyCode.Q,
            sWeaponRgiht = KeyCode.E,
            reload = KeyCode.R,
            mapHorMoveLeft = KeyCode.LeftArrow,
            mapHorMoveRight = KeyCode.RightArrow,
            backCharacterHor = KeyCode.Space,
            lockHor = KeyCode.Y,
        };
    }
}
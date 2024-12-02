using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MenuEnum Menu场景的所有Enum
namespace Menu
{
    public enum MenuSwtich
    {
        Low = 10001, //低画质
        Mid = 10002, //高画质
        High = 10003, //高画质
    }

    public enum MenuWindows
    {
        /// <summary>
        /// 全屏
        /// </summary>
        FullScreen = 10100,

        /// <summary>
        /// 窗口
        /// </summary>
        WindowsScreen = 10101,
    }

    public enum MenuToggle
    {
        Yes = 20001, //是
        No = 20002, //否
    }

    public enum MenuFrame
    {
        Fps30 = 30030,
        Fps60 = 30060,
        Fps90 = 30090,
        Fps120 = 30120,
        Unlimited = 30000 
    }
    //其余按键类型的，转换为对应的KeyCode数字 例如  KeyCode.Alpha0 = 48  ，键盘的0是48
}

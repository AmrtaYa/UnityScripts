using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAnimManager
{
    public static Dictionary<Type, List<string>> wholeAnimDir = new Dictionary<Type, List<string>>();
    private Unit unit;
    private AnimationEvents animEvents;

    public UnitAnimManager(Unit unit)
    {
        this.unit = unit;
    }

    public void Init()
    {
        InitAnimEvent();
    }

    private void InitAnimEvent()
    {
        if (unit.character == null) return;
        animEvents = unit.character.GetComponentInChildren<AnimationEvents>();
        // animEvent.Init(unit);
        unit.attackC.AddEvent();
    }

    public void AddEndAction(string animName, Action action)
    {
        var clip = unit.character.Animator.runtimeAnimatorController.animationClips.Where
            ((c) => { return c.name == animName; }).ToArray().FirstOrDefault();
        //时间 + 动画名字，  达成不同的key
        BaseAddAction(clip, animName, action, clip.length);
    }

    public void AddStartAction(string animName, Action action)
    {
        var clip = unit.character.Animator.runtimeAnimatorController.animationClips.Where
            ((c) => { return c.name == animName; }).ToArray().FirstOrDefault();
        //时间 + 动画名字，  达成不同的key
        BaseAddAction(clip, animName, action, 0);
    }

    public void AddAction(string animName, Action action, float Time)
    {
        var clip = unit.character.Animator.runtimeAnimatorController.animationClips.Where
            ((c) => { return c.name == animName; }).ToArray().FirstOrDefault();
        //时间 + 动画名字，  达成不同的key
        BaseAddAction(clip, animName, action, Time);
    }

    private void BaseAddAction(AnimationClip clip, string animName, Action action, float time)
    {
        if (clip == null) return;
        string resultAnimName = animName + time.ToString();
        if (!animEvents.animEventDic.ContainsKey(resultAnimName))
        {
            animEvents.animEventDic.Add(resultAnimName, action);
        }

        //-------------------------这个是为了避免同一个动画上添加重复的事件而设定的--------------------------------
        var unitType = unit.GetType();
        if (!wholeAnimDir.ContainsKey(unitType))
        {
            wholeAnimDir.Add(unitType, new List<string>());
        }
        var animList = wholeAnimDir[unitType];
        if (!animList.Contains(resultAnimName))
            animList.Add(resultAnimName);
        else
            return;
        //-------------------------这个是为了避免同一个动画上添加重复的事件而设定的--------------------------------

        AnimationEvent e = new AnimationEvent();
        e.time = time;
        e.functionName = "AnimEventDealWith";
        e.stringParameter = resultAnimName;
        bool b = true;
        // foreach (var singleEvent in clip.events)
        // {
        //     //如果有这个事件，那就不加了
        //     if (singleEvent.functionName == e.functionName)
        //     {
        //         b = false;
        //         break;
        //     }
        // }

        if (b)
            clip.AddEvent(e);
    }

    public void RemoveAllAction(string animName)
    {
        var clip = unit.character.Animator.runtimeAnimatorController.animationClips.Where
            ((c) => { return c.name == animName; }).ToArray().First();
        if (clip != null)
            Array.Clear(clip.events, 0, clip.events.Length);
        wholeAnimDir[unit.GetType()].Clear();
        //这边还需要清空 带有animNameKey字典里的东西
    }
}
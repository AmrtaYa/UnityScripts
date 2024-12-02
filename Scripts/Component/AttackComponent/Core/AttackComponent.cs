using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using GJC.Helper;
using UnityEngine;

/// <summary>
/// 攻击组件
/// </summary>
public class AttackComponent
{
    public Unit unit;
    private GameObject SlashAtkPrefab;
    private Queue<Unit> enemyUnitQue;
    public SkillManager skillManager;
    public AttackComponent(Unit unit)
    {
        this.unit = unit;
        enemyUnitQue = new Queue<Unit>(10);
        skillManager = new SkillManager(unit);
        //this.unit.animMgr.RemoveAllAction();
    }

    public virtual void Init()
    {
        SlashAtkPrefab = LoadWay.ResLoad<GameObject>("Prefab/AtkComponent/" + unit.GetType() + "Slash");
    }


    //动画调用逻辑
    public  void AnimSlashAtk(Unit unit)
    {
        Vector2 vec2 = unit.data.forwardOffsetRange* unit.entity.transform.right;
        var slashC = GameObjectPool.Instance.Get
        (unit.gameObject.name + "SlashAtk",
            SlashAtkPrefab, unit.character.transform.position+new Vector3(vec2.x,vec2.y,0),
            Quaternion.identity).GetComponent<Slash>();
        slashC.owner = unit;
    }

    public virtual void AddEvent()
    {
        string str = unit.GetType().ToString();
        string slashMelee1h = "SlashMelee1H" + str;
        unit.animMgr.AddStartAction(slashMelee1h,
            () => { unit.IfHegemony = true; });
        unit.animMgr.AddAction(slashMelee1h,
            () =>
            {
               AnimSlashAtk(unit);
            },
            0.2F);
        unit.animMgr.AddEndAction(slashMelee1h,
            () => { unit.IfHegemony = true; });
        
        //unit.animMgr.add
    }

    //状态机调用
    public void CommonAttack()
    {
        if (UnitManager.Instance.playerSpawn.ifPutUp) return;
        var camera = GameMainEngine.Instance.player.playerCamera;
        var screenPos = camera.WorldToScreenPoint(unit.entity.transform.position);
        var posDelta = Input.mousePosition.x - screenPos.x;
        float toward = posDelta >= 0 ? 0 : 180;
        unit.entity.transform.rotation = Quaternion.Euler(0, toward, 0);
        unit.character.Slash();
    }
    /// <summary>
    /// 在状态机里调用的寻敌     需要注重效率
    /// </summary>
    /// <returns></returns>
    public bool FinderEnemy()
    {
      
        return    ((AIUnit)unit).seekManager.IfFindEnemy;
    }

    public virtual void AICommonAttack()
    {
        unit.character.Slash();
    }
    
}





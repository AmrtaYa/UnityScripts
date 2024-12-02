using System;
using GJC.Helper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 技能()
/// </summary>
public class Skill : MonoBehaviour, IRsetable, IRelease
{
    public Unit owner;
    public SkillData data;
    [HideInInspector] public SpriteRenderer renderer;
    private IRangeSelect selectWay; //范围选区位置
    private bool IfFire = false; //一开始会有个指示圈范围，这个是用来决定是否按下
    [SerializeField] private int insideUnit = 0;

    public void OnRest()
    {
        if (data == null) return;
        if (renderer == null)
            renderer = GetComponent<SpriteRenderer>();
        if (selectWay == null)
        {
            //初始化选区
            selectWay = SkillFactory.Instance.GetAeroRange(data.select);
            selectWay.SelectInit(this);
        }
    }


    private void Update()
    {
        if (selectWay != null)
            selectWay.SelectUpdate(this);
       
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (insideUnit == 1)
        {
            renderer.color = Color.yellow;
        }
        else
        {
            renderer.color = Color.white;
        }
        var u = other.GetComponentInParent<Unit>();
        if (u == null) return;
        if (owner == null) return;
        if (u.exData.ct == owner.exData.ct) return;
        insideUnit=1;
    
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (insideUnit ==1)
        {
            renderer.color = Color.yellow;
        }
        else
        {
            renderer.color = Color.white;
        }
        var u = other.GetComponentInParent<Unit>();
        if (u == null) return;
        if (owner == null) return;
        if (u.exData.ct == owner.exData.ct) return;
        insideUnit=0;
   
    }

    //如果取消了，那么需要返回技能资源
    public void ReturnSkillResource()
    {
        data.currentCD = 0;
    }

    public void ReleaseSkill()
    {
        var attackPrefab = SkillFactory.Instance.GetSkillAttackPrefab(data, 0);
        var skillAttack = GameObjectPool.Instance.Get(data.skillName + "Attack", attackPrefab,
            transform.position, transform.rotation);
        var attack = skillAttack.GetComponent<SkillAttack>();
        attack.owner = owner;
        attack.skill = this;
        attack.OnRest();
        GameObjectPool.Instance.Release(gameObject);
    }

    public void ResetData()
    {
        owner.attackC.skillManager.skillInstance = null;
        data = null;
        owner = null;
        insideUnit = 0;
    }

    public void OnRelease()
    {
        ResetData();
    }
}

public interface ISkillLogic
{
    void LogicUpdate();
    void LogicStart();
}


//技能选区范围落定点
public interface IRangeSelect
{
    void SelectInit(Skill skill);
    void SelectUpdate(Skill skill);
}

public enum AreaType
{
    Circle = 1001,
}

public enum SkillSelect
{
    HostPlayerlMouseSelect = 1001,
}
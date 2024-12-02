using Cysharp.Threading.Tasks;
using GJC.Helper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using UnityEngine;
using If = Unity.VisualScripting.If;

public class SkillManager
{
    public Unit owner;
    private SkillData[] datas;
    private bool ResetCD;
    public Skill skillInstance;

    public SkillManager(Unit unit)
    {
        owner = unit;
        datas = SkillFactory.Instance.LoadSkillByConfig(unit);
        //datas = new SkillData[4] { new SkillData(){CD = 0},new SkillData(),new SkillData(),new SkillData()};


        AddAnimEvent(unit);
    }

    private async UniTask AddAnimEvent(Unit unit)
    {
        string str = unit.GetType().ToString();
        string skill01 = "JabMelee1H" + str;
        await UniTask.WaitUntil(() => { return unit.animMgr != null; });

        unit.animMgr.AddStartAction(skill01, () =>
        {
            ReleaseSkill();
        });
    }

    private void ReleaseSkill()
    {
        skillInstance.ReleaseSkill();
    }

    public bool ReleaseSkillAnim()
    {
        if (skillInstance == null) return false;
        owner.character.Jab();
        return true;
    }

    /// <summary>
    /// 释放技能(主机玩家专用)
    /// </summary>
    /// <param name="i">技能几号位</param>
    public void ReadySkillHostPlayer(int i)
    {
        SkillData data = datas[i - 1];
        if (skillInstance != null)
        {
            skillInstance.ReturnSkillResource();
            GameObjectPool.Instance.Release(skillInstance.gameObject);
            skillInstance = null;
            return;
        }

        if (data.currentCD > 0)
        {
            Debug.Log(data.skillName + "目前在CD当中:" + data.currentCD);
            return;
        }


        //技能逻辑，产生预制件，由预制件来负责
        GameObject prefab = SkillFactory.Instance.GetSkillSelectPrefab(data);
        var skillInstanceGO = GameObjectPool.Instance.Get(data.skillName + "Area", prefab, default, default);
        skillInstance = skillInstanceGO.GetComponent<Skill>();
        skillInstance.owner = owner;
        skillInstance.data = data;
        skillInstance.OnRest();
        data.currentCD = data.CD;

        CoolDown(data).Forget();
        Debug.Log(i - 1 + "  " + data.CD);
    }

    private async UniTask CoolDown(SkillData data)
    {
        for (int i = 0; i < 5000; i++)
        {
            await UniTask.WaitForSeconds(1);
            data.currentCD--;
            if (ResetCD) break;
        }

        data.currentCD = 0;
        ResetCD = false;
    }

    public void ShutDownAllCDTask()
    {
        ResetCD = true;
    }

    public void TestSkill()
    {
        SkillData[] datas = SkillFactory.Instance.LoadSkillByConfig(owner);
        datas[0].CD = 30;
        Debug.Log("放了测试技能");
    }
}

public class SkillData
{
    public int ID; //技能ID
    public float releaseTime;
    public float DamageRatio; //伤害倍率
    public float CD;
    public float currentCD;
    public string description;
    public string skillName;
    public SkillSelect select; //选的方法  是AI自动放，还是玩家自己操控，还是固定位置
    public AreaType area; //选区  是圆还是扇形等等
}
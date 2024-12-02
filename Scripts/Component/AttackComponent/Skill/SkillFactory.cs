using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GJC.Helper;
using OfficeOpenXml;
using UnityEngine;

public class SkillFactory : SingleTon<SkillFactory>
{
    private Dictionary<Unit, SkillData[]> skillConfigDir;
    private ExcelWorksheets skillSheets;
    private Dictionary<SkillSelect, IRangeSelect> aeroRange;

    public override void init()
    {
        base.init();
        skillConfigDir = new Dictionary<Unit, SkillData[]>();
        aeroRange = new Dictionary<SkillSelect, IRangeSelect>();
        string path = Application.streamingAssetsPath + "/SkillConfig/";
        ExcelPackage package;
        //FSM密码设置
        package = new ExcelPackage(new FileInfo(path + "SkillConfig.xlsx"));

        skillSheets = package.Workbook.Worksheets;
    }

    public SkillData[] LoadSkillByConfig(Unit unit)
    {
        var result = new SkillData[4];
        if (skillConfigDir.ContainsKey(unit))
        {
            result = (SkillData[])skillConfigDir[unit].Clone();
            return result;
        }


        string uType = unit.GetType().ToString();
        ExcelWorksheet skillSheet = skillSheets.FirstOrDefault(worksheet => worksheet.Name == uType);
        if (skillSheet == null)
        {
#if UNITY_EDITOR
            Debug.Log("未找到" + uType + "技能系统");
#endif
            return null;
        }


        SkillData[] datas = new SkillData[4];
        //写加载的算法
        object[,] objs = (object[,])skillSheet.Cells.Value;
        int rowNum = objs.GetLength(0);
        int colNum = objs.GetLength(1);
        List<object> objectsLIST = new List<object>();
        int skillIndex = 0;
        for (int r = 1; r <= rowNum; r++)
        {
            //过滤
            if (skillSheet.Cells[r, 1].Value == null) continue;
            if (skillSheet.Cells[r, 1].Value.ToString().Contains("#")) continue; //有#说明是注释
            objectsLIST.Clear();
            for (int c = 1; c <= colNum; c++)
            {
                objectsLIST.Add(objs[r - 1, c - 1]);
            }

            var entityResult = ConfigToSoliderDataEntity(objectsLIST);
            datas[skillIndex] = entityResult;
            skillIndex++;
        }


        skillConfigDir.Add(unit, datas);
        result = (SkillData[])skillConfigDir[unit].Clone();
        return result;
    }

    protected SkillData ConfigToSoliderDataEntity(List<object> arg)
    {
        if (arg[0] == null) return null;
        SkillData e = new SkillData();
        e.ID = Convert.ToInt32(arg[0]);
        e.CD = (float)Convert.ToDouble(arg[1]);
        e.DamageRatio = (float)Convert.ToDouble(arg[2]);
        e.description = arg[3].ToString();
        e.skillName = arg[4].ToString();
        e.select = Enum.Parse<SkillSelect>(arg[5].ToString());
        e.area = Enum.Parse<AreaType>(arg[6].ToString());
        return e;
    }

    public GameObject GetSkillSelectPrefab(SkillData data)
    {
        return LoadWay.ResLoad<GameObject>("Skill/Prefab/Area/" + data.skillName+"Area");
    }
    public GameObject GetSkillAttackPrefab(SkillData data,int num=0)
    {
        return LoadWay.ResLoad<GameObject>("Skill/Prefab/Attack/" + data.skillName+"Attack"+num);
    }
    public IRangeSelect GetAeroRange(SkillSelect dataSelect)
    {
        if (aeroRange.ContainsKey(dataSelect)) 
            return aeroRange[dataSelect];
        string typeName = dataSelect.ToString();
        Type t = Type.GetType(typeName);
        var rangeSelect = Activator.CreateInstance(t) as IRangeSelect;
        aeroRange.Add(dataSelect,rangeSelect);
        return aeroRange[dataSelect];
    }
}
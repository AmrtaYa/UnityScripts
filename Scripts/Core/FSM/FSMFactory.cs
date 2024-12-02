using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Excel;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using UnityEngine;


#region 配置表说明书

//配置表内，带#说明是注释
//[] 里面写状态
//在每个[]下面，左边是()  右边是{}
//()代表触发条件  {}代表触发条件后跳转的状态
//anystate状态内，会把下面的每个触发条件加入到上面每一个状态下

#endregion

namespace FSM
{
    public static class FSMFactory
    {
        private static string path;
        

        private static Dictionary<string, ExcelWorksheet> sheetsDir;
        private static Dictionary<string, FSMState[]> unitTypeStateBuffer; //给不同兵种准备的缓冲，如果有初始化过该兵种的状态机，直接使用

        static FSMFactory()
        {
            path = Application.streamingAssetsPath + "/FSMConfig/";
            ExcelPackage package;
            //FSM密码设置
            package = new ExcelPackage(new FileInfo(path + "FSMConfig.xlsx"));

            var sheets = package.Workbook.Worksheets;
            sheetsDir = new Dictionary<string, ExcelWorksheet>();
            unitTypeStateBuffer = new Dictionary<string, FSMState[]>();
            foreach (var VARIABLE in sheets)
            {
                sheetsDir.Add(VARIABLE.Name, VARIABLE);
            }
        }

        public static FSMState[] GetState(object Unit)
        {
            Type type = Unit.GetType();
#if UNITY_EDITOR
            //Debug.Log("状态机配置读取:" + type);
#endif
            //拿到对应的状态机表

            if (!unitTypeStateBuffer.ContainsKey(type.ToString()))
            {
                LoadStateBuffer(type.ToString());
            }

            return unitTypeStateBuffer[type.ToString()];
        }

        private static void LoadStateBuffer(string toString)
        {
            Dictionary<string, FSMState> stateContainer = new Dictionary<string, FSMState>();
            Dictionary<string, FSMTrigger> triggerContainer = new Dictionary<string, FSMTrigger>();
            FSMState currentState = null;
            var sheet = sheetsDir[toString];
            //计算出End的位置，找到表格的行列数
            object[,] objs = (object[,])sheet.Cells.Value;
            int rowNum = objs.GetLength(0);
            int colNum = objs.GetLength(1);
            for (int c = 1; c <= colNum; c++)
            {
                for (int r = 1; r <= rowNum; r++)
                {
                    //过滤
                    if (sheet.Cells[r, c].Value == null) continue;
                    string value = sheet.Cells[r, c].Value.ToString();
                    if (value.Contains("#")) continue;

                    //实例化
                    if (value.Contains("[") && value.Contains("]"))
                    {
                        //需要再FSM命名空间下       并且状态类就得添加状态
                        value = "FSM." + value.Replace("[", "").Replace("]", "") + "State";
                        if (!stateContainer.ContainsKey(value))
                        {
                            Type t = Type.GetType(value);
#if UNITY_EDITOR
                            if(t==null)Debug.LogError(value+"is null");            
#endif
                            currentState = Activator.CreateInstance(t) as FSMState;
                            stateContainer.Add(value, currentState);
                        }

                        currentState = stateContainer[value];
                    }
                    else if (value.Contains("(") && value.Contains(")"))
                    {
                        string triggerValue = "FSM." + value.Replace("(", "").Replace(")", "") + "Trigger";
                        value = sheet.Cells[r, c + 1].Value.ToString();
                        string trStateValue = "FSM." + value.Replace("{", "").Replace("}", "") + "State";
                        if (!stateContainer.ContainsKey(trStateValue))
                        {
                            Type t = Type.GetType(trStateValue);
#if UNITY_EDITOR
                if(t==null)  Debug.LogError("未找到 "+ trStateValue);            
#endif
                            var myState = Activator.CreateInstance(t) as FSMState;
                            stateContainer.Add(trStateValue, myState);
                        }

                        if (!triggerContainer.ContainsKey(triggerValue))
                        {
                            Type t = Type.GetType(triggerValue);
#if UNITY_EDITOR
                            if(t==null)  Debug.LogError("未找到 "+ triggerValue);            
#endif
                            var triggerIndex = Activator.CreateInstance(t) as FSMTrigger;
                            triggerContainer.Add(triggerValue, triggerIndex);
                        }

                        FSMState state = stateContainer[trStateValue];
                        FSMTrigger trigger = triggerContainer[triggerValue];

                        currentState.AddTriggers(trigger, state);
                    }
                   
                }
            }

            var  anyState = stateContainer["FSM.AnyState"];
            stateContainer.Remove("FSM.AnyState");
            
            foreach (var singleTriggerToState in anyState.GetTriggers())
            {
                foreach (var singleState in stateContainer.Values)
                {
                    if(singleState==singleTriggerToState.Value)continue;
                    singleState.AddTriggers(singleTriggerToState.Key,singleTriggerToState.Value);
                }
            }

            foreach (var singleState in stateContainer.Values)
            {
                singleState.Init();//等全部添加后才调用初始化
            }

            unitTypeStateBuffer.Add(toString, stateContainer.Values.ToArray());
        }
    }
}
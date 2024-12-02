using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using UnityEngine;

public class ExcelConfigFactory<T>
{
    private string path;
    private string password;

    public ExcelConfigFactory(string myPath, string pass = "")
    {
        path = Application.streamingAssetsPath + "/" + myPath;
        password = pass;
    }

    /// <summary>
    /// 第一位必须是ID
    /// </summary>
    /// <param name="action"></param>
    /// <param name="startNum"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Dictionary<int, T> LoadData(Func<List<object>, T> action, int startNum)
    {
        Dictionary<int, T> dirs = new Dictionary<int, T>();
        ExcelPackage package = string.IsNullOrEmpty(password)
            ? new ExcelPackage(new FileInfo(path))
            : new ExcelPackage(new FileInfo(path), password);
        var es = package.Workbook.Worksheets.First();
        object[,] objs = (object[,])es.Cells.Value;
        int rowNum = objs.GetLength(0);
        int col = objs.GetLength(1);
        List<object> objectsLIST = new List<object>();
        for (int r = startNum; r < rowNum; r++)
        {
            objectsLIST.Clear();
            for (int c = 0; c < col; c++)
            {
                objectsLIST.Add(objs[r, c]);
            }

            T result = action(objectsLIST);
            int id = Convert.ToInt32(objectsLIST[0]);
            if (id != -9999 &&objectsLIST[0]!=null)
                dirs.Add(id, result);
        }
        return dirs;
    }

    public void LoadDataEach(Action<object> processAction, Vector2 startNum, bool IfNeedEquals)
    {
        
        
    }
}
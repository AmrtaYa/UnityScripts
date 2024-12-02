using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GJC.Helper;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class MapManager : SingleTon<MapManager>
{
    public List<Tilemap> tMap = new List<Tilemap>();
    private int maxCol = 150;
    private Vector3Int currentCell = new Vector3Int(-999, -999, -999);
    private Vector3Int lastCell = new Vector3Int(-999, -999, -999);
    private bool IfChangeRow = false;
    private Color oriColor = Color.white;
    private Color hightLight = Color.yellow;

    private Vector2 mapSize;
    private Vector2 miniMapSize;
    public miniMap miniMap;

    public override void init()
    {
        base.init();
        tMap = transform.GetComponentsInChildren<Tilemap>().ToList();
        tMap.RemoveAt(tMap.Count-1);
        miniMap = GameMainEngine.Instance.player.fightUI.miniMap;
        UpdateMapData();
    }

    /// <summary>
    /// 鼠标位置获取当前是哪个道路
    /// </summary>
    /// <param name="mouseInput"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public MapRoad MouseToInput(Vector2 mouseInput)
    {
        var mW = GameMainEngine.Instance.player.playerCamera.ScreenToWorldPoint(MousePosition());
        var cell = tMap[0].WorldToCell(mW);
        return (MapRoad)cell.y;
    }

    private void UpdateMapData()
    {
        var playerSpawn = UnitManager.Instance.playerSpawn;
        var enemySpawn = UnitManager.Instance.enemySpawn;
        float width = Mathf.Abs(playerSpawn.transform.position.x - enemySpawn.transform.position.x);
        float mapHeight = Enum.GetValues(typeof(MapRoad)).Length;
        mapSize = new Vector2(width, mapHeight);
        miniMapSize = miniMap.mapSize;
    }

    /// <summary>
    /// 鼠标选中颜色
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void ChoosedColor()
    {
        if (GameMainEngine.Instance.player == null) return;
        //需要一个东西卡主，不能让他一直运行下去
        if (!UnitManager.Instance.playerSpawn.ifPutUp)
        {
            if (lastCell.x != -999)
            {
                foreach (var VARIABLE in tMap)
                {
                    for (int i = -maxCol; i < maxCol; i++)
                    {
                        //取消挂起的话，把上一帧设置成原来的颜色，然后 x-999为空
                        SetMapGridColor(VARIABLE, lastCell + new Vector3Int(i, 0, 0), oriColor);
                    }
                }

                lastCell.x = -999;
                lastCell.y = -999;
            }

            return;
        }
    
        var mW = GameMainEngine.Instance.player.playerCamera.ScreenToWorldPoint(MousePosition());
        currentCell = tMap[0].WorldToCell(mW);
        currentCell.x = 0;
        if (currentCell.y != lastCell.y && !IfChangeRow) //前后不一致，并且没有换行，才能进行切换行数
        {
            IfChangeRow = true;
        }

        if (IfChangeRow)
        {
            foreach (var VARIABLE in tMap)
            {
                for (int i = -maxCol; i < maxCol; i++)
                {
                    SetMapGridColor(VARIABLE, lastCell + new Vector3Int(i, 0, 0), oriColor);
                    SetMapGridColor(VARIABLE, currentCell + new Vector3Int(i, 0, 0), hightLight);
                }
            }

            IfChangeRow = false;
        }

        lastCell = currentCell;
    }

    /// <summary>
    ///实体地图坐标转换小地图坐标
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2 MapPosToMiniMapPos(Vector2 pos)
    {
        Vector2 rate = pos / new Vector2(mapSize.x, mapSize.y);
        return rate * miniMapSize + new Vector2(Screen.width - miniMapSize.x, miniMapSize.y / 2.0f);
    }

    public Vector3 MousePosition()
    {
        return Input.mousePosition -
               new Vector3(0, UIInfos.PlayerUIHeigh, 0);
    }

    public Vector2 MiniMapPosTo3DMapPos(Vector2 pos)
    {
        var ori = (pos - new Vector2(Screen.width - miniMapSize.x, miniMapSize.y / 2.0f)) / miniMapSize;
        return ori * new Vector2(mapSize.x, mapSize.y);
    }

    private void SetMapGridColor(Tilemap map, Vector3Int vector3Int, Color c)
    {
        map.SetTileFlags(vector3Int, TileFlags.None);
        map.SetColor(vector3Int, c);
    }

    /// <summary>
    /// 点击小地图后，摄像机转移
    /// </summary>
    public void MiniMapMoveCamera(PointerEventData e, RectTransform whiteCameraFrameRect)
    {
        Vector3 movePos = MiniMapPosTo3DMapPos(e.position);

        var oriPos = GameMainEngine.Instance.player.playerCamera.transform.position;

        var spawn = UnitManager.Instance.playerSpawn.transform.position.x;
        var spawn2 = UnitManager.Instance.enemySpawn.transform.position.x;
        var broder = GameMainEngine.Instance.player.horCtrl.deltaBroder;
        if (spawn > spawn2)
            movePos.x = Mathf.Clamp(movePos.x,spawn2+broder, spawn-broder);
        else
            movePos.x = Mathf.Clamp(movePos.x,spawn+broder, spawn2-broder);
        oriPos.x = movePos.x;
        GameMainEngine.Instance.player.playerCamera.transform.position = oriPos;
    }
}
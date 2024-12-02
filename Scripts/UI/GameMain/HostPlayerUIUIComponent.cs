using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using GJC.Helper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMP_Text = TMPro.TMP_Text;

/// <summary>
//主角战斗场景时UI
/// </summary>
public class HostPlayerUIUIComponent : UIComponent, IUIClear
{
    public miniMap miniMap;
    public PlayerInfoShow shows;

    public override void Init()
    {
        base.Init();
        miniMap = new miniMap(this);
        shows = new PlayerInfoShow(this);
    }

    protected override void _Update()
    {
        base._Update();


        miniMap.UpdateWhiteCamraFrame();
    }

    public void Clear()
    {
        shows.updateUICtk.Cancel();
        GameMainEngine.Instance.player.monenyMgr.fMoney.Clear();
        GameMainEngine.Instance.player.monenyMgr.moneyIncreaseStop.Cancel();
    }
}

/// <summary>
/// 玩家UI信息显示
/// </summary>
public class PlayerInfoShow
{
    private HostPlayerUIUIComponent UI;

    private float durHpUpdateTime = 0.02f;

    //Spawn
    private Image leftSpawnHpImg;
    private Image leftSpawnHpImgBG;
    private TMP_Text leftSpawnHpTxt;
    private Image rightSpawnHpImg;
    private Image rightSpawnHpImgBG;
    private TMP_Text rightSpawnHpTxt;
    private float leftTextHpCurrent;
    private float rightTextHpCurrent;

    //玩家奥义和血量条以及经验条
    private Image playerHpImg;
    private TMP_Text playerHpTxt;
    private Image playerMysteryImg;
    private TMP_Text playerMysteryTxt;
    private Image playerExpImg;
    private TMP_Text playerExpText;
    private TMP_Text playerLevelText;
    private float playerTextHpCurrent;
    private float playerTextMysteryCurrent;
    private float playerTextExpCurrent;

    //money显示
    public TMP_Text sMoneyTxt; //士兵钱
    public TMP_Text eMonenTxt; //装备钱

    //solider UI Shows
    public Dictionary<Button, SoliderDataEntity> soliderGrids;
    public Dictionary<KeyCode, Button> solider_kcToBtn;
    private Dictionary<Button, TMP_Text> soliderGridsKeyCodeTxt;

    private Button shopBtn;

    public CancellationTokenSource updateUICtk;

    public PlayerInfoShow(HostPlayerUIUIComponent ui)
    {
        UI = ui;
        updateUICtk = new CancellationTokenSource();
        InitUIComponent(ui);
        shopBtn.onClick.AddListener(() => { updateUICtk.Cancel(); });

        //游戏开始时候更新一次血条
        UpdateSpawnHp();
        //游戏开始时更新一次玩家血条
        UpdatePlayerInfo();
    }


    private void InitUIComponent(HostPlayerUIUIComponent ui)
    {
        var infoTr = ui.transform.FindTheTfByName("DataInfo");
        var spawnInfoTr = ui.transform.FindTheTfByName("SpawnInfo");

        shopBtn = infoTr.FindTheTfByName<Button>("Shop");

        // playerSpawnHpImg
        leftSpawnHpImg = spawnInfoTr.FindTheTfByName("LeftSpawnHpUI").FindTheTfByName<Image>("ReallyHp");
        rightSpawnHpImg = spawnInfoTr.FindTheTfByName("RightSpawnHpUI").FindTheTfByName<Image>("ReallyHp");
        leftSpawnHpTxt = spawnInfoTr.FindTheTfByName("LeftSpawnHpUI").FindTheTfByName<TMP_Text>("HpText");
        rightSpawnHpTxt = spawnInfoTr.FindTheTfByName("RightSpawnHpUI").FindTheTfByName<TMP_Text>("HpText");
        leftSpawnHpImgBG =
            spawnInfoTr.FindTheTfByName("LeftSpawnHpUI").FindTheTfByName<Image>("BG"); //以后可以做血低于xx的红色闪烁效果
        rightSpawnHpImgBG = spawnInfoTr.FindTheTfByName("RightSpawnHpUI").FindTheTfByName<Image>("BG");

        //PLAYER UI
        var playerHpTr = infoTr.FindTheTfByName("PlayerHp");
        playerHpImg = playerHpTr.FindTheTfByName<Image>("Current");
        playerHpTxt = playerHpTr.FindTheTfByName<TMP_Text>("HpNumShow");

        var playerMysTr = infoTr.FindTheTfByName("PlayerMystery");
        playerMysteryImg = playerMysTr.FindTheTfByName<Image>("Current");
        playerMysteryTxt = playerMysTr.FindTheTfByName<TMP_Text>("MysNumShow");

        var playerExpTr = infoTr.FindTheTfByName("PlayerExp");
        playerExpImg = playerExpTr.FindTheTfByName<Image>("Current");
        playerExpText = playerExpTr.FindTheTfByName<TMP_Text>("ExpNumShow");
        playerLevelText = playerExpTr.FindTheTfByName<TMP_Text>("ExpText");

        //Money
        var moneyTr = ui.transform.FindTheTfByName("Money");
        sMoneyTxt = moneyTr.FindTheTfByName("SoliderMoney").FindTheTfByName<TMP_Text>("SMoneyText");
        eMonenTxt = moneyTr.FindTheTfByName("EquipMoney").FindTheTfByName<TMP_Text>("EMoneyText");


        //Soliders
        soliderGrids = new Dictionary<Button, SoliderDataEntity>(GameMainEngine.Instance.player.sManager.maxSoliderNum);
        solider_kcToBtn = new Dictionary<KeyCode, Button>(GameMainEngine.Instance.player.sManager.maxSoliderNum);
        soliderGridsKeyCodeTxt =
            new Dictionary<Button, TMP_Text>(GameMainEngine.Instance.player.sManager.maxSoliderNum);
        List<Button> gridsBtns = new List<Button>(GameMainEngine.Instance.player.sManager.maxSoliderNum);
        ui.transform.FindTheTfByName("SoliderGrids").FindAllComponent(ref gridsBtns);
        foreach (var g in gridsBtns)
        {
            soliderGrids.Add(g, SoliderDataEntity.GetEmptyEntity());
            var kcText = g.transform.parent.GetComponentInChildren<TMP_Text>();
            var btnImagePointerEvent = g.transform.parent.AddComponent<BtnImagePointerEvent>();
            btnImagePointerEvent.pointerEnter = data => { SoliderGridsUIAnim.SoliderGridsBigger(g.transform.parent); };
            btnImagePointerEvent.pointerExit = data => { SoliderGridsUIAnim.SoliderGridsSmall(g.transform.parent); };
            soliderGridsKeyCodeTxt.Add(g, kcText);
            //添加点击挂起士兵事件
        }

        UpdateAllUI(updateUICtk.Token);
    }

    private void UpdateSoliderGrids()
    {
        solider_kcToBtn.Clear();
        var sManager = GameMainEngine.Instance.player.sManager;
        sManager.UpdateGrids();
        var datas = sManager.GetDirs().Values.ToList();
        var keys = soliderGrids.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            var k = keys[i];
            var d = datas.Find(d => d.soliderID == i);
            soliderGrids[k] = d == null ? SoliderDataEntity.GetEmptyEntity() : d;
            ShowSingleSoliderGridsUI(k, soliderGrids[k]);
        }
    }

    private void ShowSingleSoliderGridsUI(Button soliderGridBtns, SoliderDataEntity soliderDataEntity)
    {
        soliderGridBtns.image.sprite = LoadWay.ResLoad<Sprite>(soliderDataEntity.iconPath);
        if (soliderDataEntity.kcSet == KeyCode.None)
        {
            soliderGridsKeyCodeTxt[soliderGridBtns].text = String.Empty;
        }
        else
        {
            soliderGridsKeyCodeTxt[soliderGridBtns].text = soliderDataEntity.kcSet.ToString().Replace("Alpha", "");
        }

        if (soliderDataEntity.kcSet != KeyCode.None)
            solider_kcToBtn.Add(soliderDataEntity.kcSet, soliderGridBtns);

        soliderGridBtns.onClick.RemoveAllListeners();
        soliderGridBtns.onClick.AddListener(() =>
        {
            UnitManager.Instance.playerSpawn.PutUpSolider(soliderDataEntity.kcSet);
        });
    }

    private async UniTask UpdateAllUI(CancellationToken ctk)
    {
        UpdateSoliderGrids();
        while (!ctk.IsCancellationRequested)
        {
            //创造的50帧更新，避免更新次数过多 后期将在受伤回调函数等调用，减少性能消耗
            await UniTask.WaitForSeconds(durHpUpdateTime).AttachExternalCancellation(ctk).SuppressCancellationThrow();
            UpdateSpawnHp();
            UpdatePlayerInfo();
            UpdateMoneyInfo();
        }
    }

    private void UpdateMoneyInfo()
    {
        var playerSMonenyMgr = GameMainEngine.Instance.player.monenyMgr;
        sMoneyTxt.text =
            $"SoliderMoneny\n{playerSMonenyMgr.fMoney.SoliderMoney}/{playerSMonenyMgr.fMoney.maxSoliderMoney}";
        eMonenTxt.text = $"SoliderMoneny\n{playerSMonenyMgr.fMoney.equipMoney}/{playerSMonenyMgr.fMoney.MaxEquipMoney}";
    }

    private void UpdatePlayerInfo()
    {
        //降低性能消耗，如果没有受到攻击就不执行下去了
        if (!GameMainEngine.Instance.player.defenceC.IfDamaged) return;
        var pData = GameMainEngine.Instance.player.data;
        SingleBarUIShow(playerHpImg, playerHpTxt, ref playerTextHpCurrent, pData.currentHp, pData.MaxHp);
        SingleBarUIShow(playerMysteryImg, playerMysteryTxt, ref playerTextMysteryCurrent, pData.mysteryBar,
            pData.MaxMysteryBar);
        SingleBarUIShow(playerExpImg, playerExpText, ref playerTextExpCurrent, pData.CurrentExp, pData.MaxExp);
        playerLevelText.text = $"L V : {pData.level}";
    }

    //更新血量
    public void UpdateSpawnHp()
    {
       // if (!UnitManager.Instance.playerSpawn.defenceC.IfDamaged &&
           // !UnitManager.Instance.enemySpawn.defenceC.IfDamaged) return;
        var mySpawn = UnitManager.Instance.playerSpawn.data;
        var enemySpawn = UnitManager.Instance.enemySpawn.data;
        var EMySpawn = UnitManager.Instance.playerSpawn.exData;
        var EEnemySpawn = UnitManager.Instance.enemySpawn.exData;
        UnitData leftData;
        UnitData rightData;
        //根据阵营切换血条
        if (EMySpawn.ct == CampType.Right)
        {
            leftData = enemySpawn;
            rightData = mySpawn;
        }
        else
        {
            leftData = mySpawn;
            rightData = enemySpawn;
        }

        SingleBarUIShow(leftSpawnHpImg, leftSpawnHpTxt, ref leftTextHpCurrent, leftData.currentHp, leftData.MaxHp);
        SingleBarUIShow(rightSpawnHpImg, rightSpawnHpTxt, ref rightTextHpCurrent, rightData.currentHp, rightData.MaxHp);
    }

    private void SingleBarUIShow(Image barUI, TMP_Text barUIText, ref float HPLerp, float currentNum, float maxNum)
    {
        if (MathF.Abs(HPLerp - currentNum) < 0.1f) //无限接近了，说明不需要在执行了
            return;
        if (HPLerp == 0) HPLerp = maxNum;
        barUI.fillAmount = Mathf.Lerp(barUI.fillAmount, currentNum / maxNum,
            UIInfos.HpUILerp * durHpUpdateTime * 5);
        HPLerp = Mathf.Lerp(HPLerp, currentNum, UIInfos.HpUILerp * durHpUpdateTime * 5);
        barUIText.text = $"{(int)HPLerp} / {(int)maxNum}";
        if (currentNum == 0 && HPLerp <= 1.5f)
            barUIText.text = $"{(int)0} / {(int)maxNum}";
    }
}

//小地图
public class miniMap
{
    public Vector2 mapSize = new Vector2(600, 300); //小地图大小
    public Camera miniMapRender;
    private HostPlayerUIUIComponent UI;
    private Transform iconContianer;
    private Transform whiteCameraFrame; //小地图上面白色的摄像机视野框
    private RectTransform whiteCameraFrameRect; //小地图上面白色的摄像机视野框
    private Dictionary<Unit, MiniIconUIComponent> iconDir;

    public miniMap(HostPlayerUIUIComponent ui)
    {
        UI = ui;
        iconDir = new Dictionary<Unit, MiniIconUIComponent>();
        var miniMap = ui.transform.FindTheTfByName("MiniMap");
        iconContianer = miniMap.transform.FindTheTfByName("Background");
        whiteCameraFrame = miniMap.transform.FindTheTfByName("WhiteScreen");
        whiteCameraFrameRect = whiteCameraFrame.GetComponent<RectTransform>();
        miniMapRender = GameMainEngine.Instance.player.transform.FindTheTfByName<Camera>("miniMap");
        var spawn1 = UnitManager.Instance.playerSpawn.transform.position.x;
        var spawn2 = UnitManager.Instance.enemySpawn.transform.position.x;
        var midPos = (spawn1 + spawn2) / 2.0f;
        miniMapRender.transform.position = new Vector3(midPos, 0, -10);

        //小地图点击立马跳转视角
        var btnEvent = iconContianer.AddComponent<BtnImagePointerEvent>();
        btnEvent.pointerDown = (e) =>
        {
            MapManager.Instance.MiniMapMoveCamera(e, whiteCameraFrameRect);
            UpdateWhiteCamraFrame();
        };
        btnEvent.dragAction = (e) =>
        {
            MapManager.Instance.MiniMapMoveCamera(e, whiteCameraFrameRect);
            UpdateWhiteCamraFrame();
        };
    }

    public void UpdateWhiteCamraFrame()
    {
        var pos = GameMainEngine.Instance.player.playerCamera.transform.position;
        pos.y = 0;
        var miniMapPos = MapManager.Instance.MapPosToMiniMapPos(pos);

        var limitLeft = Screen.width - mapSize.x - whiteCameraFrameRect.rect.width / 2.0F;
        var limitRight = Screen.width + whiteCameraFrameRect.rect.width / 2.0F;
        Vector2 limit = new Vector2(limitLeft, limitRight);

        miniMapPos.x = Mathf.Clamp(miniMapPos.x, limit.x, limit.y);
        whiteCameraFrame.transform.position = miniMapPos;
    }

    public void AddMiniMapIcon(Unit unit)
    {
        var fightUI = UIManager.Instance.AddUIWithPool<MiniIconUIComponent>("Prefab/UI/GameMain/UnitIcon", UILayer.Mid,
            default,
            (ui) => { ui.owner = unit; });
        fightUI.transform.SetParent(iconContianer);
        fightUI.UpdateByPool();
        iconDir.Add(unit, fightUI);
    }

    public void ReleaseMiniMapIcon(Unit unit, float delay = 0.0f)
    {
        GameObjectPool.Instance.Release(iconDir[unit].gameObject, delay);
        iconDir.Remove(unit);
    }
}
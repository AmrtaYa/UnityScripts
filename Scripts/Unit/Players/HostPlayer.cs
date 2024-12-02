using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using GJC.Helper;
using Unity.VisualScripting;
using UnityEngine;

public class HostPlayer : Unit
{
    public BastInput input;
    public HorController horCtrl;
    [HideInInspector] public Camera playerCamera;
    [HideInInspector] public GameObject playerEntity;
    [HideInInspector] public SoliderManager sManager;
    public MoneyManager monenyMgr;
    public HostPlayerUIUIComponent fightUI;

    public override void Init()
    {
        base.Init();
        GameMainEngine.Instance.player = this;
        data = new UnitData()
        {
            Speed = 10,
            level = 1,
            currentHp = 500,
            MaxExp = 100,
            CurrentExp = 100,
            MaxHp = 500,
            mysteryBar = 50,
            MaxMysteryBar = 100,
            attack = 10,
            forwardOffsetRange = new Vector2(0.5F,0)
        };
        exData = new UnitExData()
        {
            ct =  CampType.Left
        };
        playerEntity = transform.FindTheTfByName("Entity").gameObject;
        playerCamera = transform.FindTheTfByName<Camera>("PlayerCamera");

        character = this.GetComponentInChildren<Character>();
        animMgr = new UnitAnimManager(this);
        attackC = new AttackComponent(this);
        
        defenceC = new DefenceComponent(this);
        horCtrl = new HorController(this);
        sManager = new SoliderManager(this);
        moveC = new HostPlayerMoveC(this);
        pyhsic = new PhysicController(this);
        monenyMgr = new MoneyManager(this);
        input = new PCInput(this);
        
        attackC.Init();
        animMgr.Init();
        AddFightUI();
    }

    public void AddFightUI()
    {
        fightUI = UIManager.Instance.AddUI<HostPlayerUIUIComponent>("Prefab/UI/GameMain/HostPlayerUI");
        fightUI.miniMap.AddMiniMapIcon(this);
    }


    protected override void _Update()
    {
        base._Update();
        //检测每帧的输入
        input?.InputUpdateChekc();

        UpdateRoadInfo();
    }

    private void UpdateRoadInfo()
    {
        float oriDataY = entity.localPosition.y;
        if (oriDataY < 0) oriDataY -= 1;
        var road = (MapRoad)(Mathf.Clamp((int)oriDataY,-3,1)) ;
        data.road = road;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using SQLite4Unity3d;
using UnityEngine;

/// <summary>
/// 玩家金币管理系统
/// </summary>
[Serializable]
public class MoneyManager
{
    public Unit owner;
    public MoneyEntity e;
    public FightSceneMoney fMoney;
    public float moneyIncreaseSpeed = 1;
    public float increaseIntravlTime = 1.0f;//每隔一秒涨钱
    public CancellationTokenSource moneyIncreaseStop;
    
    public MoneyManager(Unit hostPlayer)
    {
        owner = hostPlayer;
        moneyIncreaseStop = new CancellationTokenSource();
        e = DBManager.GetEntity<MoneyEntity>(0); //从数据库拿到钱包数据，一个存档只会有一个钱包
        fMoney = new FightSceneMoney()
        {
            equipMoney = 0,
            MaxEquipMoney = 10000,
            SoliderMoney = 0,
            maxSoliderMoney = 1000
        };
        IncreaseMoneySystem(moneyIncreaseStop.Token);
    }

    public  async void IncreaseMoneySystem(CancellationToken ctk)
    {
        while (!ctk.IsCancellationRequested)
        {
            await UniTask.WaitForSeconds(increaseIntravlTime).AttachExternalCancellation(ctk).SuppressCancellationThrow();
            if (fMoney.SoliderMoney < fMoney.maxSoliderMoney)
            {
                fMoney.SoliderMoney += moneyIncreaseSpeed;
                if (fMoney.SoliderMoney >= fMoney.maxSoliderMoney)
                    fMoney.SoliderMoney = fMoney.maxSoliderMoney;
            }
        }
    }
}

public class FightSceneMoney
{
    public float maxSoliderMoney;
    public float SoliderMoney;
    public float MaxEquipMoney;
    public float equipMoney;

    public void Clear()
    {
        maxSoliderMoney = 0;
        SoliderMoney = 0;
        MaxEquipMoney = 0;
        equipMoney = 0;
    }
}

public class MoneyEntity : IDataBase
{
    [PrimaryKey] public int ID { get; set; }
    public float BagMoney { get; set; }

    public int GetPrimKey()
    {
        return ID;
    }

    public Type[] dependEntity()
    {
        return null;
    }

    public IDataBase GetDefaultEntity(int id)
    {
        return new MoneyEntity()
        {
        };
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 电脑端输入
/// </summary>
public class PCInput : BastInput
{
    private HostPlayer player;

    public PCInput(HostPlayer player)
    {
        this.player = player;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public override void InputUpdateChekc()
    {
        base.InputUpdateChekc();
        TestInput();
        AttackChecked();
        MoveCheked();
        HorChecked();
        SoldierChecked();
    }

    /// <summary>
    /// editor专用
    /// </summary>
    private void TestInput()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
        }
#endif
    }

    /// <summary>
    /// 出士兵检测
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void SoldierChecked()
    {
        if (UnitManager.Instance.playerSpawn == null) return;
        for (int i = 0; i < GameMainEngine.Instance.AmriesKeyCodes.Count; i++)
        {
            if (Input.GetKeyDown(GameMainEngine.Instance.AmriesKeyCodes[i]))
            {
                UnitManager.Instance.playerSpawn.PutUpSolider(GameMainEngine.Instance.AmriesKeyCodes[i]);
            }
        }

        MapManager.Instance.ChoosedColor();

        if (Input.GetMouseButtonDown(0))
        {
            UnitManager.Instance.playerSpawn.CreateSoldier();
        }

        if (Input.GetMouseButtonDown(1))
        {
            UnitManager.Instance.playerSpawn.CancelSolider();
        }
    }

    private void AttackChecked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bool ifReleaseSkill = player.attackC.skillManager.ReleaseSkillAnim();
            //技能优先级大于普攻
            if (!ifReleaseSkill)
                GameMainEngine.Instance.player.attackC.CommonAttack();
        }

        if (Input.GetKeyDown(GameMainEngine.Instance.gameSetting.Skill01))
        {
            player.attackC.skillManager.ReadySkillHostPlayer(1);
        }

        if (Input.GetKeyDown(GameMainEngine.Instance.gameSetting.Skill02))
        {
            player.attackC.skillManager.TestSkill();
        }
    }

    private void HorChecked()
    {
        if (player.horCtrl == null) return;
        player.horCtrl.ScreenMoveChecked();
        if (Input.GetKeyDown(GameMainEngine.Instance.gameSetting.lockHor))
        {
            player.horCtrl.IfLockHor = !player.horCtrl.IfLockHor;
        }


        if (Input.GetKey(GameMainEngine.Instance.gameSetting.backCharacterHor) || player.horCtrl.IfLockHor)
        {
            player.horCtrl.BackToPlayerEntity();
        }
    }

    /// <summary>
    /// 移动检测
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void MoveCheked()
    {
        if (player.moveC == null) return;
        Vector2 moveVec = Vector2.zero;
        if (Input.GetKey(GameMainEngine.Instance.gameSetting.mLeft)) moveVec.x = -1;
        if (Input.GetKey(GameMainEngine.Instance.gameSetting.mRight)) moveVec.x = 1;
        if (Input.GetKey(GameMainEngine.Instance.gameSetting.mForward)) moveVec.y = 1;
        if (Input.GetKey(GameMainEngine.Instance.gameSetting.mBack)) moveVec.y = -1;
        player.moveC.Move(moveVec);
    }
}
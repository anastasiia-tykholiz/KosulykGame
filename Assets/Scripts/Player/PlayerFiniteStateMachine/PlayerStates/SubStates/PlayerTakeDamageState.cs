using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamageState : PlayerState
{
    public PlayerTakeDamageState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        core.Stats.CantTakeDamage();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isExitingState == false)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
    }
}

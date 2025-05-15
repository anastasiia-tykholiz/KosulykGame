using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        _player.JumpState.ResetAmountOfJumpsLeft();
    }
    public override void Exit()
    {
        base.Exit();

        _player.JumpState.DecreaseAmountOfJumpsLeft();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isExitingState == false)
        {
            core.Movement.SetVelocityY(-_playerData.wallSlideVelocity);
        }
    }
}

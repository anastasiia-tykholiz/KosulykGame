using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int _wallJumpDirection;
    public PlayerWallJumpState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _player.InputHandler.UseJumpInput();
        _player.JumpState.ResetAmountOfJumpsLeft();         
        _player.JumpState.DecreaseAmountOfJumpsLeft();
        core.Movement.CheckIfShouldFlip(_wallJumpDirection);
        core.Movement.SetVelocity(_playerData.wallJumpVelocity, _playerData.wallJumpAngle, _wallJumpDirection);
              
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _player.Anim.SetFloat("yVelocity", core.Movement.GetVelocityY());
        _player.Anim.SetFloat("xVelocity", Mathf.Abs(core.Movement.CurrentVelocity.x));

        if (Time.time >= _startTime + _playerData.wallJumpTime)
        {
            _isAbilityDone = true;
        }
    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall == true)
        {
            _wallJumpDirection = -core.Movement.FacingDirection;
        }
        else
        {
            _wallJumpDirection = core.Movement.FacingDirection;
        }
    }
}

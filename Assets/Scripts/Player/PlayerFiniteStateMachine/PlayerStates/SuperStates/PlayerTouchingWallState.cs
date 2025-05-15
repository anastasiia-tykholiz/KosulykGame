using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected int _xInput;

    protected bool _isGrounded;
    protected bool _isTouchingWall;
    protected bool _jumpInput;
    protected bool _isTouchingLedge;
    private bool _canWallJump;
    protected bool _isTakeDamage;
    private bool _isDead;

    private bool _canLedgeLlimb = false; // відключення вскарабкування на виступи
    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = core.CollisionSenses.IsGrounded();
        _isTouchingWall = core.CollisionSenses.IsTouchingWall();
        _isTouchingLedge = core.CollisionSenses.IsTouchingLedge();

        if (_isTouchingWall == true && _isTouchingLedge == false)
        {
            _player.LedgeClimbState.SetDetectedPosition(_player.transform.position);
        }

        if (_playerData.canWallJump == "true")
        {
            _canWallJump = true;
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _isTakeDamage = core.Stats.IsTakeDamage;
        _isDead = core.Stats.IsDead;
        _xInput = _player.InputHandler.NormalizedInputX;
        _jumpInput = _player.InputHandler.JumpInput;

        if (_isTakeDamage == true)
        {
            _player.StateMachine.ChangeState(_player.TakeDamageState);
        }
        else if (_isDead == true)
        {
            _stateMachine.ChangeState(_player.FaintState);
        }
        else if (_jumpInput == true && _canWallJump == true)
        {
            _player.WallJumpState.DetermineWallJumpDirection(_isTouchingWall);
            _stateMachine.ChangeState(_player.WallJumpState);
        }
        else if (_isGrounded == true)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
        else if (_isTouchingWall == false || _xInput != core.Movement.FacingDirection)
        {
            _stateMachine.ChangeState(_player.InAirState);
        }
        else if ((_isTouchingWall == true && _isTouchingLedge == false) && _canLedgeLlimb == true)
        {
            _stateMachine.ChangeState(_player.LedgeClimbState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

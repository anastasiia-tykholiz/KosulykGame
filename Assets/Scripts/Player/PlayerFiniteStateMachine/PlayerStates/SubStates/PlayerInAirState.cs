using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private int _xInput;

    private bool _isGrounded;
    private bool _isJumping;
    private bool _isTouchingLedge;
    private bool _isTouchingWall;
    private bool _oldIsTouchingWall;
    private bool _isTouchingWallBack;
    private bool _oldIsTouchingWallBack;
    private bool _jumpInput;
    private bool _jumpInputStop;
    public bool _coyoteTime;
    private bool _isWallJumpCoyoteTime;
    private float _startWallJumpCoyoteTime;
    private bool _isTakeDamage;
    private bool _isDead;
    private bool _canWallJump;

    private bool _canLedgeLlimb = false; // відключення вскарабкування на виступи
    public PlayerInAirState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _oldIsTouchingWall = _isTouchingWall;
        _oldIsTouchingWallBack = _isTouchingWallBack;

        _isGrounded = core.CollisionSenses.IsGrounded();
        _isTouchingWall = core.CollisionSenses.IsTouchingWall();
        _isTouchingWallBack = core.CollisionSenses.IsTouchingWallBack();
        _isTouchingLedge = core.CollisionSenses.IsTouchingLedge();

        if (_isTouchingWall == true && _isTouchingLedge == false)
        {
            _player.LedgeClimbState.SetDetectedPosition(_player.transform.position);
        }

        if (!_isWallJumpCoyoteTime && !_isTouchingWall && !_isTouchingWallBack && (_oldIsTouchingWall || _oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
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

        _player.Anim.SetFloat("yVelocity", core.Movement.CurrentVelocity.y);
        _oldIsTouchingWall = false;
        _isTouchingWall = false;
        _oldIsTouchingWallBack = false;
        _isTouchingWallBack = false;
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        _xInput = _player.InputHandler.NormalizedInputX;
        _jumpInput = _player.InputHandler.JumpInput;
        _jumpInputStop = _player.InputHandler.JumpInputStop;
        _isTakeDamage = core.Stats.IsTakeDamage;
        _isDead = core.Stats.IsDead;

        CheckJumpBuffer();
        if (_isTakeDamage == true)
        {
            _stateMachine.ChangeState(_player.TakeDamageState);
        }
        else if (_isDead == true)
        {
            _stateMachine.ChangeState(_player.FaintState);
        }
        else if (_isGrounded == true && core.Movement.CurrentVelocity.y < 0.01f)
        {
            _stateMachine.ChangeState(_player.LandState);
        }
        else if ((_isTouchingWall == true && _isTouchingLedge == false && _isGrounded == false) && _canLedgeLlimb == true)
        {
            _player.StateMachine.ChangeState(_player.LedgeClimbState);
        }
        else if ((_jumpInput == true && (_isTouchingWall == true || _isTouchingWallBack == true) && _canWallJump == true))
        { 
            StopWallJumpCoyoteTime();             
            
            _isTouchingWall = core.CollisionSenses.IsTouchingWall();
            _player.WallJumpState.DetermineWallJumpDirection(_isTouchingWall);
            
            _stateMachine.ChangeState(_player.WallJumpState);
        }
        else if (_jumpInput == true && _player.JumpState.CanJump() == true)
        {
            _stateMachine.ChangeState(_player.JumpState);
        }
        else if (_isTouchingWall == true && _xInput == core.Movement.FacingDirection && core.Movement.CurrentVelocity.y <= 0)
        {
            _stateMachine.ChangeState(_player.WallSlideState);
        }
        else
        {
            if (_isWallJumpCoyoteTime == false)
            {
                core.Movement.CheckIfShouldFlip(_xInput);
            }
            core.Movement.SetVelocityX(_playerData.movementVelocity * _xInput);

            _player.Anim.SetFloat("yVelocity", core.Movement.CurrentVelocity.y);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckJumpBuffer()
    {
        if (_isJumping == true)
        {
            if (_jumpInputStop == true)
            {
                core.Movement.SetVelocityY(core.Movement.CurrentVelocity.y * _playerData.jumpBuffer);
                _isJumping = false;
            }
            else if (core.Movement.CurrentVelocity.y <= 0f)
            {
                _isJumping = false;
            }
        }
    }

    private void CheckCoyoteTime()
    {
        if (_coyoteTime == true && Time.time > _startTime + _playerData.coyoteTime)
        {
            _coyoteTime = false;
            _player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }
    public void CheckWallJumpCoyoteTime()
    {
        if (_isWallJumpCoyoteTime == true && Time.time > _startWallJumpCoyoteTime + _playerData.coyoteTime)
        {
            _isWallJumpCoyoteTime = false;
        }
    }

    public void StartCoyoteTime()
    {
        _coyoteTime = true;
    }
    public void StartWallJumpCoyoteTime()
    {
        _isWallJumpCoyoteTime = true;
        _startWallJumpCoyoteTime = Time.time;
    }
    public void StopWallJumpCoyoteTime()
    {
        _isWallJumpCoyoteTime = false;
    }
    public void SetIsJumping()
    {
        _isJumping = true;
    }
}

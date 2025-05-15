using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int _xInput;

    protected bool _JumpInput;
    protected bool _interactInput;
    protected bool _isGrounded;
    protected bool _isTakeDamage;
    private bool _isDead;
    public PlayerGroundedState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = core.CollisionSenses.IsGrounded();   
    }

    public override void Enter()
    {
        base.Enter();
        _player.JumpState.ResetAmountOfJumpsLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _xInput = _player.InputHandler.NormalizedInputX;
        _JumpInput = _player.InputHandler.JumpInput;
        _interactInput = _player.InputHandler.InteractInput;
        _isTakeDamage = core.Stats.IsTakeDamage;
        _isDead = core.Stats.IsDead;

        if (_isTakeDamage == true)
        {
            _player.StateMachine.ChangeState(_player.TakeDamageState);
        }
        else if (_isDead == true)
        {
            _stateMachine.ChangeState(_player.FaintState);
        }
        else if (_JumpInput == true && _player.JumpState.CanJump() == true)
        {
            _stateMachine.ChangeState(_player.JumpState);
        }
        else if (_isGrounded == false)
        {
            _player.InAirState.StartCoyoteTime();
            _stateMachine.ChangeState(_player.InAirState);
        }
        else if (core.CollisionSenses.IsInteracting && _interactInput)
        {
            _player.InputHandler.UseInteractInput();
            _stateMachine.ChangeState(_player.InteractionState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

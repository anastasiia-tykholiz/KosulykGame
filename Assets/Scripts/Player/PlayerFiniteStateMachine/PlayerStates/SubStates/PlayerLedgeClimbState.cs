using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 _detecterPosition;
    private Vector2 _cornerPosition;
    private Vector2 _startPosition;
    private Vector2 _stopPosition;
    private Vector2 workspace;

    private bool _isHanging;
    private bool _isClimbing;
    private bool _jumpInput;

    private int xInput;
    private int yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        _player.Anim.SetBool("ledgeClimb", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        _isHanging = true;
    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityZero();
        _player.transform.position = _detecterPosition;
        _cornerPosition = DetermineCornerPosition();

        _startPosition.Set(_cornerPosition.x - (core.Movement.FacingDirection * _playerData.startOffset.x), _cornerPosition.y - _playerData.startOffset.y);
        _stopPosition.Set(_cornerPosition.x + (core.Movement.FacingDirection * _playerData.stopOffset.x), _cornerPosition.y + _playerData.stopOffset.y);

        _player.transform.position = _startPosition;
    }

    public override void Exit()
    {
        base.Exit();

        _isHanging = false;

        if (_isClimbing == true)
        {
            _player.transform.position = _stopPosition;
            _isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(_isAnimationFinished == true)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
        else
        {
            xInput = _player.InputHandler.NormalizedInputX;
            yInput = _player.InputHandler.NormalizedInputY;
            _jumpInput = _player.InputHandler.JumpInput;

            core.Movement.SetVelocityZero();
            _player.transform.position = _startPosition;

            if (xInput == core.Movement.FacingDirection && _isHanging == true && _isClimbing == false)
            {
                _isClimbing = true;
                _player.Anim.SetBool("ledgeClimb", true);
            }
            else if (yInput == -1 && _isHanging == true && _isClimbing == false)
            {
                _stateMachine.ChangeState(_player.InAirState);
            }
            else if(_jumpInput && _isClimbing == false && _isHanging == true)
            {
                _player.WallJumpState.DetermineWallJumpDirection(true);
                _stateMachine.ChangeState(_player.WallJumpState);
            }
        }      
    }

    public void SetDetectedPosition(Vector2 position)
    {
        _detecterPosition = position;
    }

    private Vector2 DetermineCornerPosition()
    {
        RaycastHit2D raycastHitX = Physics2D.Raycast(core.CollisionSenses.WallCheck1.position, Vector2.right * core.Movement.FacingDirection, core.CollisionSenses.WallCheckDistance, core.CollisionSenses.GroundLayer);
        float xDistance = raycastHitX.distance;

        workspace.Set((xDistance + 0.015f) * core.Movement.FacingDirection, 0f);

        RaycastHit2D raycastHitY = Physics2D.Raycast(core.CollisionSenses.LedgeCheck.position + (Vector3)(workspace), Vector2.down, core.CollisionSenses.LedgeCheck.position.y - core.CollisionSenses.WallCheck1.position.y + 0.015f, core.CollisionSenses.GroundLayer);
        float yDistance = raycastHitY.distance;

        workspace.Set(core.CollisionSenses.WallCheck1.position.x + (xDistance * core.Movement.FacingDirection), core.CollisionSenses.LedgeCheck.position.y - yDistance);

        return workspace;
    }
}

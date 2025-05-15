using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartIdleState : PlayerGroundedState
{
    public PlayerStartIdleState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isExitingState == false)
        {   
            if (_xInput != 0)
            {
                _stateMachine.ChangeState(_player.StartMoveState);
            }
            else if (_isAnimationFinished == true)
            {
                _stateMachine.ChangeState(_player.IdleState);
            }
        }     
    }
}

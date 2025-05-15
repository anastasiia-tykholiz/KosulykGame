using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionState : PlayerGroundedState
{
    public PlayerInteractionState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        core.CollisionSenses.CanInteract();
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

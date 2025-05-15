using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFaintState : PlayerState
{
    private bool _canRestart = true;

    public PlayerFaintState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        core.Movement.SetVelocityX(0);
        if (_isAnimationFinished == true)
        {
            if (_canRestart == true)
            {
                core.Stats.RestartLvl();
                _canRestart = false;
            }
        }
    }
}

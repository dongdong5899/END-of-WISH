using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(PlayerBrain player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.CanNextAnimation = true;
        player.InputCompo.MovementEvent += HandleMovementEvent;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (player.IsRuning)
        {
            player.StateMachine.StateChange(PlayerStateEnum.Run);
        }
    }

    private void HandleMovementEvent(Vector3 movement)
    {
        if (movement.sqrMagnitude == 0)
        {
            player.StateMachine.StateChange(PlayerStateEnum.Idle);
        }
        player.MoveCompo.SetMovement(
            (movement.x * player.rotationTrm.right
            + movement.z * Vector3.ProjectOnPlane(player.rotationTrm.forward, Vector3.up)).normalized
            * player.walkSpeed, true);
    }

    public override void Exit()
    {
        base.Exit();
        player.InputCompo.MovementEvent -= HandleMovementEvent;
    }
}

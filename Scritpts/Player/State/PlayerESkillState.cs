using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerESkillState : PlayerState
{
    private ChipSO _currentChipSO;

    public PlayerESkillState(PlayerBrain player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _currentChipSO = GameManager.Instance.currentChip;

        _currentChipSO.eSkill.Skill.Enter(player);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (player.OnSkill == true && player.CanNextAnimation)
        {
            player.OnSkill = false;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _currentChipSO.eSkill.Skill.Exit();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.StateChange(PlayerStateEnum.Idle);
    }
}

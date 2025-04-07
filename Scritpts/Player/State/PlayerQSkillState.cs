using UnityEngine;

public class PlayerQSkillState : PlayerState
{
    private ChipSO _currentChipSO;

    public PlayerQSkillState(PlayerBrain player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _currentChipSO = GameManager.Instance.currentChip;

        _currentChipSO.qSkill.Skill.Enter(player);
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
        _currentChipSO.qSkill.Skill.Exit();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.StateChange(PlayerStateEnum.Idle);
    }
}

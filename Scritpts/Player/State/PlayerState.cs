using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerBrain player;

    private int animBoolHash;
    protected bool animEndTriggerCalled;

    public PlayerState(PlayerBrain player, PlayerStateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        animBoolHash = Animator.StringToHash(animBoolName);
    }
    public virtual void Enter()
    {
        player.CanNextAnimation = false;
        player.AnimatorCompo.SetBool(animBoolHash, true);
        animEndTriggerCalled = false;
    }

    public virtual void UpdateState()
    {

    }

    public virtual void Exit()
    {

        player.AnimatorCompo.SetBool(animBoolHash, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        animEndTriggerCalled = true;
    }
}

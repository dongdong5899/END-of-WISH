using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorTrigger : MonoBehaviour
{
    private PlayerBrain player;

    private void Awake()
    {
        player = transform.parent.GetComponent<PlayerBrain>();
    }

    public void AnimationEnd()
    {
        player.StateMachine.CurrentState.AnimationFinishTrigger();
    }

    public void StartEffect()
    {
        player.StartEffectEvent?.Invoke();
    }

    public void CanNextAnimation()
    {
        player.CanNextAnimation = true;
    }
}

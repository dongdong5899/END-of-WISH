using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStingSkill : Skill
{
    private Coroutine _moveCoroutine;
    private Coroutine _endCoroutine;

    public override void Enter(PlayerBrain player)
    {
        base.Enter(player);

        player.MoveCompo.SetInputRotation();
        player.MoveCompo.SetMovement(player.visualTrm.forward * 30, true);

        Poolable effect = PoolManager.Instance.Pop("IceSting");
        Vector3 offset = new Vector3(0, 1, 5.5f);
        Vector3 effectPos = player.transform.position + player.visualTrm.rotation * offset;
        effect.transform.localScale = Vector3.one * 0.9f;
        effect.transform.SetPositionAndRotation(effectPos, player.visualTrm.rotation);
        effect.transform.DOMove(effect.transform.position + effect.transform.forward * 4f, 0.7f).SetEase(Ease.OutSine);

        CameraManager.Instance.CameraShake(4f, 8f, 0.2f);

        _moveCoroutine = GameManager.Instance.DelayCallback
            (new WaitForSeconds(0.15f), player.MoveCompo.MoveStop);

        _endCoroutine = GameManager.Instance.DelayCallback
            (new WaitForSeconds(0.25f), () => 
            player.StateMachine.StateChange(PlayerStateEnum.Idle));
    }

    protected override void HandleStartEffectEvent()
    {
        base.HandleStartEffectEvent();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        GameManager.Instance.StopDelayCallback(_moveCoroutine);
        GameManager.Instance.StopDelayCallback(_endCoroutine);
    }
}

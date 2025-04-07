using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTornadoSkill : Skill
{
    private Coroutine _skillCoroutine;
    public override void Enter(PlayerBrain player)
    {
        base.Enter(player);

        player.MoveCompo.MoveStop();

        _skillCoroutine = GameManager.Instance.DelayCallback(new WaitForSeconds(0.3f), () =>
        {
            WindTornado windTornado = PoolManager.Instance.Pop("WindTornado").GetComponent<WindTornado>();
            windTornado.SetOwner(player.visualTrm);

            CameraManager.Instance.CameraShake(8, 8, 3, DG.Tweening.Ease.OutQuart);

            player.StateMachine.StateChange(PlayerStateEnum.Idle);
        });
        

        player.StartEffectEvent -= HandleStartEffectEvent;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();

        GameManager.Instance.StopDelayCallback(_skillCoroutine);
    }

    protected override void HandleStartEffectEvent()
    {
        base.HandleStartEffectEvent();
    }
}

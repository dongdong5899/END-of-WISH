using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShockWaveSkill : Skill
{
    private Coroutine _skillCoroutine;
    public override void Enter(PlayerBrain player)
    {
        base.Enter(player);

        _player.MoveCompo.MoveStop();

        _skillCoroutine = GameManager.Instance.DelayCallback(new WaitForSeconds(0.2f), HandleStartEffectEvent);
    }

    protected override void HandleStartEffectEvent()
    {
        base.HandleStartEffectEvent();

        Poolable shockWave = PoolManager.Instance.Pop("IceShockWave");
        shockWave.Initialize();
        shockWave.transform.position = _player.transform.position;

        CameraManager.Instance.CameraShake(10, 10, 0.3f);

        _player.StateMachine.StateChange(PlayerStateEnum.Idle);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();

        _player.StartEffectEvent -= HandleStartEffectEvent;

        GameManager.Instance.StopDelayCallback(_skillCoroutine);
    }
}

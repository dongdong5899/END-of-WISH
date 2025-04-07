using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRoadSkill : Skill
{
    public override void Enter(PlayerBrain player)
    {
        base.Enter(player);
        _player.MoveCompo.SetInputRotation();
        _player.OnSkill = true;

        Vector3 movement = _player.visualTrm.forward * 10;
        _player.MoveCompo.SetMovement(movement, false);
        _player.MoveCompo.SetJump(0.8f);
    }

    protected override void HandleStartEffectEvent()
    {
        _player.MoveCompo.MoveStop();

        Poolable obj = PoolManager.Instance.Pop("FireTempest");
        obj.transform.rotation = _player.visualTrm.rotation;
        obj.transform.SetPositionAndRotation(
            _player.transform.position,
            _player.visualTrm.rotation);

        CameraManager.Instance.CameraShake(5.5f, 3, 0.5f);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();

        _player.StartEffectEvent -= HandleStartEffectEvent;
    }
}

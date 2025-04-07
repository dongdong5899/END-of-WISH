using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindShieldSkill : Skill
{
    public override void Enter(PlayerBrain player)
    {
        base.Enter(player);
        player.StartEffectEvent -= HandleStartEffectEvent;

        WindShield windSield = PoolManager.Instance.Pop("WindShield").GetComponent<WindShield>();
        windSield.SetTarget(player.transform);

        player.StateMachine.StateChange(PlayerStateEnum.Idle);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected override void HandleStartEffectEvent()
    {
        base.HandleStartEffectEvent();
    }
}

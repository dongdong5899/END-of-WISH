using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WispSkill : Skill
{

    public override void Enter(PlayerBrain player)
    {
        base.Enter(player);

        GameManager.Instance.StartCoroutine(SpawnCoroutine(player.transform));

        _player.StartEffectEvent -= HandleStartEffectEvent;
        player.StateMachine.StateChange(PlayerStateEnum.Idle);
    }

    private IEnumerator SpawnCoroutine(Transform centerTrm)
    {
        Wisp[] wisps = new Wisp[3];
        for (int i = 0; i < 3; i++)
        {
            wisps[i] = PoolManager.Instance.Pop("Flame").GetComponent<Wisp>();
            wisps[i].Initialize();
            wisps[i].SetPosition(centerTrm, i);
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0;i < 3; i++)
        {
            wisps[i].OnActive();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

    protected override void HandleStartEffectEvent()
    {
        base.HandleStartEffectEvent();
    }
}

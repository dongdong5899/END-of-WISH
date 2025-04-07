using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public bool isSkillEnd;
    public bool isAnimationEnd;

    protected PlayerBrain _player;

    private Coroutine _effectCoroutine;

    protected virtual void HandleStartEffectEvent()
    {

    }

    public virtual void Enter(PlayerBrain player)
    {
        _player = player;

        _effectCoroutine = GameManager.Instance.DelayFrameCallback(1, () => _player.StartEffectEvent += HandleStartEffectEvent);
    }
    public virtual void Update()
    {
        if (_player.OnSkill == true && _player.CanNextAnimation)
        {
            _player.OnSkill = false;
        }
    }
    public virtual void Exit()
    {
        if (_effectCoroutine != null)
        {
            GameManager.Instance.StopDelayCallback(_effectCoroutine);
        }
        _player.OnSkill = false;
    }
}

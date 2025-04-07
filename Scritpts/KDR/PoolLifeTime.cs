using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolLifeTime : Poolable
{
    [SerializeField] private float lifeTime = 2f;

    private Coroutine _lifeTimeCoroutine;

    public override void Initialize()
    {
        _lifeTimeCoroutine = GameManager.Instance.DelayCallback(new WaitForSeconds(lifeTime), Die);
    }

    public void Die()
    {
        GameManager.Instance.StopDelayCallback(_lifeTimeCoroutine);
        PoolManager.Instance.Push(this);
    }
}

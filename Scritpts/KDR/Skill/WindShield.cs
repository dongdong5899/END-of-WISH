using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindShield : PoolLifeTime
{
    private Transform _followTargetTrm;

    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _whatIsBullet;
     
    private void Update()
    {
        transform.position = _followTargetTrm.position;

        CheckBullet();
    }

    public void SetTarget(Transform targetTrm)
    {
        _followTargetTrm = targetTrm;
    }

    private void CheckBullet()
    {
        Collider[] colls = Physics.OverlapCapsule
            (transform.position + Vector3.down * 2, transform.position + Vector3.up * 2, _radius, _whatIsBullet);

        foreach (Collider coll in colls)
        {
            if (coll.TryGetComponent(out PoolLifeTime poolLifetTime))
            {
                Transform cuttingEffect = PoolManager.Instance.Pop("WindBulletCutting").transform;
                cuttingEffect.position = coll.transform.position;
                cuttingEffect.rotation = Random.rotation;

                poolLifetTime.Die();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * 2, _radius);
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 2, _radius);
    }
}

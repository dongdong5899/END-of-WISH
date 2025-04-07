using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wisp : PoolLifeTime
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _targetRadius = 1;
    [SerializeField] private float _radius = 1;
    [SerializeField] private int _damage = 10;
    [SerializeField] private LayerMask _targetLayer;

    private Transform _targetTrm = null;
    private Transform _centerTrm = null;
    private Vector3 _offsetPos;
    private Vector3 _targetPos;
    private bool _isActive = false;

    private const float RatationValue = Mathf.PI * 2 / 3;

    public override void Initialize()
    {
        base.Initialize();
        _targetTrm = null;
        _isActive = false;
    }

    public void OnActive()
    {
        _isActive = true;
    }

    public void SetPosition(Transform centerTrm, int index)
    {
        _centerTrm = centerTrm;
        Vector3 pos = new Vector3(Mathf.Cos(RatationValue * index), 0, Mathf.Sin(RatationValue * index)) * 2;
        pos.y = 1;
        _offsetPos = pos;
        transform.position = _centerTrm.position + Quaternion.Euler(0, Time.time * 10, 0) * _offsetPos;
    }

    private void FixedUpdate()
    {
        if (Physics.SphereCast(transform.position + Vector3.down * (_targetRadius + 2), _targetRadius,
            Vector3.up, out RaycastHit hit, _targetRadius + 2, _targetLayer))
        {
            _targetTrm = hit.transform;
        }
        else
        {
            _targetTrm = null;
        }

        if (_targetTrm != null && _isActive)
            Move();
        else
        {
            _targetPos = _centerTrm.position + Quaternion.Euler(0, Time.time * 100, 0) * _offsetPos;
            transform.position += (_targetPos - transform.position).normalized * Time.fixedDeltaTime * _speed;
        }
    }

    private void Move()
    {
        Vector3 movement = (_targetTrm.position + Vector3.up - transform.position).normalized
            * Time.fixedDeltaTime * _speed;
        if (Physics.SphereCast(transform.position, _radius, movement.normalized, 
            out RaycastHit hit, movement.magnitude, _targetLayer))
        {
            //Ãæµ¹
            transform.position += movement.normalized * hit.distance;
            if (hit.transform.TryGetComponent(out IHealth health))
                health.ApplyDamage(_damage, Vector3.zero, Vector3.zero, 10, 0.2f, this);

            CameraManager.Instance.CameraShake(7f, 7f, 0.13f);

            PoolManager.Instance.Push(this);
        }
        else
        {
            transform.position += movement;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}

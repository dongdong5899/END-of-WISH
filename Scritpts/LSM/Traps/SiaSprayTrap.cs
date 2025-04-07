using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class SiaSprayTrap : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float _radius;
    [SerializeField] private float _angle;
    [SerializeField] private float _alpa;
    [SerializeField] private float _holdingTime;
    [SerializeField] private LayerMask _playerMask;

    private bool _isRangeInPlayer;

    private void Update()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, _radius, _playerMask);

        if (collider.Length >= 1)
        {
            RangeCheck(collider[0]);
        }
        else if (_isRangeInPlayer)
        {
            TrapManager.Instance.ActiveSiaTrap(false);
            _isRangeInPlayer = false; 
        }
    }

    private void RangeCheck(Collider collider)
    {
        Vector3 forwardVec = transform.forward.normalized;
        Vector3 playerVec = (collider.transform.position - transform.position).normalized;

        float value = Vector3.Dot(playerVec, forwardVec);
        float theta = Mathf.Acos(value);
        float degree = theta * Mathf.Rad2Deg;
        if (_angle / 2 > degree)
        {
            if (_isRangeInPlayer == false)
            {
                TrapManager.Instance.ActiveSiaTrap(true);
                _isRangeInPlayer = true;
            }
        }
        else if(_isRangeInPlayer)
        {
            TrapManager.Instance.ActiveSiaTrap(false);
            _isRangeInPlayer = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);

#if UNITY_EDITOR
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, _angle / 2, _radius);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -_angle / 2, _radius);
#endif
    }

}

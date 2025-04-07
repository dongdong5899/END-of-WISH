using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ThomTrap : MonoBehaviour
{
    enum OverlapEnum
    {
        Sphere,
        Box
    }

    [Header("Setting")]
    [SerializeField] OverlapEnum overlapEnum;
    [SerializeField] float _coolTime;
    [SerializeField] float _hitCoolTime;
    [SerializeField] float _sphereRadius;
    [SerializeField] int _damage;
    [SerializeField] Vector3 _boxSize;
    [SerializeField] LayerMask _playerMask;

    [SerializeField] float _shakeTime = 0.5f;
    [SerializeField] float _amplitude = 5f;
    [SerializeField] float _frequency = 2f;
    [SerializeField] Ease _easeType;

    private Transform _visual;
    private SphereCollider _collider;

    private void Awake()
    {
        
        _collider = GetComponent<SphereCollider>();
        _visual = transform.Find("Visual");
    }
    //private Vector3 SetDicrection(TrapDirection directionEnum)
    //{
    //    Vector3 returnDicrection = Vector3.zero;
    //    switch (directionEnum)
    //    {
    //        case TrapDirection.Up:
    //            returnDicrection = Vector3.up;
    //            break;
    //        case TrapDirection.Down:
    //            returnDicrection = Vector3.down;
    //            break;
    //        case TrapDirection.Left:
    //            returnDicrection = Vector3.left;
    //            break;
    //        case TrapDirection.Right:
    //            returnDicrection = Vector3.right;
    //            break;
    //    }

    //    return returnDicrection;
    //}

    private void Update()
    {
        Collider[] collider;
        if (overlapEnum == OverlapEnum.Sphere)
        {
            collider = Physics.OverlapSphere(transform.position, _sphereRadius, _playerMask);
            if (collider.Length > 0 && !TrapManager.Instance.isHitThom)
            {
                StartCoroutine(ThomHitPlayer());
            }
        }
        if (overlapEnum == OverlapEnum.Box)
        {
            collider = Physics.OverlapBox(transform.position + new Vector3(0, _boxSize.y / 2, 0), _boxSize / 2, Quaternion.identity, _playerMask);
            if (collider.Length > 0 && !TrapManager.Instance.isHitThom)
            {
                StartCoroutine(ThomHitPlayer());
            }
        }

    }

    private IEnumerator ThomHitPlayer()
    {
        
        TrapManager.Instance.isHitThom = true;
        //플레이어 데미지
        CameraManager.Instance.CameraShake(_amplitude, _frequency, _shakeTime, _easeType);
        GameManager.Instance.player.ApplyDamage(_damage,Vector3.zero,Vector3.zero,0,0,
            null);
        yield return new WaitForSeconds(_coolTime);
        TrapManager.Instance.isHitThom = false;
    }

    private void OnDrawGizmos()
    {
        if(overlapEnum == OverlapEnum.Sphere)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _sphereRadius);
        }
        if (overlapEnum == OverlapEnum.Box)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + new Vector3(0, _boxSize.y / 2, 0), _boxSize);
        }

    }
}
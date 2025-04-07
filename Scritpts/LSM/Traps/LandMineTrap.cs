using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineTrap : MonoBehaviour
{
    [Header("Setting")]
    //[SerializeField] private float _trapDamage;
    [SerializeField] private float _radius;
    [SerializeField] private int _damage;
    [SerializeField] private LayerMask _playerMask;

    [Header("CameraShakeSetting")]
    [SerializeField] private float _amplitude;
    [SerializeField] private float _frequency;
    [SerializeField] private float _time;
    [SerializeField] private Ease _shakeCurve;

    private void Update()
    {
        Collider[] collider =
            Physics.OverlapSphere(transform.position, _radius, _playerMask);
        if (collider.Length > 0 )
        {
            //GameManager.Instance.player.ApplyDamage(_trapDamage,);
            CameraManager.Instance.CameraShake(_amplitude, _frequency, _time, _shakeCurve);
            GameManager.Instance.player.ApplyDamage(_damage, Vector3.zero, Vector3.zero, 0, 0, null);
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

}

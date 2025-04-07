using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float _slowPersent;
    [SerializeField] private Vector3 _radius;
    [SerializeField] private LayerMask _playerMask;

    private bool _isSpiderWeb;
    private float _playerSaveSpeed;

    private void Update()
    {
        Collider[] collider = 
            Physics.OverlapBox(transform.position, _radius/2,Quaternion.identity, _playerMask);
        if(collider.Length > 0 && !_isSpiderWeb)
        {
            if (TrapManager.Instance.isSlowTrap) return;
            TrapManager.Instance.isSlowTrap = true;
            SlowPlayerSpeed();
        }
        else if(collider.Length <= 0 && _isSpiderWeb)
        {
            TrapManager.Instance.isSlowTrap = false; 
            GameManager.Instance.player.runSpeed = _playerSaveSpeed;
            GameManager.Instance.player.AnimatorCompo.speed = 1;
            _isSpiderWeb = false;
        }
    }

    private void SlowPlayerSpeed()
    {
        _isSpiderWeb = true;
        _playerSaveSpeed = GameManager.Instance.player.runSpeed;
        GameManager.Instance.player.runSpeed = _playerSaveSpeed * _slowPersent;
        GameManager.Instance.player.AnimatorCompo.speed = _slowPersent;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, _radius);
    }

}

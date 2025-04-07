using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class WindTornado : PoolLifeTime
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _radius = 1;
    [SerializeField] private LayerMask _whatIsObstacleObj;

    public void SetOwner(Transform owner)
    {
        transform.SetPositionAndRotation(owner.position, owner.rotation);
    }

    private void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + Vector3.up * (_radius * 1.2f), _radius, transform.forward, 
            out hit, _speed * Time.fixedDeltaTime, _whatIsObstacleObj))
        {
            Vector3 newDir = Vector3.ProjectOnPlane(transform.forward, hit.normal);
            movement = newDir * _speed * Time.fixedDeltaTime;
        }
        else
        {
            movement = transform.forward * _speed * Time.fixedDeltaTime;
        }
        transform.position += movement;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * (_radius * 1.2f), _radius);
    }
}

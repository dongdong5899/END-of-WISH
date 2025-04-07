using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraClamp : MonoBehaviour
{
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float clampCollRadius;
    [SerializeField] private float cameraDistaceScrollValue;
    [SerializeField] private float distanceValueChangeSpeed;
    [SerializeField] private LayerMask whatIsObstacle;
    private float currentDistance;
    private float targetDistance;

    private Transform cameraTrm;

    private void Awake()
    {
        cameraTrm = transform.Find("PlayerCam");

        targetDistance = maxDistance;
        currentDistance = targetDistance;
    }

    private void Update()
    {
        targetDistance = Mathf.Clamp(targetDistance - Input.mouseScrollDelta.y * cameraDistaceScrollValue, minDistance, maxDistance);

        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * distanceValueChangeSpeed);
    }

    private void LateUpdate()
    {
        if (Physics.SphereCast(transform.position + transform.forward, clampCollRadius, 
            -transform.forward, out RaycastHit hit, maxDistance, whatIsObstacle) && hit.distance < currentDistance)
        {
            cameraTrm.localPosition = Vector3.back * hit.distance;
        }
        else
        {
            cameraTrm.localPosition = Vector3.back * currentDistance;
        }
    }
}

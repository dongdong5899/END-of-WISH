using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private TrailRenderer _trailRenderer;
    private Coroutine _coroutine;

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * _speed * Time.fixedDeltaTime;
    }

    private void OnEnable()
    {
        _coroutine = GameManager.Instance.DelayFrameCallback(3, () => _trailRenderer.enabled = true);
    }

    private void OnDisable()
    {
        _trailRenderer.enabled = false;
        if (_coroutine != null) GameManager.Instance.StopCoroutine(_coroutine);
    }
}

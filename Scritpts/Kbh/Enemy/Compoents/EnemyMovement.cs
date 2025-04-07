using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum MoveType
{
    Idle,
    Normal,
    Run
}

[System.Serializable]
public class EnemyMovement : IPartialCompoent
{

    public MonoBehaviour Owner { get; set; }
    public Vector3 Velocity
    {
        get
        {
            if (!_navAgent.isStopped)
                return _rigid.velocity;

            else
                return _navAgent.velocity;
        }
    }



    private Rigidbody _rigid;
    private NavMeshAgent _navAgent;
    private Transform _trm;

    [Header("Movement Info")]
    [SerializeField] private float normalSpeed = 3;
    [SerializeField] private float runSpeed = 7;
    [SerializeField] private float currentSpeed = 3;
    [SerializeField] private float stunPercent = 0;


    [Header("Knockback Info")]
    [SerializeField] private float _maxKnockbackVelocity = 2f;
    [SerializeField] private float knockbackDurationTime = 1f;

    private bool _isKnockback = false;
    private float _lastKnockbackTime = 0f;
    private Coroutine _knockbackCoroutine;


    public void SetOwner(MonoBehaviour owner)
    {
        this.Owner = owner;

    }

    public void Initialize(EnemyBrain brain)
    {
        _rigid = brain.RigidbodyCompo;
        _navAgent = brain.NavMeshCompo;
        _trm = Owner.transform;
        SetSpeed(MoveType.Idle);

        _navAgent.enabled = true;
    }

    public void SetDestination(Vector3 endPosition)
    {
        if (_navAgent.enabled)
            _navAgent.SetDestination(endPosition);
    }

    public void GiveKnockback(Vector3 force, float knockbackTime, System.Action callback = null)
    {
        SetActiveNavAent(true);
        StopImmediately();

        if (_knockbackCoroutine is not null)
            Owner.StopCoroutine(_knockbackCoroutine);

        _lastKnockbackTime = Time.time;
        _isKnockback = true;

        SetActiveNavAent(false);

        _knockbackCoroutine
           = Owner.StartCoroutine(GiveKnockbackCoroutine(force, knockbackTime, callback));
    }

    public float GetSpeed() => currentSpeed;
    public void SetSpeed(MoveType moveType)
    {
        if (_isKnockback) return;
        switch (moveType)
        {
            case MoveType.Idle:
                currentSpeed = 0;
                _navAgent.speed = 0;
                _rigid.velocity = _rigid.velocity * Mathf.Epsilon;


                break;
            case MoveType.Normal:
                currentSpeed = normalSpeed;
                _navAgent.speed = normalSpeed;
                _rigid.velocity = _rigid.velocity.normalized * normalSpeed;

                break;

            case MoveType.Run:
                currentSpeed = runSpeed;
                _navAgent.speed = runSpeed;
                _rigid.velocity = _rigid.velocity.normalized * runSpeed;

                break;
        }
    }

    public bool IsCanReached() // Path가 있는가
       => (_navAgent.path.status != NavMeshPathStatus.PathInvalid
       && _navAgent.path.status != NavMeshPathStatus.PathPartial);

    public void SetGravityState(bool isHaveGravity)
         => _rigid.useGravity = isHaveGravity;

    public void SetActiveNavAent(bool activeSelf)
    {
        if (activeSelf)
        {
            _navAgent.Warp(Owner.transform.position);
            _rigid.velocity = Vector3.zero;
            _rigid.angularVelocity = Vector3.zero;
        }

        _navAgent.enabled = activeSelf;
        _rigid.useGravity = !activeSelf;
        _rigid.isKinematic = activeSelf;
    }

    public void StopImmediately()
    {
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;

        if (_navAgent.enabled)
            _navAgent.SetDestination(Owner.transform.position);
    }


    private IEnumerator GiveKnockbackCoroutine(Vector3 force, float knockbackTime, System.Action callback)
    {
        _rigid.velocity += force;

        yield return new WaitUntil(() =>
        {
            return _lastKnockbackTime + knockbackTime < Time.time;
        });

        // Complete
        SetActiveNavAent(true);
        StopImmediately();
        callback?.Invoke();
        _isKnockback = false;
    }

}

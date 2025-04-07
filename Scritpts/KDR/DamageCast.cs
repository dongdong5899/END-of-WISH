using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct ActiveTime
{
    public float time;
    public int damage;
    public Vector3 knockBack;
    public float knockBackTime;
    public Vector3 cameraShake;
    [HideInInspector] public bool isActived;
}

[Serializable]
public struct DamageAreaMove
{
    public Vector3 movePosition;
    public Vector3 moveRotation;
    public float startTime;
    public float duration;
    public AnimationCurve animationCurve;
    [HideInInspector] public bool isStarted;
}

public class DamageCast : MonoBehaviour
{
    [SerializeField] private Vector3 offSet;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float radius;
    [SerializeField] private Vector3 rectSize;
    [SerializeField] private LayerMask whatIsEnemy;

    [SerializeField] private DamageAreaMove moveInfo;

    [SerializeField] private bool useSphereCast;
    [SerializeField] private bool useBoxCast;

    [SerializeField] private UnityEvent<RaycastHit> hitEvent;

    [SerializeField] private string[] hitEffects;

    public ActiveTime[] activeTimes;

    new private ParticleSystem particleSystem;
    private float maxScale = 1;

    private Vector3 _startPos;

    protected virtual void OnEnable()
    {
        for (int i = 0; i < activeTimes.Length; i++)
        {
            activeTimes[i].isActived = false;
        }
        moveInfo.isStarted = false;

        GameManager.Instance.DelayFrameCallback(1, () =>
        {
            maxScale = Mathf.Max(Mathf.Max(transform.localScale.x, transform.localScale.y), transform.localScale.z);
        });
    }

    private void Awake()
    {
        particleSystem = GetComponentInParent<ParticleSystem>();
    }
    Sequence seq;
    protected virtual void Update()
    {
        for (int i = 0; i < activeTimes.Length; i++)
        {
            ActiveTime activeTime = activeTimes[i];
            if (activeTime.isActived == false && activeTime.time <= particleSystem.time)
            {
                OnDamage(activeTime);
                activeTimes[i].isActived = true;
            }
        }

        if (moveInfo.isStarted == false && moveInfo.startTime <= particleSystem.time)
        {
            moveInfo.isStarted = true;
            if (seq != null && seq.IsActive()) seq.Kill();


            _startPos = transform.localPosition;
            seq = DOTween.Sequence()
                .Append(transform.DOLocalMove(moveInfo.movePosition, moveInfo.duration).SetEase(moveInfo.animationCurve))
                .Join(transform.DOLocalRotate(moveInfo.moveRotation, moveInfo.duration).SetEase(moveInfo.animationCurve));
        }

    }

    public List<RaycastHit> DamageableCast()
    {
        Vector3 localDirection = transform.rotation * direction * maxScale;

        RaycastHit[] exceptSphereHits = Physics.SphereCastAll(
            transform.position + transform.rotation * offSet - localDirection,
            radius * maxScale,
            Vector3.up,
            0,
            whatIsEnemy);
        RaycastHit[] sphereHits = Physics.SphereCastAll(
            transform.position + transform.rotation * offSet - localDirection,
            radius * maxScale,
            localDirection.normalized,
            localDirection.magnitude,
            whatIsEnemy);
        RaycastHit[] rectHits = Physics.BoxCastAll(
            transform.position + transform.rotation * offSet,
            new Vector3(
                transform.localScale.x * rectSize.x,
                transform.localScale.y * rectSize.y,
                transform.localScale.z * rectSize.z) / 2,
            Vector3.up,
            transform.rotation,
            0,
            whatIsEnemy);

        List<RaycastHit> resoultHits = new List<RaycastHit>();

        foreach (RaycastHit hit in useSphereCast ? sphereHits : rectHits)
        {
            if (useSphereCast && Array.Exists(exceptSphereHits, i => i.transform.Equals(hit.transform))) continue;

            if (Array.Exists(useBoxCast ? rectHits : sphereHits, i => i.transform.Equals(hit.transform)) && hit.transform.GetComponent<IHealth>() != null)
            {
                /*ApplyDamage(hit, damage, knockbackPower, cameraShake);*/
                resoultHits.Add(hit);
            }
        }
        return resoultHits;
    }

    public void ApplyDamage(RaycastHit hit, int damage, Vector3 knockback, float knockBackTime, Vector3 cameraShake)
    {
        Quaternion dir = Quaternion.LookRotation(hit.transform.position - transform.position);
        Vector3 knockBack = dir * knockback;


        if (hit.transform.TryGetComponent(out IHealth health))
        {
            foreach (string prefabName in hitEffects)
            {
                Poolable hitEffect = PoolManager.Instance.Pop(prefabName);
                hitEffect.transform.position = hit.transform.position + Vector3.up;
                hitEffect.transform.localScale = Vector3.one * 1.5f;
            }
            CameraManager.Instance.CameraShake(cameraShake.x, cameraShake.y, cameraShake.z);
            health.ApplyDamage(
                damage,
                hit.point,
                knockBack.normalized,
                knockBack.magnitude,
                knockBackTime,
                this);
            hitEvent?.Invoke(hit);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + offSet, transform.position + offSet + transform.rotation * moveInfo.movePosition);

        if (useSphereCast)
        {
#if UNITY_EDITOR
            maxScale = Mathf.Max(Mathf.Max(transform.localScale.x, transform.localScale.y), transform.localScale.z);
            _startPos = transform.localPosition;
#endif

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + offSet, radius * maxScale);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + offSet - transform.rotation * direction * maxScale, radius * maxScale);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + offSet + transform.rotation * moveInfo.movePosition, radius * maxScale);
        }
        if (useBoxCast)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(offSet, rectSize);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(moveInfo.movePosition + offSet + (_startPos - transform.localPosition), rectSize);
        }
    }

    public virtual void OnDamage(ActiveTime activeTime) { }
}

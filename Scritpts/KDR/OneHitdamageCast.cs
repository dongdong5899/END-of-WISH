using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class OneHitdamageCast : DamageCast
{
    [SerializeField] private bool isOneFrameHit;
    [SerializeField] private float duration;

    private bool onDamage;

    private ActiveTime whatDamage;
    private List<RaycastHit> hitedObj = new List<RaycastHit>();

    protected override void OnEnable()
    {
        base.OnEnable();

        onDamage = false;

        hitedObj.Clear();

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    protected override void Update()
    {
        base.Update();

        if (onDamage)
        {
            List<RaycastHit> hits = DamageableCast();
            foreach (RaycastHit item in hits)
            {
                if (hitedObj.Any(x => x.transform == item.transform)) 
                    continue;

                ApplyDamage(item, whatDamage.damage, whatDamage.knockBack, whatDamage.knockBackTime, whatDamage.cameraShake);
                hitedObj.Add(item);
            }
        }
    }

    public override void OnDamage(ActiveTime activeTime)
    {
        base.OnDamage(activeTime);
        if (isOneFrameHit)
        {
            List<RaycastHit> hits = DamageableCast();
            foreach (RaycastHit item in hits)
            {
                ApplyDamage(item, activeTime.damage, activeTime.knockBack, activeTime.knockBackTime, activeTime.cameraShake);
            }
        }
        else
        {
            onDamage = true;
            GameManager.Instance.DelayCallback(new WaitForSeconds(duration), () => onDamage = false);
            whatDamage = activeTime;
        }
    }
}

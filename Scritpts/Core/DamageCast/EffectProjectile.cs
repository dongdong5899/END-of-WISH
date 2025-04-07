using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectProjectile : Poolable, IDamage
{
   public float lifeTime;

   [field : SerializeField] public int Damage { get; set; }
   [field : SerializeField] public LayerMask WhatIsEnemy { get; set; }
   [field : SerializeField] public float knockbackPower { get; set; }

   public override void Initialize()
   {
      // 여기서 이펙트 관련해서 초기화할꺼하고,
      // 콜라이더 움직일 것이면 여기서 하고
   }


   public virtual void ApplyCast(RaycastHit hit)
   {
      if (!hit.transform) return;

      if (hit.transform.TryGetComponent(out IHealth health))
      {
         if (this.CanSendDamage(health))
         {
            //health.ApplyDamage(Damage, hit.point, hit.normal, knockbackPower, this);
         }
      }
   }

}

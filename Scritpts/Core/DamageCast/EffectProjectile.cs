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
      // ���⼭ ����Ʈ �����ؼ� �ʱ�ȭ�Ҳ��ϰ�,
      // �ݶ��̴� ������ ���̸� ���⼭ �ϰ�
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

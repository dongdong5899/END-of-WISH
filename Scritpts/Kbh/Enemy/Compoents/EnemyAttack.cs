using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemyAttack : IPartialCompoent
{
   public MonoBehaviour Owner { get; set; }

   public void SetOwner(MonoBehaviour owner)
   {
      this.Owner = owner;
   }

   public float attackSpeed = 3f;
   public float playerDetectRadius;
   public float playerAttackRadius;
   public LayerMask whatIsEnemy;
   public float lastAttackTime;
   public float attackCoolTime = 1.5f;

   public Transform attackTrm;
   public Vector3 rotateScale;


   public bool IsPlayerNear()
      => Vector3.Distance(Owner.transform.position
         , GameManager.Instance.player.transform.position) <= playerDetectRadius;

   public bool CanAttack()
      => IsPlayerInAttackRange() && IsAttackCoolFill();

   public bool IsPlayerInAttackRange()
      => (Vector3.Distance(Owner.transform.position
         , GameManager.Instance.player.transform.position) <= playerAttackRadius);

   public bool IsAttackCoolFill()
      => (Time.time >= lastAttackTime + attackCoolTime);




#if UNITY_EDITOR
   public void OnDrawGizmo()
   {
      Vector3 position = Owner is not null
         ? Owner.transform.position : Vector3.zero;

      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(position, playerAttackRadius);

      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(position, playerDetectRadius);

      Gizmos.color = Color.white;
   }
#endif

}

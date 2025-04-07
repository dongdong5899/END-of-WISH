using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
   [SerializeField] private EnemyBrain enemyBrain;

   private void AnimationEndTrigger()
   {
      enemyBrain.AnimationEndTrigger();
   }

   private void AttackTrigger()
   {
      enemyBrain.AttackTrigger();
   }

   

}

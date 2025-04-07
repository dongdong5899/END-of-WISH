using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PunchEnemyBrain : EnemyBrain
{
   public int currentPunchCounter = 0;
   public int punchMaxCnt = 2;

   public override void AnimationEndTrigger()
   {
      base.AnimationEndTrigger();
      AnimatorCompo.SetFloat("PunchCounter"
         , currentPunchCounter);
   }

   public override void AttackTrigger()
   {
      base.AttackTrigger();

      ++currentPunchCounter;
      currentPunchCounter %= punchMaxCnt;
   }

}

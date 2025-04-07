
using UnityEngine;


public interface IDamage
{
   int Damage { get; set; }
   float knockbackPower { get; set; }
   LayerMask WhatIsEnemy { get; set; }
}

public static class DamageableHelper
{
   public static bool CanSendDamage(this IDamage damage, IHealth target)
   {
      return 
         (target.WhatIsMe.value & damage.WhatIsEnemy.value) > 0
            &&
         (target.Hp > 0);
   }
}
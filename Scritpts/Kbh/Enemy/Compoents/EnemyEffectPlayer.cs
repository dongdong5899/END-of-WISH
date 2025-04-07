using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyEffectPlayer : IPartialCompoent
{
   public MonoBehaviour Owner { get; set;  }

   [SerializeField] private float freezeTime;
   [SerializeField] private float stunScale;

   public void SetOwner(MonoBehaviour owner)
   {
      this.Owner = owner;
   }
}

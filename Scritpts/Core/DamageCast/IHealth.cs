
using UnityEngine;

public interface IHealth 
{
   int Hp { get; set; }
   LayerMask WhatIsMe { get; set; }
   void ApplyDamage(int damage, Vector3 hitPoint, Vector3 normal, float knockbackPower, float knockBackTime, MonoBehaviour dealer);
}

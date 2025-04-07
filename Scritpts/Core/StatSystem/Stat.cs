using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
   [SerializeField] private int _defaultValue = 0;
   private int valueModifierSum = 0;

   [Range(0, 1)] [SerializeField] private float _percent = 1;
   private float percentModifierSum = 0;

   public int Value
   {
      get
      {
         return 
            Mathf.CeilToInt
            (Mathf.Clamp((_defaultValue + valueModifierSum) * _percent, 0, _defaultValue));
      }

      set
      {
         valueModifierSum = value - _defaultValue;
      }
   }

   public float Percent
   {
      get => Mathf.Clamp(_percent, 0, 1);

      set
      {
         percentModifierSum = value - _percent;
      }
   }
   
   public Stat(int defaultValue)
   {
      _defaultValue = defaultValue;
   }

   public void AddValue(int modifier)
   {
      valueModifierSum += modifier;
   }

   public void DecreaseValue(int modifier)
   {
      valueModifierSum -= modifier;
   }

   public void AddPercent(float modifier)
   {
      percentModifierSum += modifier;
   }

   public void RemovePercent(float modifier)
   {
      percentModifierSum -= modifier;
   }

   public void SetValueDefault()
   {
      valueModifierSum = 0;
   }

   public void SetPercentDefault()
   {
      _percent = 1;
   }

   public void ApplyValue()
   {
      _defaultValue = Value; // 현재 계산된 값을 원래 것에 저장하고
      // 나머지를 밀어준다. 
      _percent = 1;
      valueModifierSum = 0;
      percentModifierSum = 0;
   }

}

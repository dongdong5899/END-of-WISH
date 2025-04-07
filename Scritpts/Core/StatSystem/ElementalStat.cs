using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementalType : long
{
   Fire = 2,
   Water = 3,

   Evaporation = 6, // ΑυΉί
}

[Serializable]
public struct RegisteElementStamp
{
   public ElementalType elementType;
   public float time;
   public int changeValue;
}

public struct ElementInfo
{
   public int value;
   public float endTime;
}


[System.Serializable]
public class ElementalStat : IPartialCompoent
{
   private ElementalType _currentElement;
   public ElementalType CurrentElement => _currentElement;

   public MonoBehaviour Owner { get; set; }
   public ElementalType[] elementalTypeList;

   private Dictionary<ElementalType, ElementInfo> _elementInfoTimeDictionary;

   public Dictionary<ElementalType, Action<int>> OnElementTypeEnter;
   public Dictionary<ElementalType, Action<int>> OnElementTypeDuring;
   public Dictionary<ElementalType, Action> OnElementTypeExit;
   public event Action OnElementChange;



   public void SetOwner(MonoBehaviour owner)
   {
      this.Owner = owner;
      owner.StartCoroutine(ElementTypeUpdate());
      elementalTypeList = Enum.GetValues(typeof(ElementalType)) as ElementalType[];
   }

   private IEnumerator ElementTypeUpdate()
   {
      while (true)
      {

         for(int i = 0; i<_elementInfoTimeDictionary.Count; ++i)
         {
            ElementalType currentType = elementalTypeList[i];


            if (_elementInfoTimeDictionary.TryGetValue(currentType, out ElementInfo info))
            {
               if(info.endTime >= Time.time)
               {
                  _elementInfoTimeDictionary.Remove(currentType);
                  
                  if(OnElementTypeExit.TryGetValue(currentType, out var evt))
                  {
                     evt?.Invoke();
                  }
               }
               else
               {
                  if (OnElementTypeDuring.TryGetValue(currentType, out var evt))
                  {
                     evt?.Invoke(info.value);
                  }
               }
            }
         }

         yield return null;
      }
   }

   public void GiveElement(ElementalType element, float time, int newValue)
   {
      ElementInfo elementInfo = _elementInfoTimeDictionary[element];

      float newTime = Time.time + time;

      if (newTime > elementInfo.endTime)
         elementInfo.endTime = newTime;

      if (newValue > elementInfo.value)
         elementInfo.value = newValue;

      _elementInfoTimeDictionary[element] = elementInfo;

      if (OnElementTypeEnter.TryGetValue(element, out var evt))
      {
         evt?.Invoke(newValue);
      }

      if (((int)_currentElement / (int)element) > 0)
      {
         _currentElement
            = (ElementalType)((int)element / BestEstimate((int)element, (int)_currentElement));
      }

   }

   private int BestEstimate(int a, int b)
   {
      if (a < 0 || b < 0) return 1;

      int tmp;
      while(b != 0)
      {
         tmp = a % b;
         a = b;
         b = tmp;
      }
      return a;
   }
}

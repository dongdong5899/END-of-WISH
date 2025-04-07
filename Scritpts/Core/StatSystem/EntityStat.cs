using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;


[Serializable]
public struct RegisteValueIncrease
{
   public Stat targetStat;
   public float time;
   public int changeValue;
}


[Serializable]
public struct RegistePercentIncrease
{
   public Stat targetStat;
   public float time;
   public float changePercent;
}


[CreateAssetMenu(menuName = "SO/Stat")]
public class EntityStat : ScriptableObject, IPartialCompoent
{

   public Stat damage;
   public Stat maxHealth;
   public Stat criticalChance;
   public Stat criticalDamage;
   public Stat armor;

   protected Dictionary<StatType, Stat> _statDictionary;
   
   protected List<RegisteValueIncrease> registeValueModify
      = new List<RegisteValueIncrease>();

   protected List<RegistePercentIncrease> registePercentModify
       = new List<RegistePercentIncrease>();

   public MonoBehaviour Owner { get; set; }
   protected Coroutine _statusUpdateRoutine;


   public void SetOwner(MonoBehaviour owner)
   {
      this.Owner = owner;
      _statusUpdateRoutine = owner.StartCoroutine(StatusUpdate());
   }

   protected virtual void OnEnable()
   {
      _statDictionary = new Dictionary<StatType, Stat>();

      Type agentStatType = typeof(EntityStat);
      foreach (StatType enumType in Enum.GetValues(typeof(StatType)))
      {
         try
         {
            string fieldname = LowerFirstChar(enumType.ToString());

            FieldInfo statField = agentStatType.GetField(fieldname);
            _statDictionary.Add(enumType, statField.GetValue(this) as Stat);
         }
         catch (Exception ex)
         {
            Debug.LogError($"There are no stat - {enumType.ToString()}, {ex.Message}");
         }
      }
   }
   private IEnumerator StatusUpdate()
   {
      while (true)
      {
         for(int i = 0; i<registeValueModify.Count; ++i)
         {
            if(registeValueModify[i].time > Time.time)
            {
               registeValueModify[i].targetStat.AddValue
                  (registeValueModify[i].changeValue);

               registeValueModify.RemoveAt(i);
            }
         }
      
         for(int i = 0; i<registePercentModify.Count; ++i)
         {
            if (registePercentModify[i].time > Time.time)
            {
               registePercentModify[i].targetStat.AddPercent
                  (registePercentModify[i].changePercent);

               registePercentModify.RemoveAt(i);
            }
         }

         yield return null;

      }
   }
   public void StopStatusUpdate()
   {
      Owner.StopCoroutine(_statusUpdateRoutine);
   }
   


   public void IncreaseValue(int modifyValue, float duration, Stat statToModifiy)
   {
      statToModifiy.AddValue(modifyValue);
      var recoverInfo
         = new RegisteValueIncrease
         {
            changeValue = -modifyValue,
            time = Time.time + duration,
         };

      registeValueModify.Add(recoverInfo);
   }
   public void IncreasePercent(float modifyPercent, float duration, Stat statToModifiy)
   {
      statToModifiy.AddPercent(modifyPercent);

      var recoverInfo
         = new RegistePercentIncrease
         {
            changePercent = -modifyPercent,
            time = Time.time + duration
         };

      registePercentModify.Add(recoverInfo);
   }


   public void ApplyModifier(params Stat[] statToApply)
   {
      foreach(var targetStat in statToApply)
         targetStat.ApplyValue();
   }
   

   private string LowerFirstChar(string input)
      => $"{char.ToLower(input[0])}{input.Substring(1)}";

}

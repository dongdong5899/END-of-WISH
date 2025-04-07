using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityListGoal
{
   None,
   Install,
   Dispose
}

[Serializable]
public class EntityList 
{
   private EntityListGoal _goal;
   public EntityListGoal Goal => _goal;

   public int capacity;
   private Dictionary<object, EntityStruct> _entityInfos;


   public EntityList(int capacity, EntityListGoal goal)
   {
      this.capacity = capacity;
      _goal = goal;
      _entityInfos = new Dictionary<object, EntityStruct>(this.capacity);
   }


   public void PushAt(object key, EntityStruct entityInfo)
   {
      if (_goal == EntityListGoal.None) return;

      if (_goal == EntityListGoal.Install)
         entityInfo.Initialize();
      if (_goal == EntityListGoal.Dispose)
         entityInfo.Dispose();

      _entityInfos[key] = entityInfo;
   }


   public EntityStruct Erase(object key)
   {
      if (_goal == EntityListGoal.None) return default;

      EntityStruct inp = _entityInfos[key];

      _entityInfos.Remove(key);
      return inp;
   }

}

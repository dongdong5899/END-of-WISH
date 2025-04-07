using System;
using UnityEngine;


public interface IEntityData : IDisposable
{
   public bool IsConnected { get; set; }

   public IEntity entity { get; set; }

   public object reference => entity;
}

public interface IEntity : IDisposable
{
   public void Initialize(ref EntityStruct input);
   public EntityRequest UpdateObject(ref EntityStruct input);
}

[Serializable]
public struct EntityRequest
{
   
}

[Serializable]
public struct EntityStruct : IEntityData
{

   [field: SerializeField] public bool IsConnected { get; set; }

   [field:SerializeField] public IEntity entity { get; set; }


   /* Transform */
   public Vector3 position;
   public Quaternion rotation;
   public Vector3 scale;

   public EntityStruct(IEntity entity)
   {
      IsConnected = true;
      this.entity = entity;

      position = Vector3.zero;
      rotation = Quaternion.identity;
      scale = Vector3.one; 
   }

   public void Initialize()
   {
      if (!IsConnected) return;
      entity.Initialize(ref this);
   }

   public EntityRequest Update()
   {
      if (!IsConnected) return default;
      return entity.UpdateObject(ref this);
   }


   public void Dispose()
      => entity.Dispose();

}
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public enum EntityEnum
//{
//   None,
//   Player,
//   Enemy,
//}

//[System.Serializable]
//public struct EntityPoolStruct
//{
//   public EntityEnum entityType;
//   public int cnt;
//}

//public class EntityManager : MonoSingleton<EntityManager>
//{
//   [HideInInspector] public int _hashCounter = 0;
//   private Dictionary<EntityEnum, Type> _entityDictionary;

//   private int _maxEntityNum = 150;
//   private List<IEntity> _entities;

//   private Dictionary<EntityEnum, Stack<IEntity>> entityPool;
//   [SerializeField] private List<EntityPoolStruct> entityPoolStruct;
//   [SerializeField] private int addingNumberAboutPoolMember;

//   private void Awake()
//   {
//      _entityDictionary = new Dictionary<EntityEnum, Type>();
//      _entities = new List<IEntity>(_maxEntityNum);

//      foreach(EntityEnum entityType in Enum.GetValues(typeof(EntityEnum)))
//      {
//         string typeName = UpperFirstChar($"{entityType.ToString()}");
//         Type t = Type.GetType(typeName);

//         _entityDictionary.Add(entityType, t);
//      }

//      entityPool = new Dictionary<EntityEnum, Stack<IEntity>>();
//      foreach(var poolInfo in entityPoolStruct)
//      {
//         if (!_entityDictionary.TryGetValue(poolInfo.entityType, out Type t)) // 만드는 것에 실패한 타입이라면 
//             continue;

//         entityPool[poolInfo.entityType] = new Stack<IEntity>(poolInfo.cnt);

//         for(int i = 0; i < poolInfo.cnt; i++)
//         {
//            entityPool[poolInfo.entityType].Push(NewEntity(poolInfo.entityType));
//         }
//      }

//   }

//   public IEntity Pop(EntityEnum entityEnum)
//   {
//      if(entityPool.TryGetValue(entityEnum, out Stack<IEntity> result))
//      {
//         if(result.Count > 0)
//            return result.Pop();
//         else
//         {
//            for(int i = 0; i<addingNumberAboutPoolMember-1; ++i)
//            {
//               result.Push(NewEntity(entityEnum));
//            }
//            return NewEntity(entityEnum);
//         }
//      }
//      return NewEntity(entityEnum);
//   }

//   public void Push(IEntity entity, EntityEnum entityEnum)
//   {
//      if (entityPool.TryGetValue(entityEnum, out Stack<IEntity> result))
//      {
//         result.Push(entity);
//      }
//      else
//      {
//         Debug.LogWarning($"warning! : {entityEnum.ToString()} Type Can't be pushed because, it doesn't have a pool. ");
//      }
//   }

//   private IEntity NewEntity(EntityEnum entityEnum)
//   {
//      IEntity entity = Activator.CreateInstance(_entityDictionary[entityEnum]
//         , _hashCounter++, entityEnum) as IEntity;
//      _entities.Add(entity);
//      return entity;
//   }
   
//   private string UpperFirstChar(string inp)
//   {
//      return $"{char.ToUpper(inp[0]).ToString()}{inp.Substring(1)}";
//   }

//   public IEntity GetEntityByHash(int hash)
//   {
//      if(_entities.Count > hash)
//      {
//         return _entities[hash];
//      }
//      return null;
//   }

//}



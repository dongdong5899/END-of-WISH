using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyEnum
{
   Idle = 0,
   Attack = 1,
   Walk = 2,
   Run = 3,
   Die = 4
}

public class EnemyStateMachine : IPartialCompoent, IDisposable
{
   private Dictionary<EnemyEnum, EnemyState> stateDictionary;
   public Animator animator;
   private EnemyState currentState;

   public MonoBehaviour Owner { get; set; }

   public void SetOwner(MonoBehaviour owner)
   {
      Owner = owner;
      animator = (owner as EnemyBrain).AnimatorCompo;
   }

   public void Initialize(EnemyEnum defaultState, Func<string, string> typeGetter)
   {
      stateDictionary = new Dictionary<EnemyEnum, EnemyState>();

      foreach (EnemyEnum enumElement in Enum.GetValues(typeof(EnemyEnum)))
      {
            try
            {
                string enumElementStr = enumElement.ToString();
                string typeName = typeGetter?.Invoke(enumElementStr);
                Type t = Type.GetType(typeName);

                stateDictionary.Add(enumElement,
                   Activator.CreateInstance(t, Owner, this, enumElement, enumElementStr)
                      as EnemyState);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

      ChangeState(defaultState);
   }


   public void ChangeState(EnemyEnum changingState)
   {
      currentState?.Exit();
      currentState = stateDictionary[changingState];
      currentState?.Enter();
   }


   public void Update()
   {
      currentState?.UpdateState();
   }

   public void Dispose()
   {
      currentState = null;
      stateDictionary.Clear();
   }

   public EntityRequest Update(ref EntityStruct input)
   {
      return new EntityRequest();
   }

   public void AnimationTrigger()
   {
      currentState?.AnimationEndTrigger();
   }
   
}

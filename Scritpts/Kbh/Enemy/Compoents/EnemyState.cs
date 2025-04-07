using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyState
{
   protected EnemyBrain _brain;
   protected EnemyStateMachine _stateController;
   protected EnemyEnum _enumElement;

   protected int _animHash;
   protected bool _endTrigger = false;

   public EnemyState(EnemyBrain brain, EnemyStateMachine stateMachine, EnemyEnum enumElement, string stateName)
   {
      _brain = brain; 
      _stateController = stateMachine;
      _enumElement = enumElement;
      _animHash = Animator.StringToHash(stateName);

      SettingNew();
   }

   public virtual void SettingNew() {}

   public virtual void Enter()
   {
      _brain.AnimatorCompo.SetBool(_animHash, true);
   }

   public virtual void UpdateState()
   {

   }

   public virtual void Exit()
   {
      _brain.AnimatorCompo.SetBool(_animHash, false);
      _endTrigger = false;
   }

   public virtual void AnimationEndTrigger()
   {
      _endTrigger = true;
   }

}
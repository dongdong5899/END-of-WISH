using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
   public EnemyAttackState(EnemyBrain brain, EnemyStateMachine stateMachine, EnemyEnum enumElement, string stateName) : base(brain, stateMachine, enumElement, stateName)
   {
   }

   public override void SettingNew()
   {
      Type t = _brain.GetType();
      string attackType = t.ToString().Replace("EnemyBrain", "");
      _animHash = Animator.StringToHash(attackType);
   }

   public override void Enter()
   {
      base.Enter();
      _brain.transform.forward
         = (GameManager.Instance.player.transform.position - _brain.transform.position);

      _brain.AnimatorCompo.speed = _brain.attack.attackSpeed;
      _brain.movement.StopImmediately();
   }


   public override void UpdateState()
   {

      if (_endTrigger)
      {
         _stateController.ChangeState(EnemyEnum.Idle);
         return;
      }

      base.UpdateState();
   }

   public override void Exit()
   {
      _brain.AnimatorCompo.speed = 1;
      base.Exit();
   }

}

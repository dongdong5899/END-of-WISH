using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
   public EnemyIdleState(EnemyBrain brain, EnemyStateMachine stateMachine, EnemyEnum enumElement, string stateName) : base(brain, stateMachine, enumElement, stateName)
   {
   }

   public float timeStamp;
   public float delay;
   public float randomDelayMin = 5f;
   public float randomDelayMax = 7f;

   public override void Enter()
   {
      base.Enter();

      timeStamp = Time.time;
      _brain.movement.SetDestination(_brain.transform.position);
      delay = Random.Range(randomDelayMin, randomDelayMax);
   }

   public override void UpdateState()
   {
      if (_brain.attack.IsPlayerInAttackRange()
         && _brain.attack.IsAttackCoolFill())
      {
         _stateController.ChangeState(EnemyEnum.Attack);
         return;
      }

      if (!_brain.attack.IsPlayerInAttackRange()
         && _brain.attack.IsPlayerNear())
      {
         _stateController.ChangeState(EnemyEnum.Run);
         return;
      }
      
      if ((Time.time > timeStamp + delay))
      {
         _stateController.ChangeState(EnemyEnum.Walk);
         return;
      }




   }
}

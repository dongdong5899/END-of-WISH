using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : EnemyState
{
   public EnemyWalkState(EnemyBrain brain, EnemyStateMachine stateMachine, EnemyEnum enumElement, string stateName) : base(brain, stateMachine, enumElement, stateName)
   {
   }

   public float distance;

   public float distanceDelayMin = 1f;
   public float distanceDelayMax = 3f;

   public Vector3 endPosition;


   public override void Enter()
   {
      base.Enter();

      Vector3 currentPosition = _brain.transform.position;
      Vector3 randomMoveDirection = Random.insideUnitSphere.normalized;
      randomMoveDirection.y = 0;

      if (randomMoveDirection == Vector3.zero)
         randomMoveDirection = Vector3.right;

      _brain.movement.SetSpeed(MoveType.Normal);

      distance = Random.Range(distanceDelayMin, distanceDelayMax);

      endPosition = currentPosition
         + randomMoveDirection * distance;

      _brain.movement.SetDestination(endPosition);
   }

   public override void UpdateState()
   {

      if (_brain.attack.IsPlayerInAttackRange())
      {
         if (_brain.attack.IsAttackCoolFill())
            _stateController.ChangeState(EnemyEnum.Attack);
         else
            _stateController.ChangeState(EnemyEnum.Idle);

         return;
      }

      if (!_brain.attack.IsPlayerInAttackRange()
         && _brain.attack.IsPlayerNear())
      {
         _stateController.ChangeState(EnemyEnum.Run);
         return;
      }



      bool isArrived = Vector3.Distance(_brain.transform.position, endPosition)
         < _brain.NavMeshCompo.stoppingDistance + 0.1f;

      if (isArrived || !_brain.movement.IsCanReached())
      {
         _stateController.ChangeState(EnemyEnum.Idle);
         return;
      }

   }

   public override void Exit()
   {
      _brain.movement.SetSpeed(MoveType.Idle);
      base.Exit();
   }

}

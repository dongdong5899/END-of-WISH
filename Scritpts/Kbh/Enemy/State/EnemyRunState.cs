using UnityEngine;

public class EnemyRunState : EnemyState
{
   public EnemyRunState(EnemyBrain brain, EnemyStateMachine stateMachine, EnemyEnum enumElement, string stateName) : base(brain, stateMachine, enumElement, stateName)
   {
   }

   
   public override void Enter()
   {
      base.Enter();
      _brain.movement.SetSpeed(MoveType.Run);
   }


   public override void UpdateState()
   {
      Vector3 playerPosition = _brain.gameManager.player.transform.position;
      _brain.movement.SetDestination(playerPosition);

      
      if (_brain.attack.IsPlayerInAttackRange())
      {
         if (_brain.attack.IsAttackCoolFill())
            _stateController.ChangeState(EnemyEnum.Attack);
         else
            _stateController.ChangeState(EnemyEnum.Idle);

         return;
      }

      if (!_brain.attack.IsPlayerNear())
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

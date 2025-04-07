using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : EnemyState
{
   public EnemyDieState(EnemyBrain brain, EnemyStateMachine stateMachine, EnemyEnum enumElement, string stateName) : base(brain, stateMachine, enumElement, stateName)
   {
   }

   public override void Enter()
   {
      base.Enter();
      _brain.Die();

      //임시 점수 시스템
      GameManager.Instance.AddScore();
   }

}

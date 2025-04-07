using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDefaultAttackState : PlayerState
{
    private int comboCount;
    private float lastAttackTime;
    private float attackWindow = 0.4f;
    private int comboCounterHash = Animator.StringToHash("AttackCount");

    private ChipSO currentChipSO;

    private Coroutine effectCoroutine;
    private Coroutine moveCoroutine;


    private float moveStartTime;
    private float moveTime = 0.07f;



    public PlayerDefaultAttackState(PlayerBrain player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(currentChipSO != GameManager.Instance.currentChip)
        {
            comboCount = 0;
            currentChipSO = GameManager.Instance.currentChip;
        }

        player.LookEnemy();

        int maxCombo = currentChipSO == null ? 3 : currentChipSO.defaultAttackFrefabName.Length;
        float currntTime = Time.time;
        bool timeOver = currntTime > lastAttackTime + attackWindow;
        bool comboOver = comboCount >= maxCombo;
        if (timeOver || comboOver)
            comboCount = 0;

        player.AnimatorCompo.speed = player.attackSpeed;
        player.currentComboCount = comboCount;
        player.AnimatorCompo.SetInteger(comboCounterHash, comboCount);


        if (player.CanMoveForTarget())
        {
            moveStartTime = Time.time;

            Vector3 movement = currentChipSO.actionMove[comboCount];
            movement.y = 0;
            player.MoveCompo.SetMovement(player.visualTrm.rotation * movement, false);

            moveCoroutine = GameManager.Instance.DelayCallback
                (new WaitUntil(() => player.CanMoveForTarget() == false || moveStartTime + moveTime < Time.time), player.MoveCompo.MoveStop);
        }
        else
        {
            player.MoveCompo.MoveStop();
        }


        player.InputCompo.MovementEvent += HandleMovementEvent;
        effectCoroutine = GameManager.Instance.DelayFrameCallback(1, () => player.StartEffectEvent += HandleStartEffectEvent);
    }

    private void HandleMovementEvent(Vector3 vector)
    {
        if (vector != Vector3.zero && player.CanNextAnimation)
        {
            stateMachine.StateChange(PlayerStateEnum.Run);
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (animEndTriggerCalled)
        {
            stateMachine.StateChange(PlayerStateEnum.Idle);
        }
    }

    public override void Exit()
    {
        base.Exit();

        lastAttackTime = Time.time;
        ++comboCount;
        player.AnimatorCompo.speed = 1;
        player.InputCompo.MovementEvent -= HandleMovementEvent;
        GameManager.Instance.StopDelayCallback(moveCoroutine);
        player.MoveCompo.MoveStop();
        GameManager.Instance.StopDelayCallback(effectCoroutine);
        player.StartEffectEvent = null;
    }

    private void HandleStartEffectEvent()
    {
        if (currentChipSO == null)
        {

        }
        else if (string.IsNullOrEmpty(currentChipSO.defaultAttackFrefabName[comboCount]) == false)
        {
            Poolable obj = PoolManager.Instance.Pop(currentChipSO.defaultAttackFrefabName[comboCount]);
            obj.transform.SetPositionAndRotation(
                player.transform.position + player.visualTrm.rotation * currentChipSO.prefabTrmSetting[comboCount].position,
                player.visualTrm.rotation * Quaternion.Euler(currentChipSO.prefabTrmSetting[comboCount].rotation));
            obj.transform.localScale = currentChipSO.prefabTrmSetting[comboCount].scale;
            obj.GetComponentInChildren<DamageCast>().activeTimes[0].damage = currentChipSO.damage[comboCount];
        }

        CameraManager.Instance.CameraShake(
            currentChipSO.cameraShakeValue[comboCount].x,
            currentChipSO.cameraShakeValue[comboCount].y,
            currentChipSO.cameraShakeValue[comboCount].z
            );
    }

    public override void AnimationFinishTrigger()
    {
        animEndTriggerCalled = true;
    }
}

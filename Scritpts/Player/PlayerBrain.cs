using System;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerStateEnum
{
    Idle,
    Walk,
    Run,
    Dead,
    DefaultAttack,
    QSkill,
    ESkill,
}


public class PlayerBrain : MonoBehaviour, IHealth
{
    public float walkSpeed = 2;
    public float runSpeed = 15;

    public float attackSpeed = 1.2f;
    public int currentComboCount = 0;

    /*[SerializeField]
    private float damage = 2;*/

    public bool CanStateChangeable = true;
    public bool CanNextAnimation = true;
    public bool OnSkill = false;
    public bool IsRuning = true;

    public Action StartEffectEvent;

    [HideInInspector] public PlayerInput InputCompo { get; private set; }
    [HideInInspector] public PlayerMovement MoveCompo { get; private set; }
    [HideInInspector] public Animator AnimatorCompo { get; private set; }

    [HideInInspector] public Transform rotationTrm;
    [HideInInspector] public Transform visualTrm;

    public PlayerStateMachine StateMachine { get; private set; }


    public LayerMask WhatIsMe { get; set; }

    [SerializeField] private float _autoTargetRadius = 12;
    [SerializeField] private float _reTargetRadius = 7;
    [SerializeField] private float _stopMoveForTargetRadius = 5f;
    [SerializeField] private int maxHp = 100;
    [SerializeField] private Collider targetCollider = null;
    public int currentHp = 100;
    public int Hp
    {
        get
        {
            return currentHp;
        }
        set
        {
            currentHp = value;
            currentHp = Mathf.Clamp(currentHp, 0, maxHp);
            UIManager.Instance.HpUIUpDate(currentHp);
            if (currentHp == 0)
            {
                currentHp = 0;
                StateMachine.StateChange(PlayerStateEnum.Dead);
                GameManager.Instance.PlayerDie();
            }
        }
    }

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        foreach (PlayerStateEnum playerState in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            try
            {
                string stateName = playerState.ToString();

                Type t = Type.GetType($"Player{stateName}State");
                PlayerState newPlayerState =
                    Activator.CreateInstance(t, this, StateMachine, stateName) as PlayerState;

                StateMachine.AddState(playerState, newPlayerState);
            }
            catch (Exception e)
            {
                Debug.LogError("Ŭ���� �̸� �߸������� ��û��");
                Debug.LogException(e);
            }
        }
        InputCompo = GetComponent<PlayerInput>();
        MoveCompo = GetComponent<PlayerMovement>();
        MoveCompo.Initialize();

        visualTrm = transform.Find("Visual");
        AnimatorCompo = visualTrm.GetComponent<Animator>();

        rotationTrm = transform.Find("Rotation");

        InputCompo.AttackEvent += HandleAttackEvent;
        InputCompo.MovementEvent += HandleMovementEvent;
    }

    private void HandleMovementEvent(Vector3 movement)
    {
        if (OnSkill || CanNextAnimation == false) return;
        if (movement.sqrMagnitude > 0)
        {
            StateMachine.StateChange(PlayerStateEnum.Run);
        }
    }

    private void HandleAttackEvent()
    {
        if (MoveCompo.IsGround == false || Cursor.visible || CanNextAnimation == false || OnSkill) return;
        StateMachine.StateChange(PlayerStateEnum.DefaultAttack);
    }

    private void Start()
    {
        StateMachine.Initialize(PlayerStateEnum.Idle, this);

        GameManager.Instance.SetCursorActive(false);

        CanNextAnimation = true;
        OnSkill = false;
    }

    private void Update()
    {
        if (GameManager.Instance.isDie) return;

        //디버그
        /*if (Input.GetKeyDown(KeyCode.L))
        {
            ApplyDamage(10, Vector3.zero, Vector3.zero, 0, 0, new MonoBehaviour());
            CameraManager.Instance.CameraShake(8, 10, 0.15f);
        }*/

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            IsRuning = !IsRuning;
        }

        if (GameManager.Instance.currentChip != null && OnSkill == false)
        {
            if (Input.GetKeyDown(KeyCode.Q) && GameManager.Instance.currentChip.qSkill != null
                && GameManager.Instance.currentChip.qSkill.CanUseSkill)
            {
                StateMachine.StateChange(PlayerStateEnum.QSkill);
                UIManager.Instance.UseQSkill(GameManager.Instance.currentChip);
            }
            if (Input.GetKeyDown(KeyCode.E) && GameManager.Instance.currentChip.eSkill != null
                && GameManager.Instance.currentChip.eSkill.CanUseSkill)
            {
                StateMachine.StateChange(PlayerStateEnum.ESkill);
                UIManager.Instance.UseESkill(GameManager.Instance.currentChip);
            }
        }

        StateMachine.CurrentState.UpdateState();
    }

    public void SetTarget()
    {
        Collider[] colliders =
            Physics.OverlapSphere(transform.position, _autoTargetRadius, 1 << LayerMask.NameToLayer("Enemy"));

        if (colliders.Length == 0)
        {
            targetCollider = null;
            return;
        }

        targetCollider = colliders[0];
        float minDistance = _autoTargetRadius;
        foreach (Collider coll in colliders)
        {
            float dis = Vector3.Distance(coll.transform.position, transform.position);
            if (dis < minDistance)
            {
                minDistance = dis;
                targetCollider = coll;
            }
        }
    }

    public void LookEnemy()
    {
        if (targetCollider != null && targetCollider.enabled)
        {
            float dis = TargetEnemyDistance();
            if (dis > _autoTargetRadius)
                targetCollider = null;
            else if (dis > _reTargetRadius)
                SetTarget();
        }
        else
            SetTarget();

        if (targetCollider != null)
        {
            Vector3 lookDir =
                Vector3.ProjectOnPlane(targetCollider.transform.position - transform.position, Vector3.up).normalized;
            MoveCompo.SetRotation(lookDir);
        }
        else
            MoveCompo.SetInputRotation();
    }

    public float TargetEnemyDistance()
    {
        if (targetCollider == null) return 100;
        return Vector3.Distance(targetCollider.transform.position, transform.position);
    }

    public bool CanMoveForTarget()
        => _stopMoveForTargetRadius < TargetEnemyDistance();

    public void ApplyDamage(int damage, Vector3 hitPoint, Vector3 normal, float knockbackPower, float knockBackTime, MonoBehaviour dealer)
    {
        Hp -= damage;
    }
}

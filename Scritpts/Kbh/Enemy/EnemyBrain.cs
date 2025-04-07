using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum EnemyType
{
    Punch,
    Shoot
}


public abstract class EnemyBrain : Poolable, IHealth
{
    #region Compoents
    public static bool IsRayShow;
    [SerializeField] private bool isRayShow;

    public Rigidbody RigidbodyCompo { get; set; }
    public NavMeshAgent NavMeshCompo { get; set; }
    public Animator AnimatorCompo { get; set; }
    public EnemyStateMachine StateMachineCompo { get; set; }
    public EnemyHpBar EnemyHpBarCompo { get; set; }
    public EnemyDieHandle enemyDieHandleCompo { get; set; }
    public EnemyAttackData ActionData { get; set; }

    private readonly int _DieAniHash = Animator.StringToHash("DieCounter");

    [field: SerializeField] public int MaxHp { get; set; }
    [SerializeField] private int hp;
    public int Hp
    {
        get => Mathf.Clamp(hp, 0, MaxHp);
        set
        {
            hp = Mathf.Clamp(value, 0, MaxHp);
            EnemyHpBarCompo.Percent = (float)hp / MaxHp;
            if (hp == 0)
            {
                AnimatorCompo.SetFloat(_DieAniHash, Random.Range(0, 2));
                StateMachineCompo.ChangeState(EnemyEnum.Die);
            }
        }
    }
    [SerializeField] private Renderer _visual;
    [field: SerializeField] public LayerMask WhatIsMe { get; set; }

    [Space(15)][SerializeField] private EntityStat _entityStat;
    [Space(15)] public EnemyMovement movement;
    [Space(15)] public EnemyAttack attack;
    [Space(15)][SerializeField] private EnemyEffectPlayer _effectPlayer;

    [HideInInspector] public GameManager gameManager;

    private EnemySpawner spawner;

    #endregion
    #region Initialize

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    public void SetSpawner(EnemySpawner owner)
    {
        spawner = owner;
    }

    public override void Initialize()
    {
        spawner = null;
        ActionData = new EnemyAttackData();

        EnemyHpBarCompo = GetComponentInChildren<EnemyHpBar>();
        EnemyHpBarCompo.SetOwner(this);
        Hp = MaxHp;


        AnimatorCompo = GetComponentInChildren<Animator>();
        AnimatorCompo.runtimeAnimatorController
           = Instantiate(AnimatorCompo.runtimeAnimatorController);
        RigidbodyCompo = GetComponent<Rigidbody>();
        NavMeshCompo = GetComponent<NavMeshAgent>();
        NavMeshCompo.Warp(transform.position);

        movement.SetOwner(this);
        movement.Initialize(this);


        _entityStat.SetOwner(this);
        attack.SetOwner(this);
        _effectPlayer.SetOwner(this);


        enemyDieHandleCompo = GetComponentInChildren<EnemyDieHandle>();
        enemyDieHandleCompo.SetOwner(this);

        gameManager
            .DelayCallback(new WaitForSeconds(3f),
            StartStateMachine);

    }

    private void StartStateMachine()
    {
        StateMachineCompo = new EnemyStateMachine();
        StateMachineCompo.SetOwner(this);
        StateMachineCompo.Initialize(EnemyEnum.Idle, StringToTypeGetter);
    }



    #endregion

    private void Update()
    {
        StateMachineCompo?.Update();
    }

    public void ApplyDamage(int damage, Vector3 hitPoint, Vector3 hitDir, float knockbackPower, float knockbackTime, MonoBehaviour dealer)
    {
        if (StateMachineCompo is null) return;

        //Debug.Log($"{gameManager.name}이 {damage}의 피해를 입었습니다.");
        
        Hp -= damage;

        movement.GiveKnockback(hitDir * knockbackPower, knockbackTime,
           () =>
           {
               if (Hp > 0)
                   StateMachineCompo.ChangeState(EnemyEnum.Idle);
           });
        Poolable poolable = PoolManager.Instance.Pop("HitEffect");
        poolable.transform.position = transform.position + Vector3.up;
    }


    public EntityRequest UpdateObject(ref EntityStruct input)
    {
        EntityRequest request = new EntityRequest();
        return request;
    }


    public virtual string StringToTypeGetter(string str)
       => $"Enemy{str}State";

    public virtual void AnimationEndTrigger()
    {
        StateMachineCompo.AnimationTrigger();
    }

    public virtual void Die(float delay = 10)
    {
        movement.SetActiveNavAent(false);
        movement.SetGravityState(false);
        enemyDieHandleCompo.OnDie();
        StateMachineCompo.Dispose();

        if (spawner == null) return;
        spawner.EnemyDie();
    }


    public virtual void AttackTrigger()
    {
        attack.lastAttackTime = Time.time;

        string typeName = GetType().ToString().Replace("EnemyBrain", "");
        Transform effectTrm = PoolManager.Instance.Pop($"{typeName}Effect", attack.attackTrm.position).transform;
        effectTrm.forward = (gameManager.player.transform.position + Vector3.up - effectTrm.position).normalized;
        effectTrm.Rotate(attack.rotateScale);
    }




    #region OnlyEditor
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        isRayShow = IsRayShow;

        if (!IsRayShow)
            return;

        attack.SetOwner(this);
        attack.OnDrawGizmo();

    }

    private void OnValidate()
    {
        IsRayShow = isRayShow;
    }


#endif
    #endregion



}

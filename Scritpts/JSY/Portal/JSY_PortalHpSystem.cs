using DG.Tweening;
using System;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class JSY_PortalHpSystem : MonoBehaviour, IHealth
{
    [field: SerializeField] public LayerMask WhatIsMe { get; set; }

    [field: SerializeField] public int MaxHp { get; set; }
    [SerializeField] private int hp;
    
    public event Action OnDieEvent;
    private bool isDie = false;

    private Canvas hpCanvas;
    private RectTransform hpImage;
    private Transform _visualTrm;

    private void Awake()
    {
        hpCanvas = GetComponentInChildren<Canvas>();
        hpImage = hpCanvas.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (IsDie) return;
        Vector3 direction = Camera.main.transform.position - hpCanvas.transform.position;
        direction.y = 0; // y 축의 회전을 고정
        hpCanvas.transform.rotation = Quaternion.LookRotation(direction);
    }

    public void SetVisualTrm(Transform visualTrm)
    {
        _visualTrm = visualTrm;
    }

    public bool IsDie
    {
        get => isDie;
        set
        {
            isDie = value;
        }
    }

    public int Hp
    {
        get => Mathf.Clamp(hp, 0, MaxHp);
        set
        {
            hp = Mathf.Clamp(value, 0, MaxHp);
            if (hp <= 0)
            {
                PortalDie();
            }
        }
    }


    private void PortalDie()
    {
        isDie = true;
        hpCanvas.enabled = false;
        OnDieEvent?.Invoke();
    }

    public void ApplyDamage(int damage, Vector3 hitPoint, Vector3 normal, float knockbackPower, float knockBackTime, MonoBehaviour dealer)
    {
        Hp -= damage;

        Transform hitEffectTrm = PoolManager.Instance.Pop("HitEffect").transform;
        hitEffectTrm.position = _visualTrm.position;

        ChangeUI();
    }

    private void ChangeUI()
    {
        hpImage.localScale = new Vector2((float)Hp / MaxHp, 1);
    }
}

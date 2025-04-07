using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum TrapDirection
{
    Up,
    Down,
    Left,
    Right
}

public class TrapManager : MonoSingleton<TrapManager>
{

    [Header("HitTrap")]
    public bool isHitThom;

    [Header("SlowTrap")]
    public bool isSlowTrap;

    [Header("SiaTrap")]
    [SerializeField] private float _siaSprayAlpa;
    [SerializeField] private float _siaTime;
    public bool isSiaTrap { get; private set; } = false;
    private Material _cameraSprayMat;

    private readonly int AlphaHash = Shader.PropertyToID("_Alpa");

    private void Awake()
    {
        _cameraSprayMat = Camera.main.transform.Find("SiaSpray").GetComponent<SpriteRenderer>().material;
        if (_cameraSprayMat != null)
        {
            _cameraSprayMat.SetFloat(AlphaHash, 0);
        }
    }

    public void ActiveSiaTrap(bool active)
    {
        Debug.Log(active);
        if (isSiaTrap == active) return;
        isSiaTrap = active;
        _cameraSprayMat.DOKill();
        _cameraSprayMat.DOFloat(active ? _siaSprayAlpa : 0, AlphaHash, _siaTime);
    }
}
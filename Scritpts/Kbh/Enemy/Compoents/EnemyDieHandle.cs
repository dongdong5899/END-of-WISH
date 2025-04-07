using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemyDieHandle : MonoBehaviour, IPartialCompoent
{
   public MonoBehaviour Owner { get; set; }
   [SerializeField] private float _downingTime = 2f;
   public float dissolveTime = 0.7f;
   [SerializeField] private Ease dissolveEase = Ease.Linear;

   private int _dissolveHash = Shader.PropertyToID("_DissolvePercent");
   private Vector3 defaultPosition;
   [SerializeField] private Transform _visual;
   [SerializeField] private Renderer[] _renderers;
   [SerializeField] private CapsuleCollider _defaultCollider;
   [SerializeField] private BoxCollider _dieCollider;

   public void OnDie()
   {
      ColliderEnable(false);

      if (true)
         DieTween();
   }

   public void ColliderEnable(bool value)
   {
      _defaultCollider.enabled = value;
      _dieCollider.enabled = !value;
   }


   private void DieTween()
   {
      Sequence seq = DOTween.Sequence();
      seq.AppendInterval(_downingTime);
      _dieCollider.enabled = false;

      for(int i = 0; i<_renderers.Length; ++i)
      {
         if(i == 0)
            seq.Append(_renderers[i].material.DOFloat(1, _dissolveHash, dissolveTime))
               .SetEase(dissolveEase);
         else
            seq.Join(_renderers[i].material.DOFloat(1, _dissolveHash, dissolveTime))
               .SetEase(dissolveEase);
      }

      seq.AppendCallback(() => PoolManager.Instance.Push(Owner as Poolable));

   }

   private void Awake()
   {
      _defaultCollider = GetComponentInParent<CapsuleCollider>();
      _dieCollider = GetComponent<BoxCollider>();
      defaultPosition = transform.position;
   }

   public void SetOwner(MonoBehaviour owner)
   {
      Owner = owner;
      DissolveEffectEnter();
   }

   private void DissolveEffectEnter()
   {
      ColliderEnable(false);
      Sequence seq = DOTween.Sequence();
      
      for (int i = 0; i < _renderers.Length; ++i)
      {
         _renderers[i].material.SetFloat(_dissolveHash, 1);

         if (i == 0)
            seq.Append(_renderers[i].material.DOFloat(0, _dissolveHash, dissolveTime))
               .SetEase(dissolveEase);
         else
            seq.Join(_renderers[i].material.DOFloat(0, _dissolveHash, dissolveTime))
                .SetEase(dissolveEase);
      }
      seq.AppendCallback(() => ColliderEnable(true));
   }
}

using UnityEngine;
using DG.Tweening;

public class Item : MonoBehaviour
{

    [SerializeField] private ItemSO _itemSO;
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private float _itemStartYPos;
    [SerializeField] private float _itemEndYPos;

    private bool _isActive;
    private bool _ismoveLoop;
    private Sequence _seq;
    private MeshRenderer _mataerial;
    private Transform _parentTrm;
    private ItemStruct _item;
    private Animator _animator;

    private void Awake()
    {
        //Debug.Log(transform.position);
        _parentTrm = transform.parent;
        _item = _itemSO.RandomSpawnItem();
        _mataerial = GetComponent<MeshRenderer>();
        _mataerial.material = _item.itemMaterial;
        _animator = _parentTrm.Find("Visual").GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //transform.localPosition = Vector3.zero;
        _seq = DOTween.Sequence();
        _seq.Append(transform.DOLocalMoveY(_itemStartYPos, 2))
            .AppendCallback(() => _isActive = true)
            .AppendCallback(() => _ismoveLoop = true);

    }

    

    private void Update()
    {
        if(_ismoveLoop)
        {
            transform.DOLocalMoveY(transform.localPosition.y + _itemEndYPos, 2)
                .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            _ismoveLoop = false;
        }

        if (_isActive && Input.GetKeyDown(KeyCode.F))
        {
            Collider[] colliders;
            colliders = Physics.OverlapSphere(transform.position, 5, _playerMask);
            if (colliders.Length > 0)
            {
 
                JSY_ChipItem chipItem = _item.prefab.GetComponent<JSY_ChipItem>();
                UIManager.Instance.AddItem(chipItem);
                transform.DOKill();
                _animator.SetTrigger("Close");
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,5);
    }
}

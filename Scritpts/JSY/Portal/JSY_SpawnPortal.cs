using DG.Tweening;
using UnityEngine;

public class JSY_SpawnPortal : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 5;
    [SerializeField] private float upDownMoveSpeed = 5;

    private JSY_PortalHpSystem portalHp;

    private Transform visualTrm;
    private Material dissolveMat;
    private BoxCollider _collider;

    private void Start()
    {
        visualTrm = transform.Find("Visual");
        _collider = GetComponent<BoxCollider>();
        portalHp = GetComponent<JSY_PortalHpSystem>();
        portalHp.SetVisualTrm(visualTrm);
        dissolveMat = visualTrm.GetComponent<MeshRenderer>().material;
        portalHp.OnDieEvent += HandlePortalDieEvent;
    }

    private void Update()
    {
        if (portalHp.IsDie) return;
        visualTrm.Rotate(new Vector3(0, 10f, 0) * rotateSpeed * Time.deltaTime);
        float yPos = 1.5f + Mathf.Sin(180 * Mathf.Deg2Rad * Time.time) * 0.6f;
        visualTrm.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }

    private void HandlePortalDieEvent()
    {
        dissolveMat.DOFloat(1f, "_Dissolve", 1f);
        Destroy(gameObject, 1f);
        _collider.enabled = false;
    }

}

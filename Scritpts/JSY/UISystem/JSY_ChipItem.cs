using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JSY_ChipItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    [SerializeField] private ChipSO chipDataSO;
    private Image itemImage;
    private JSY_EquipItemSlot equipSlot;

    private Transform[] selectedSlots = new Transform[4];
    private InventorySlot[] inventories;

    [HideInInspector] public Transform parentAfterDrag;
    public Transform canvasTrm;

    private void Awake()
    {
        itemImage = GetComponent<Image>();
        itemImage.sprite = chipDataSO.sprite;
        parentAfterDrag = transform.parent;
        chipDataSO.Initialize();

        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        itemImage.raycastTarget = false;
        transform.SetParent(canvasTrm);

        equipSlot = parentAfterDrag.GetComponent<JSY_EquipItemSlot>();
        AddSlotChipData(null);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 ratio = new Vector2(1920f / Screen.width , 1080f / Screen.height);
        Vector2 delta = new Vector2(ratio.x * eventData.delta.x, ratio.y * eventData.delta.y);
        itemImage.rectTransform.anchoredPosition += delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        transform.SetParent(parentAfterDrag);
        itemImage.raycastTarget = true;

        equipSlot = parentAfterDrag.GetComponent<JSY_EquipItemSlot>();
        AddSlotChipData(chipDataSO);
    }
     
    public void OnPointerClick(PointerEventData eventData)
    {
        if(transform.parent.TryGetComponent<JSY_EquipItemSlot>(out JSY_EquipItemSlot equipItemSlot))
        {
            foreach (InventorySlot slot in inventories)
            {
                if (slot.transform.childCount == 0)
                {
                    equipSlot = equipItemSlot;
                    AddSlotChipData(null);
                    transform.SetParent(slot.transform);
                    return;
                }
            }
        }
        else
        {
            foreach(Transform slotTrm in selectedSlots)
            {
                if (slotTrm.childCount == 0)
                {
                    equipSlot = slotTrm.GetComponent<JSY_EquipItemSlot>();
                    AddSlotChipData(chipDataSO);
                    transform.SetParent(slotTrm);
                    return;
                }
            }
        }
        
    }

    private void AddSlotChipData(ChipSO chipData)
    {
        if (equipSlot != null)
        {
            FindObjectOfType<JSY_SkillUI>().AddChipData(chipData, equipSlot.slotIndex);
        }
    }
    public void SelectedChipSlot(Transform selectedChipSlot, InventorySlot[] inventorySlots, Transform canvasTrm)
    {
        this.canvasTrm = canvasTrm;
        inventories = inventorySlots;
        for (int i = 0; i < selectedChipSlot.childCount; ++i)
            selectedSlots[i] = selectedChipSlot.GetChild(i).transform;
    }
}

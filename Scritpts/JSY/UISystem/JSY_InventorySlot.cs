using UnityEngine;
using UnityEngine.EventSystems;

public class JSY_InventorySlot : InventorySlot
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            JSY_ChipItem inventoryItem = eventData.pointerDrag.GetComponent<JSY_ChipItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }
}

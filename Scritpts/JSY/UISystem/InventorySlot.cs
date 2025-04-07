using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InventorySlot : MonoBehaviour, IDropHandler
{
    public abstract void OnDrop(PointerEventData eventData);
}

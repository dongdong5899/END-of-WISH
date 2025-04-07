using UnityEngine;

public class JSY_InventorySystem : MonoBehaviour
{
    [SerializeField] private InventorySlot[] inventorySlots;
    [SerializeField] private Transform selectedChipSlot;
    [SerializeField] private JSY_ChipItem[] inventoryItemPrefabs;
    [SerializeField] private Transform canvasTrm;

    private void Awake()
    {
        inventorySlots = GetComponentsInChildren<InventorySlot>();

        foreach (JSY_ChipItem item in inventoryItemPrefabs)
        {
            AddItem(item);
        }
    }

    private void Update()
    {
        //µð¹ö±×
        /*if(Input.GetKeyDown(KeyCode.I))
        {
            AddItem();
        }*/
    }
    public void AddItem(JSY_ChipItem item)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.transform.childCount == 0)
            {
                SpawnNewItem(slot, item);
                return;
            }
        }
    }

    private void SpawnNewItem(InventorySlot slot, JSY_ChipItem item)
    {
        JSY_ChipItem newItem = Instantiate(item, slot.transform);
        newItem.SelectedChipSlot(selectedChipSlot, inventorySlots, canvasTrm);
    }
}

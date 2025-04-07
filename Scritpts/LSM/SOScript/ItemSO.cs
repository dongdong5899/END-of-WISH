using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemStruct
{
    public Material itemMaterial;
    public GameObject prefab;
}


[CreateAssetMenu(menuName ="SO/ItemSO")]
public class ItemSO : ScriptableObject
{

    public ItemStruct[] item;

    public ItemStruct RandomSpawnItem()
    {
        int itemNumber = Random.Range(0, item.Length);
        return item[itemNumber];
    }

}

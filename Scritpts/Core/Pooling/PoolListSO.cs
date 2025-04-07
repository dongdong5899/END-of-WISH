using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PoolData
{
    public string name;
    public Poolable prefab;
    public int count;
}

[CreateAssetMenu(fileName = "PoolListSO", menuName = "SO/PoolList")]
public class PoolListSO : ScriptableObject
{
    public PoolData[] poolDatas;
    
}


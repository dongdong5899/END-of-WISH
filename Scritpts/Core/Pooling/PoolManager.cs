using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    [SerializeField] private PoolListSO poolList;

    private Dictionary<string, Stack<Poolable>> poolDictionary = new Dictionary<string, Stack<Poolable>>();

    private void Awake()
    {
        foreach (PoolData poolData in poolList.poolDatas)
        {
            Stack<Poolable> poolables = new Stack<Poolable>();
            poolDictionary.Add(poolData.name, poolables);

            CreatePool(poolData.name, poolData.count);
        }
    }

    private void CreatePool(string name, int count = 1)
    {
        PoolData poolData;
        foreach (PoolData pool in poolList.poolDatas)
        {
            if (pool.name != name) continue;
            poolData = pool;

            for (int i = 0; i < count; i++)
            {
                Poolable poolObj = Instantiate(poolData.prefab, transform);
                poolObj.name = poolObj.name.Replace("(Clone)", "");
                poolObj.gameObject.SetActive(false);
                poolDictionary[name].Push(poolObj);
            }
            break;
        }
    }

    public Poolable Pop(string name, Vector3 pos = default)
    {
        if (poolDictionary[name].Count <= 0) CreatePool(name, 10);
        Poolable poolObj = poolDictionary[name].Pop();
        if (pos != default)
            poolObj.transform.position = pos;
        poolObj.gameObject.SetActive(true);
        poolObj.Initialize();
        return poolObj;
    }

    public void Push(Poolable poolable)
    {
        poolable.gameObject.SetActive(false);
        if (poolDictionary.TryGetValue(poolable.name, out Stack<Poolable> poolStack))
        {
            poolStack.Push(poolable);
        }
    }
}

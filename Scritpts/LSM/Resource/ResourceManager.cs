using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    
    private Dictionary<ResourceEnum, ResourceSO> _resourceName = new Dictionary<ResourceEnum, ResourceSO>();
    
    [SerializeField] private ResourceListSO _resourceListSO;
    

    private void Start()
    {
        for (int i = 0; i < _resourceListSO.resourceList.Length; ++i)
        {
            _resourceName.Add((ResourceEnum)i, _resourceListSO.resourceList[i]);
            _resourceListSO.resourceList[i].currentCount = _resourceListSO.resourceList[i].basecount;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddResource(ResourceEnum.Coin, 1);
        }
    }


    public void AddResource(ResourceEnum resource,int amount)
    {

        ResourceSO resourceSO = _resourceName[resource];
        resourceSO.currentCount += amount;
    }

    public void SetResource(ResourceEnum resource, int amount)
    {

        ResourceSO resourceSO = _resourceName[resource];
        resourceSO.currentCount = amount;
    }
    public int GetResourceAmoount(ResourceEnum resource)
    {

        return _resourceName[resource].currentCount;
    }

}

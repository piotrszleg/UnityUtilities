using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pool : Singleton<Pool>
{
    Dictionary<PoolObject, List<PoolObject>> pooledObjects=new Dictionary<PoolObject, List<PoolObject>>();
    
    private void Start() {

    }

    public void Register(PoolObject prefab, uint expectedCount)
    {
        if (prefab == null)
        {
            throw new ArgumentNullException("prefab");
        }
        if(!enabled){
            return;
        }
        List<PoolObject> instances;
        if (!pooledObjects.TryGetValue(prefab, out instances))
        {
            instances = pooledObjects[prefab] = new List<PoolObject>();
        } else if (instances.Count<=expectedCount)
        {
            expectedCount -= (uint)instances.Count;
        } else
        {
            // no pooling is needed
            return;
        }
        for (int i = 0; i<expectedCount; i++){
            PoolObject newInstance = Instantiate(prefab, transform);
            newInstance.Initialize(prefab);
            newInstance.Deactivate();
            instances.Add(newInstance);
        }
    }

    public PoolObject Request(PoolObject prefab)
    {
        if (prefab == null)
        {
            throw new ArgumentNullException("prefab");
        }
        if(!enabled){
            return Instantiate(prefab);       
        }
        List<PoolObject> instances;
        if (!pooledObjects.TryGetValue(prefab, out instances))
        {
            instances = pooledObjects[prefab] = new List<PoolObject>();
        }
        if (instances.Count == 0)
        {
            PoolObject newInstance=Instantiate(prefab, transform);
            newInstance.Initialize(prefab);
            return newInstance;
        } else
        {
            int lastIndex = instances.Count - 1;
            PoolObject pooled = instances[lastIndex];
            instances.RemoveAt(lastIndex);
            pooled.Reset();
            pooled.Activate();
            instance.transform.SetParent(null);
            return pooled;
        }
    }

    public PoolObject Request(PoolObject prefab, Vector3 position, Quaternion rotation, Transform parent=null)
    {
        PoolObject requestResult = Request(prefab);
        if (requestResult != null)
        {
            requestResult.transform.position = position;
            requestResult.transform.rotation = rotation;
            requestResult.transform.SetParent(parent);
        }
        return requestResult;
    }

    public PoolObject Request(PoolObject prefab, Transform parent)
    {
        PoolObject requestResult = Request(prefab);
        if (requestResult != null)
        {
            requestResult.transform.position = parent.position;
            requestResult.transform.rotation = parent.rotation;
            requestResult.transform.SetParent(parent);
        }
        return requestResult;
    }

    public void Return(PoolObject prefab, PoolObject instance)
    {
        if (prefab == null)
        {
            throw new ArgumentNullException("prefab");
        }
        instance.Deactivate();
        instance.transform.SetParent(transform);
        pooledObjects[prefab].Add(instance);
    }
}
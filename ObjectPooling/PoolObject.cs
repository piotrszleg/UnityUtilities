using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PoolObject : MonoBehaviour
{
    PoolObject prefab;

    [HideInInspector]
    public bool active;
    [HideInInspector]
    public UnityEvent onActivate;
    [HideInInspector]
    public UnityEvent onDeactivate;

    static Dictionary<GameObject, PoolObject> cached=new Dictionary<GameObject, PoolObject>();
    // drop in replacement fro Object.Destroy for objects that may be pooled
    public static void ReturnOrDestroy(GameObject target)
    {
        PoolObject poolObject;
        if (!cached.TryGetValue(target, out poolObject) || poolObject == null) {
            poolObject = target.GetComponent<PoolObject>();
            if (poolObject != null)
            {
                cached[target] = poolObject;
            }
        }
        if (poolObject != null)
        {
            poolObject.ReturnSelf();
        }
        else
        {
            Destroy(target);
        }
    }

    public void Initialize(PoolObject prefab)
    {
        this.prefab = prefab;
    }

    public virtual void Reset()
    {
        
    }

    public virtual void Activate()
    {
        onActivate.Invoke();
        gameObject.SetActive(true);
        active = true;
    }

    public virtual void Deactivate()
    {
        onDeactivate.Invoke();
        gameObject.SetActive(false);
        active = false;
    }

    public virtual void ReturnSelf()
    {
        if (prefab != null)
        {
            Pool.instance.Return(prefab, this);
        } else
        {
            Destroy(gameObject);
        }
    }
}

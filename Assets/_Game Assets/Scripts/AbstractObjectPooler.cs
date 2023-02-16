using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem<T>
{

    public T objectToPool;
    public int amountToPool;
    public bool shouldExpand = true;

    public ObjectPoolItem(T obj, int amt, bool exp = true)
    {
        objectToPool = obj;
        amountToPool = Mathf.Max(amt, 2);
        shouldExpand = exp;
    }
}

public abstract class AbstractObjectPooler<T> : MonoBehaviour where T : MonoBehaviour
{
    public List<ObjectPoolItem<T>> itemsToPool;


    public List<List<T>> pooledObjectsList;
    public List<T> pooledObjects;
    private List<int> positions;

    #region Unity's Callback
    protected virtual void Awake()
    {
        pooledObjectsList = new List<List<T>>();
        pooledObjects = new List<T>();
        positions = new List<int>();

        for (int i = 0; i < itemsToPool.Count; i++)
        {
            ObjectPoolItemToPooledObject(i);
        }

    }
    #endregion

    public T GetPooledObject(int index)
    {

        int curSize = pooledObjectsList[index].Count;
        for (int i = positions[index] + 1; i < positions[index] + pooledObjectsList[index].Count; i++)
        {

            if (!pooledObjectsList[index][i % curSize].gameObject.activeInHierarchy)
            {
                positions[index] = i % curSize;
                return pooledObjectsList[index][i % curSize];
            }
        }

        if (itemsToPool[index].shouldExpand)
        {

            T obj = Instantiate(itemsToPool[index].objectToPool);
            obj.gameObject.SetActive(false);
            obj.transform.parent = this.transform;
            pooledObjectsList[index].Add(obj);
            return obj;

        }
        return default;
    }

    public List<T> GetAllPooledObjects(int index)
    {
        return pooledObjectsList[index];
    }


    public int AddObject(T GO, int amt = 3, bool exp = true)
    {
        ObjectPoolItem<T> item = new ObjectPoolItem<T>(GO, amt, exp);
        int currLen = itemsToPool.Count;
        itemsToPool.Add(item);
        ObjectPoolItemToPooledObject(currLen);
        return currLen;
    }


    void ObjectPoolItemToPooledObject(int index)
    {
        ObjectPoolItem<T> item = itemsToPool[index];

        pooledObjects = new List<T>();
        for (int i = 0; i < item.amountToPool; i++)
        {
            T obj = Instantiate(item.objectToPool);
            obj.gameObject.SetActive(false);
            obj.transform.parent = this.transform;
            pooledObjects.Add(obj);
        }
        pooledObjectsList.Add(pooledObjects);
        positions.Add(0);

    }
}

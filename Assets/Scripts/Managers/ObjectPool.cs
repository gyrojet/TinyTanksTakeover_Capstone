using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public static class ObjectPool
{
    public static Dictionary<string, Queue<Component>> itemPoolDictionary = new Dictionary<string, Queue<Component>>();

    public static void ReturnObjectToPool<T>(T obj, string name) where T : Component
    {
        if (!obj.gameObject.activeSelf)
            return;

        obj.transform.position = Vector2.zero;

        itemPoolDictionary[name].Enqueue(obj);

        obj.gameObject.SetActive(false);
    }

    public static T RetrieveObjectFromPool<T>(string dictKey) where T : Component
    {
        return (T)itemPoolDictionary[dictKey].Dequeue();
    }

    public static void SetupItemPool<T>(T pooledItemPrefab, int poolSize, string dictionaryKey) where T : Component
    {
        itemPoolDictionary.Add(dictionaryKey, new Queue<Component>());

        for (int i = 0; i < poolSize; i++)
        {
            T pooledItemInstance = Object.Instantiate(pooledItemPrefab);
            pooledItemInstance.gameObject.SetActive(false);

            itemPoolDictionary[dictionaryKey].Enqueue(pooledItemInstance);
        }
    }
}

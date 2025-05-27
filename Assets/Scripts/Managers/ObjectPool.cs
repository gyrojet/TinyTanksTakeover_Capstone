using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using JetBrains.Annotations;
using Unity.VisualScripting.FullSerializer;

public static class ObjectPool
{
    public static Dictionary<string, Component> poolLookup = new Dictionary<string, Component>();
    public static Dictionary<string, Queue<Component>> itemPoolDictionary = new Dictionary<string, Queue<Component>>();

    public static void EnqueueObject<T>(T obj, string name) where T : Component
    {
        if (!obj.gameObject.activeSelf)
            return;

        obj.transform.position = Vector2.zero;

        itemPoolDictionary[name].Enqueue(obj);

        obj.gameObject.SetActive(false);
    }

    public static T DequeueObject<T>(string dictKey) where T : Component
    {
        if (itemPoolDictionary[dictKey].TryDequeue(out var item))
            return (T)item;

        return (T)EnqueueNewInstance(poolLookup[dictKey], dictKey);
    }

    public static T EnqueueNewInstance<T>(T item, string key) where T : Component
    {
        T newInstance = Object.Instantiate(item);
        newInstance.gameObject.SetActive(false);
        newInstance.transform.position = Vector2.zero;
        itemPoolDictionary[key].Enqueue(newInstance);

        return newInstance;
    }

    public static void SetupItemPool<T>(T pooledItemPrefab, int poolSize, string dictionaryKey) where T : Component
    {
        if (!itemPoolDictionary.ContainsKey(dictionaryKey))
            itemPoolDictionary.Add(dictionaryKey, new Queue<Component>());

        for (int i = 0; i < poolSize; i++)
        {
            T pooledItemInstance = Object.Instantiate(pooledItemPrefab);
            pooledItemInstance.gameObject.SetActive(false);

            itemPoolDictionary[dictionaryKey].Enqueue(pooledItemInstance);
        }
    }
}

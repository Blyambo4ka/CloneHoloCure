using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler Instance;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;


    public void ClearPool(string tag)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            Queue<GameObject> pool = poolDictionary[tag];
            
            while (pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                Destroy(obj);
            }

            poolDictionary[tag] = new Queue<GameObject>(); // Пересоздаем очередь
        }
    }


    private void Awake()
    {
         Instance = this;
         poolDictionary = new Dictionary<string, Queue<GameObject>>();

         foreach (Pool pool in pools)
         {
            Queue<GameObject> objectPool = new Queue<GameObject>();

             for(int i = 0; i < pool.size; i++)
             {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
             }

              poolDictionary.Add(pool.tag, objectPool);
         }
    }

   public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject obj;
        
        if (poolDictionary[tag].Count == 0)
        {
            // Если в пуле нет объектов, создаем новый
            Pool pool = pools.Find(p => p.tag == tag);
            if (pool != null)
            {
                obj = Instantiate(pool.prefab);
            }
            else
            {
                Debug.LogWarning("No prefab found for tag " + tag);
                return null;
            }
        }
        else
        {
            obj = poolDictionary[tag].Dequeue();
        }

        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        return obj;
    }

    



   public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn`t exist.");
            return;
        }

        if (objectToReturn == null)
        {
            Debug.LogWarning("Trying to return a null object to the pool.");
            return;
        }

        // Сброс состояния объекта перед возвратом в пул
        Experience experienceScript = objectToReturn.GetComponent<Experience>();
        if (experienceScript != null)
        {
            experienceScript.ResetExperience();
        }

        objectToReturn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToReturn);
    }

}

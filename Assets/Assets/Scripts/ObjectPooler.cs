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
            var pool = poolDictionary[tag];
            var count = pool.Count;

            // Удаляем все объекты в этом пуле
            for (int i = 0; i < count; i++)
            {
                var obj = pool.Dequeue();
                Destroy(obj); // Полное уничтожение
            }
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
            Debug.LogWarning("Pool with tag " + tag + " doesn`t exist.");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();

        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;


        poolDictionary[tag].Enqueue(obj);
       
        return obj;
    }

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
       if (!poolDictionary.ContainsKey(tag))
       {
           Debug.LogWarning("Pool with tag " + tag + " doesn`t exist.");
           return;
       }

       // Сбросить состояние объекта перед его возвратом в пул
       Experience experienceScript = objectToReturn.GetComponent<Experience>();
       if (experienceScript != null)
       {
           experienceScript.ResetExperience();  // Сбросим все значения опыта
       }

       objectToReturn.SetActive(false);
       poolDictionary[tag].Enqueue(objectToReturn);
    }
}

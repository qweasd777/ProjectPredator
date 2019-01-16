using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;           
        public int size;                                        // how many of this object to place in pool (prepare in game scene)

        public void Initialise(bool useOnePoolSizeOnly)         // fn. just to check if pool is properly set
        {
            if(prefab == null)
            {
                Debug.LogWarning("Pool prefab is empty!");
                size = 0;
                return;
            }

            if(tag.Length == 0)
                tag = prefab.name;

            if(useOnePoolSizeOnly)
                size = _instance.standardizedPoolSize;

            size = Mathf.Clamp(size, 0, int.MaxValue);
        }
    }

    #region Singleton
    private static ObjectPooler _instance;
    public static ObjectPooler Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject.FindGameObjectWithTag("ObjectPooler").AddComponent<ObjectPooler>();
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    public void DestroyInstance()
    {
        // TODO: check agn if correct way of implementing destroy
        _instance = null;
        Destroy(this);
    }
    #endregion

    public bool useOnePoolSizeOnly = false;
    public int standardizedPoolSize = 1;
    public List<Pool> pools;                                            // list of objects in pools
    public Dictionary<string, Queue<GameObject>> poolDictionary;        // 'list' of pools in game scene

    void Awake()
    {
        if (_instance == null)
            _instance = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            pool.Initialise(useOnePoolSizeOnly);

            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.parent = transform;                       // just to set a default parent so all inactive ones would be child of ObjectPooler
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);                   // add queue of pool objects into dictionary using pool.tag as key
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("ERROR: Pool with tag " + tag + " doesn't exist!");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();       // dequeues 1 GameObject that has the same key as [tag], from pool queue in dictionary
        
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);                     // readd to prev said queue

        return objectToSpawn;
    }

    public int GetInactiveTilesIndex()
    {
        // TODO: 
        // 1) find out which tiles are inactive
        // 2) return them so that tilemanager can spawn new inactive tiles safely
        // - might need return an array of the tile indexes (that arent active) or sth

        return -1;
    }
}

// Object pooling by Brackeys: https://www.youtube.com/watch?v=tdSmKaJvCoA
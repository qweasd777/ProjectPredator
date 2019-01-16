using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;

    public bool usePlayerStartPosForOriginSpawn = true;     // uses player's start pos for tiles origin spawn pt    
    public float tileSpawnPtZ = 0f;                         // tiles spawnPt in Z axis (which is also last tile pos)
    public float tileLength = 20f;                          // tiles length in units
    public int maxTilesOnScreen = 7;                        // to prevent probs make sure objectpooler tiles is twice more than this value

    private bool firstTileHasSpawned = false;
    private int lastPrefabIndex = 0;                        // TODO: better logic patterns (need to do sth to track last tile index spawned, etc)
    private Transform playerTransform;
    private ObjectPooler objectPooler;

    private List<GameObject> activeTiles;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if(usePlayerStartPosForOriginSpawn)
            tileSpawnPtZ = playerTransform.position.z;

        objectPooler = ObjectPooler.Instance;

        activeTiles = new List<GameObject>();

        for(int i = 0; i < maxTilesOnScreen; i++) 
            SpawnTile();
    }

    void Update()
    {
        float playerZDistToLastMaxTile = tileSpawnPtZ - maxTilesOnScreen * tileLength;

        // Start pt (spawns new tile on touching this pt)
        // Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, playerZDistToLastMaxTile), new Vector3(transform.position.x, transform.position.y + 5, playerZDistToLastMaxTile), Color.green);
        // End pt (last tile, which is new spawn pt)
        // Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, tileSpawnPtZ), new Vector3(transform.position.x, transform.position.y + 5, tileSpawnPtZ), Color.red);

        float safeZone = tileLength * 2;    // safe zone length to despawn prev tiles player has passed + spawn new tiles

        if(playerTransform.position.z -  safeZone > playerZDistToLastMaxTile)
        {
            SpawnTile();
            DespawnPrevTile();
        }
    }
    
    void DespawnPrevTile()
    {
        activeTiles[0].SetActive(false);
        activeTiles.RemoveAt(0);
    }

    void SpawnTile(int prefabIndex = -1)
    {
        GameObject go = objectPooler.SpawnFromPool(tilePrefabs[RandomPrefabIndex()].name, Vector3.forward * tileSpawnPtZ, Quaternion.identity);

        if(go == null)
        {
            Debug.LogWarning("GameObject from objectPooler not found!");
            return;
        }

        go.transform.SetParent(transform);
        activeTiles.Add(go);

        tileSpawnPtZ += tileLength;  
    }

    int RandomPrefabIndex()
    {
        if(!firstTileHasSpawned)
        {
            firstTileHasSpawned = true;
            return 0;
        }

        int randomIndex = Random.Range(0, tilePrefabs.Length-1);

        return randomIndex;
    }
}

// TODO : 
// 1) better random logic patterns
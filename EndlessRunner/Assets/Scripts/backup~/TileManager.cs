using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOT USING OBJECT POOLING
public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;

    public bool usePlayerStartPosForOriginSpawn = true;     // uses player's start pos for tiles origin spawn pt    
    public float tileSpawnPtZ = 0f;                         // tiles spawnPt in Z axis (which is also last tile pos)
    public float tileLength = 20f;                          // tiles length in units
    public int maxTilesOnScreen = 7;        
    
    private Transform playerTransform;
    private ObjectPooler objectPooler;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if(usePlayerStartPosForOriginSpawn)
            tileSpawnPtZ = playerTransform.position.z;

        objectPooler = ObjectPooler.Instance;
    }

    void Update()
    {
        float playerZDistToLastMaxTile = tileSpawnPtZ - maxTilesOnScreen * tileLength;

        // Start pt (spawns new tile on touching this pt)
        // Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, playerZDistToLastMaxTile), new Vector3(transform.position.x, transform.position.y + 5, playerZDistToLastMaxTile), Color.green);
        // End pt (last tile, which is new spawn pt)
        // Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, tileSpawnPtZ), new Vector3(transform.position.x, transform.position.y + 5, tileSpawnPtZ), Color.red);

        if (playerTransform.position.z > playerZDistToLastMaxTile)
        {
            SpawnTile();
        }
    }

    void SpawnTile(int prefabIndex = -1)
    {
        GameObject go;

        go = Instantiate(tilePrefabs[0]) as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * tileSpawnPtZ;

        tileSpawnPtZ += tileLength;  
    }
}

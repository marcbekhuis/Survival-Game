using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [SerializeField] private Transform tempTarget;
    [SerializeField] private AIEntry[] aiEntries;

    [System.Serializable]
    public class AIEntry 
    {
        public GameObject prefab;
        public float spawnCooldown = 60;
        public int maxSpawnCount = 1;
        public Biome[] spawnBiomes;

        [HideInInspector] public float TimeUntillNextSpawn = 0;
        [HideInInspector] public int spawnCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GamePaused) return;

        foreach (var aiEntry in aiEntries)
        {
            if (aiEntry.spawnCount < aiEntry.maxSpawnCount)
            {
                if (GameTime.time > aiEntry.TimeUntillNextSpawn)
                {
                    List<MapTile> mapTiles = new List<MapTile>();
                    foreach (var item in MapData.instance.mapTilesFilterByBiomeNoResource)
                    {
                        mapTiles.AddRange(item.Value);
                    }

                    if (mapTiles.Count <= 0) return;
                    MapTile mapTile = mapTiles[Random.Range(0, mapTiles.Count)];
                    GameObject gameObject = Instantiate(aiEntry.prefab, (Vector2)mapTile.gridPosition, new Quaternion(0, 0, 0, 0));
                    gameObject.GetComponent<AIMovement>().target = tempTarget;

                    aiEntry.spawnCount++;
                    aiEntry.TimeUntillNextSpawn = GameTime.time + aiEntry.spawnCooldown;
                }
            }
        }
    }
}

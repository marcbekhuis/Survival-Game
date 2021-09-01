using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private AIEntry[] aiEntries;

    [System.Serializable]
    public class AIEntry 
    {
        public GameObject prefab;
        public float spawnCooldown = 60;
        public int maxSpawnCount = 1;
        public float minDistanceFromPlayer = 10;
        public float maxDistanceFromPlayer = 40;
        public Biome[] spawnBiomes;

        [HideInInspector] public float TimeUntillNextSpawn = 0;
        [HideInInspector][Tooltip("Spawn count for 100 X 100 area")] public int spawnCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GamePaused) return;

        foreach (var aiEntry in aiEntries)
        {
            if (aiEntry.spawnCount < aiEntry.maxSpawnCount * MapData.instance.sizeMultiplier)
            {
                if (GameTime.time > aiEntry.TimeUntillNextSpawn)
                {
                    List<MapTile> mapTiles = new List<MapTile>();
                    foreach (var item in MapData.instance.mapTilesFilterByBiomeNoResource)
                    {
                        mapTiles.AddRange(item.Value);
                    }

                    if (mapTiles.Count <= 0) return;
                    for (int i = 0; i < 100; i++)
                    {
                        MapTile mapTile = mapTiles[Random.Range(0, mapTiles.Count)];
                        float distance = Vector2.Distance((Vector2)player.position, mapTile.gridPosition);
                        if (distance < aiEntry.minDistanceFromPlayer || distance > aiEntry.maxDistanceFromPlayer) continue;

                        GameObject gameObject = Instantiate(aiEntry.prefab, (Vector2)mapTile.gridPosition + new Vector2(0.5f, 0), new Quaternion(0, 0, 0, 0));
                        gameObject.GetComponent<AIMovement>().target = player;

                        aiEntry.spawnCount++;
                        aiEntry.TimeUntillNextSpawn = GameTime.time + aiEntry.spawnCooldown;
                        break;
                    }
                }
            }
        }
    }
}

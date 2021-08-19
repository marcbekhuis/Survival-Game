using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : Singleton<PlayerSpawn>
{
    [SerializeField] private Biome[] spawnBiomes;
    [SerializeField] private int distanceFromBorder = 10;
    [SerializeField] private Transform player;
    public Vector2 spawnPosition = new Vector2(-1, -1);

    private Vector2[] SpawnPositions()
    {
        List<Vector2> positions = new List<Vector2>();
        
        foreach (var biomeCluster in MapData.instance.biomeClusters)
        {
            bool infalid = true;
            foreach (var spawnbiome in spawnBiomes)
            {
                if(biomeCluster.biome == spawnbiome)
                {
                    infalid = false;
                    break;
                }
            }

            if (infalid) continue;

            foreach (var maptile in biomeCluster.mapTiles)
            {
                if (maptile.gridPosition.x < distanceFromBorder || maptile.gridPosition.y < distanceFromBorder) continue;
                if (maptile.gridPosition.x > MapData.instance.mapSize.x - 1 - distanceFromBorder || maptile.gridPosition.y > MapData.instance.mapSize.y - 1 - distanceFromBorder) continue;
                positions.Add(maptile.gridPosition);
            }
        }

        return positions.ToArray();
    }

    public void SpawnPlayer()
    {
        if (spawnPosition != new Vector2(-1,-1))
        {
            player.position = spawnPosition;
            return;
        }

        Vector2[] positions = SpawnPositions();
        player.position = positions[Random.Range(0, positions.Length)] + new Vector2(0.5f, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BiomeCluster
{
    public Biome biome;
    public List<MapTile> mapTiles;

    public BiomeCluster(Biome biome, List<MapTile> mapTiles)
    {
        this.biome = biome;
        this.mapTiles = mapTiles;
        MapData.instance.biomeClusters.Add(this);
    }


    public void AddTileToCluster(MapTile mapTile)
    {
        if (!mapTiles.Contains(mapTile))
        {
            mapTiles.Add(mapTile);
            mapTile.biomeCluster = this;
        }
    }
    public void RemoveTileFromCluster(MapTile mapTile)
    {
        if (mapTiles.Contains(mapTile))
        {
            mapTiles.Remove(mapTile);
            if(mapTile.biomeCluster == this) mapTile.biomeCluster = null;
        }
    }

    public void MergeCluster(BiomeCluster biomeCluster)
    {
        while (biomeCluster.mapTiles.Count > 0)
        {
            MapTile mapTile = biomeCluster.mapTiles[0];
            biomeCluster.RemoveTileFromCluster(mapTile);
            AddTileToCluster(mapTile);
        }
        MapData.instance.biomeClusters.Remove(biomeCluster);
    }
}

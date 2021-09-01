using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BiomeCluster
{
    public Biome biome;
    public List<MapTile> mapTiles;
    public Dictionary<BiomeCluster, List<MapTile>> neighbours = new Dictionary<BiomeCluster, List<MapTile>>();

    public BiomeCluster(Biome biome, List<MapTile> mapTiles)
    {
        this.biome = biome;
        this.mapTiles = mapTiles;
        MapData.instance.biomeClusters.Add(this);
    }

    public bool SameBiome(Biome biome)
    {
        return biome == this.biome;
    }

    public void AddNeighbour(BiomeCluster neighbour, MapTile mapTile)
    {
        if (neighbours.ContainsKey(neighbour))
        {
            if (!neighbours[neighbour].Contains(mapTile)) neighbours[neighbour].Add(mapTile);
        }
        else neighbours.Add(neighbour, new List<MapTile> { mapTile });
    }

    public void AddMaptile(MapTile mapTile)
    {
        if (!mapTiles.Contains(mapTile)) mapTiles.Add(mapTile);
        mapTile.biomeCluster = this;
    }

    public void RemoveNeighbour(BiomeCluster neighbour, MapTile mapTile)
    {
        if (neighbours.ContainsKey(neighbour))
        {
            if (neighbours[neighbour].Contains(mapTile))
            {
                neighbours[neighbour].Remove(mapTile);
                if (neighbours[neighbour].Count <= 0) neighbours.Remove(neighbour);
            }
        }
    }

    public void RemoveMaptile(MapTile mapTile)
    {
        if (mapTiles.Contains(mapTile)) mapTiles.Remove(mapTile);
        mapTile.biomeCluster = null;

        if (mapTiles.Count <= 0) MapData.instance.biomeClusters.Remove(this);
    }

    public void Merge(BiomeCluster biomeCluster)
    {
        foreach (var mapTile in biomeCluster.mapTiles)
        {
            AddMaptile(mapTile);
        }

        foreach (var neighbour in biomeCluster.neighbours)
        {
            foreach (var mapTile in neighbour.Value)
            {
                AddNeighbour(neighbour.Key, mapTile);
            }
        }
    }

    public bool RequiredBiomeNeighbour()
    {
        if (biome.requiredBiomeNeighbours.Length <= 0) return true;

        int requiredNeighbourCount = 0;

        foreach (var requiredBiomeNeighbour in biome.requiredBiomeNeighbours)
        {
            foreach (var neighbour in neighbours)
            {
                if (requiredBiomeNeighbour == neighbour.Key.biome)
                {
                    requiredNeighbourCount++;
                    break;
                }
            }
        }

        return requiredNeighbourCount >= biome.requiredBiomeNeighbours.Length;
    }

    public void GetMinMax(out int minX, out int maxX, out int minY, out int maxY)
    {
        minX = mapTiles[0].gridPosition.x;
        maxX = mapTiles[0].gridPosition.x;
        minY = mapTiles[0].gridPosition.y;
        maxY = mapTiles[0].gridPosition.y;

        foreach (var mapTile in mapTiles)
        {
            if (mapTile.gridPosition.x < minX) minX = mapTile.gridPosition.x;
            if (mapTile.gridPosition.x > maxX) maxX = mapTile.gridPosition.x;
            if (mapTile.gridPosition.y < minY) minY = mapTile.gridPosition.y;
            if (mapTile.gridPosition.y > maxY) maxY = mapTile.gridPosition.y;
        }
    }
}

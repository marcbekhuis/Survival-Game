using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile
{
    public Resource resource;
    public bool blocked;
    private Biome biomeVar;
    public float wetness;
    public float height;
    public float tempature;
    public Vector2Int gridPosition;
    public float waterLevel;
    public float seaLevel;
    public BiomeCluster biomeCluster;

    public MapTile(Biome biome, Resource resource, Vector2Int gridPosition, float height, float tempature, float wetness, float waterLevel, float seaLevel, bool blocked = false)
    {
        biomeCluster = null;
        this.biome = biome;
        this.resource = resource;
        this.gridPosition = gridPosition;
        this.height = height;
        this.tempature = tempature;
        this.wetness = wetness;
        this.waterLevel = waterLevel;
        this.blocked = blocked;
        this.seaLevel = seaLevel;
    }

    public Biome biome
    {
        get => biomeVar;
        set
        {
            if (value == biomeVar) return;
            biomeVar = value;
            if (biomeCluster != null) LeaveBiomeCluster();
            if (biomeVar) SetBiomeCluster();
        }
    }

    public void LeaveBiomeCluster()
    {
        biomeCluster.RemoveTileFromCluster(this);
        biomeCluster = null;
    }

    public void SetBiomeCluster()
    {
        if (gridPosition.y > 0) AddCluster(MapData.instance.mapTiles[gridPosition.x, gridPosition.y - 1]);
        if (gridPosition.x > 0) AddCluster(MapData.instance.mapTiles[gridPosition.x - 1, gridPosition.y]);
        if (gridPosition.x < MapData.instance.mapSize.x - 1) AddCluster(MapData.instance.mapTiles[gridPosition.x + 1, gridPosition.y]);
        if (gridPosition.y < MapData.instance.mapSize.y - 1) AddCluster(MapData.instance.mapTiles[gridPosition.x, gridPosition.y + 1]);
        if (biomeCluster == null) biomeCluster = new BiomeCluster(biome, new List<MapTile> { this });
    }

    private void AddCluster(MapTile mapTile)
    {
        if (mapTile.biomeCluster == null) return;
        if (mapTile.biome == biomeVar)
        {
            if (biomeCluster == null) mapTile.biomeCluster.AddTileToCluster(this);
            else if (biomeCluster != mapTile.biomeCluster) mapTile.biomeCluster.MergeCluster(biomeCluster);
        }
    }

    public WalkableType IsWalkable
    {
        get
        {
            if (resource)
            {
                if (resource.IsWalkable == WalkableType.Blocked)
                {
                    return WalkableType.Blocked;
                }
            }
            if (biome.isWalkable == WalkableType.Blocked)
            {
                return WalkableType.Blocked;
            }
            if (resource)
            {
                if (resource.IsWalkable == WalkableType.Semi)
                {
                    return WalkableType.Semi;
                }
            }
            if (biome.isWalkable == WalkableType.Semi)
            {
                return WalkableType.Semi;
            }
            return WalkableType.Walkable;
        }
    }
}

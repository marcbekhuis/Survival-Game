using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile
{
    private Resource resourceVar;
    
    public Vector2Int gridPosition;
    public MapData mapdata;

    public float height;
    public float heightFactor;

    public float tempature;
    public float tempatureFactor;

    public float wetness;
    public float wetnessFactor;

    public float oceanLevel;

    private Biome biomeVar;

    public BiomeCluster biomeCluster;

    public MapTile neighbourUp;
    public MapTile neighbourUpLeft;
    public MapTile neighbourUpRight;
    public MapTile neighbourDown;
    public MapTile neighbourDownLeft;
    public MapTile neighbourDownRight;
    public MapTile neighbourLeft;
    public MapTile neighbourRight;

    public MapTile(Vector2Int gridPosition, MapData mapdata, float height, float heightFactor, float tempature, float tempatureFactor, float wetness, float wetnessFactor, float oceanLevel)
    {
        this.gridPosition = gridPosition;
        this.mapdata = mapdata;
        this.height = height;
        this.heightFactor = heightFactor;
        this.tempature = tempature;
        this.tempatureFactor = tempatureFactor;
        this.wetness = wetness;
        this.wetnessFactor = wetnessFactor;
        this.oceanLevel = oceanLevel;
        SetNeighbours();
    }

    public MapTile(int x, int y, MapData mapdata, float height, float heightFactor, float tempature, float tempatureFactor, float wetness, float wetnessFactor, float oceanLevel)
    {
        this.gridPosition = new Vector2Int(x,y);
        this.mapdata = mapdata;
        this.height = height;
        this.heightFactor = heightFactor;
        this.tempature = tempature;
        this.tempatureFactor = tempatureFactor;
        this.wetness = wetness;
        this.wetnessFactor = wetnessFactor;
        this.oceanLevel = oceanLevel;
        SetNeighbours();
    }

    public void SetNeighbours()
    {
        if (gridPosition.x - 1 >= 0)
        {
            // Left
            neighbourLeft = mapdata.GetGridCell(gridPosition.x - 1, gridPosition.y);
            if (neighbourLeft != null) neighbourLeft.neighbourRight = this;
            // Left Down
            if (gridPosition.y - 1 >= 0)
            {
                neighbourDownLeft = mapdata.GetGridCell(gridPosition.x - 1, gridPosition.y - 1);
                if (neighbourDownLeft != null) neighbourDownLeft.neighbourUpRight = this;
            }
            // Left Up
            if (gridPosition.y + 1 < mapdata.Size.y)
            {
                neighbourUpLeft = mapdata.GetGridCell(gridPosition.x - 1, gridPosition.y + 1);
                if (neighbourUpLeft != null) neighbourUpLeft.neighbourDownRight = this;
            }
        }
        if (gridPosition.x + 1 < mapdata.Size.x)
        {
            // Right
            neighbourRight = mapdata.GetGridCell(gridPosition.x + 1, gridPosition.y);
            if(neighbourRight != null) neighbourRight.neighbourLeft = this;
            // Right Down
            if (gridPosition.y - 1 >= 0)
            {
                neighbourDownRight = mapdata.GetGridCell(gridPosition.x + 1, gridPosition.y - 1);
                if (neighbourDownRight != null) neighbourDownRight.neighbourUpLeft = this;
            }
            // Right Up
            if (gridPosition.y + 1 < mapdata.Size.y)
            {
                neighbourUpRight = mapdata.GetGridCell(gridPosition.x + 1, gridPosition.y + 1);
                if (neighbourUpRight != null) neighbourUpRight.neighbourDownLeft = this;
            }
        }
        // Down
        if (gridPosition.y - 1 >= 0)
        {
            neighbourDown = mapdata.GetGridCell(gridPosition.x, gridPosition.y - 1);
            if (neighbourDown != null) neighbourDown.neighbourUp = this;
        }
        // Up
        if (gridPosition.y + 1 < mapdata.Size.y)
        {
            neighbourUp = mapdata.GetGridCell(gridPosition.x, gridPosition.y + 1);
            if (neighbourUp != null) neighbourUp.neighbourDown = this;
        }
    }

    public Biome biome
    {
        get
        {
            return biomeVar;
        }

        set
        {
            if (biome != null)
            {
                if (mapdata.mapTilesFilterByBiome.ContainsKey(biome))
                {
                    if (mapdata.mapTilesFilterByBiome[biome].Contains(this))
                        mapdata.mapTilesFilterByBiome[biome].Remove(this);
                }
                if (mapdata.mapTilesFilterByBiomeNoResource.ContainsKey(biome))
                {
                    if (mapdata.mapTilesFilterByBiomeNoResource[biome].Contains(this))
                        mapdata.mapTilesFilterByBiomeNoResource[biome].Remove(this);
                }
            }

            biomeVar = value;

            if (biomeVar == null) return;

            if (mapdata.mapTilesFilterByBiomeNoResource.ContainsKey(biome))
            {
                if (!mapdata.mapTilesFilterByBiomeNoResource[biome].Contains(this)) mapdata.mapTilesFilterByBiomeNoResource[biome].Add(this);
            }
            else mapdata.mapTilesFilterByBiomeNoResource.Add(biome, new List<MapTile> { this });

            if (mapdata.mapTilesFilterByBiome.ContainsKey(biome))
            {
                if (!mapdata.mapTilesFilterByBiome[biome].Contains(this)) mapdata.mapTilesFilterByBiome[biome].Add(this);
            }
            else mapdata.mapTilesFilterByBiome.Add(biome, new List<MapTile> { this });

            if (biomeCluster != null)
            {
                RemoveNeighbourBiomeCluster(neighbourUp);
                RemoveNeighbourBiomeCluster(neighbourDown);
                RemoveNeighbourBiomeCluster(neighbourLeft);
                RemoveNeighbourBiomeCluster(neighbourRight);

                biomeCluster.RemoveMaptile(this);
            }

            AddNeighbourBiomeCluster(neighbourUp);
            AddNeighbourBiomeCluster(neighbourDown);
            AddNeighbourBiomeCluster(neighbourLeft);
            AddNeighbourBiomeCluster(neighbourRight);

            if (biomeCluster == null) biomeCluster = new BiomeCluster(biome, new List<MapTile> { this });
        }
    }

    public Resource resource 
    { 
        get
        {
            return resourceVar;
        }
        set
        {
            if (value)
            {
                if (mapdata.mapTilesFilterByBiomeNoResource.ContainsKey(biome))
                {
                    if (mapdata.mapTilesFilterByBiomeNoResource[biome].Contains(this))
                        mapdata.mapTilesFilterByBiomeNoResource[biome].Remove(this);
                }
            }
            else
            {
                if (mapdata.mapTilesFilterByBiomeNoResource.ContainsKey(biome))
                {
                    if (!mapdata.mapTilesFilterByBiomeNoResource[biome].Contains(this))
                        mapdata.mapTilesFilterByBiomeNoResource[biome].Add(this);
                }
                else mapdata.mapTilesFilterByBiomeNoResource.Add(biome, new List<MapTile> { this });
            }

            resourceVar = value;
        }
    }

    private void AddNeighbourBiomeCluster(MapTile neighbourTile)
    {
        if (neighbourTile != null)
        {
            if (neighbourTile.biomeCluster != null)
            {
                if (neighbourTile.biomeCluster.biome == biome)
                {
                    if (biomeCluster != null)
                    {
                        if (biomeCluster != neighbourTile.biomeCluster)
                        {
                            if (biomeCluster.mapTiles.Count > neighbourTile.biomeCluster.mapTiles.Count) biomeCluster.Merge(neighbourTile.biomeCluster);
                            else neighbourTile.biomeCluster.Merge(biomeCluster);
                        }
                    }
                    else neighbourTile.biomeCluster.AddMaptile(this);
                }
                else if (biomeCluster != null)
                {
                    neighbourTile.biomeCluster.AddNeighbour(biomeCluster, this);
                    biomeCluster.AddNeighbour(neighbourTile.biomeCluster, neighbourTile);
                }
            }
        }
    }

    private void RemoveNeighbourBiomeCluster(MapTile neighbourTile)
    {
        if (neighbourTile != null)
        {
            if (neighbourTile.biomeCluster != null)
            {
                if (biomeCluster != null)
                {
                    neighbourTile.biomeCluster.RemoveNeighbour(biomeCluster, this);
                    biomeCluster.RemoveNeighbour(neighbourTile.biomeCluster, neighbourTile);
                }
            }
        }
    }

    public Biome MostCommonBiomeNeighbour()
    {
        if (!biome) return null;

        Dictionary<Biome, int> biomes = new Dictionary<Biome, int>();

        if (neighbourUp != null)
        {
            if (biomes.ContainsKey(neighbourUp.biome)) biomes[neighbourUp.biome]++;
            else biomes.Add(neighbourUp.biome, 1);
        }

        if (neighbourDown != null)
        {
            if (biomes.ContainsKey(neighbourDown.biome)) biomes[neighbourDown.biome]++;
            else biomes.Add(neighbourDown.biome, 1);
        }

        if (neighbourLeft != null)
        {
            if (biomes.ContainsKey(neighbourLeft.biome)) biomes[neighbourLeft.biome]++;
            else biomes.Add(neighbourLeft.biome, 1);
        }

        if (neighbourRight != null) 
        {
            if (biomes.ContainsKey(neighbourRight.biome)) biomes[neighbourRight.biome]++;
            else biomes.Add(neighbourRight.biome, 1);
        }

        Biome commonBiome = null;
        int commonAmount = 0;

        foreach (var biome in biomes)
        {
            if(commonBiome == null || biome.Value > commonAmount)
            {
                commonBiome = biome.Key;
                commonAmount = biome.Value;
            }
        }

        return commonBiome;
    }

    public Biome MostCommonBiomeNeighbourNotThisBiome()
    {
        if (!biome) return null;

        Dictionary<Biome, int> biomes = new Dictionary<Biome, int>();

        if (neighbourUp != null)
        {
            if (!neighbourUp.biome) return null;
            if (neighbourUp.biome != biome)
            {
                if (biomes.ContainsKey(neighbourUp.biome)) biomes[neighbourUp.biome]++;
                else biomes.Add(neighbourUp.biome, 1);
            }
        }

        if (neighbourDown != null)
        {
            if (!neighbourDown.biome) return null;
            if (neighbourDown.biome != biome)
            {
                if (biomes.ContainsKey(neighbourDown.biome)) biomes[neighbourDown.biome]++;
                else biomes.Add(neighbourDown.biome, 1);
            }
        }

        if (neighbourLeft != null)
        {
            if (!neighbourLeft.biome) return null;
            if (neighbourLeft.biome != biome)
            {
                if (biomes.ContainsKey(neighbourLeft.biome)) biomes[neighbourLeft.biome]++;
                else biomes.Add(neighbourLeft.biome, 1);
            }
        }

        if (neighbourRight != null)
        {
            if (!neighbourRight.biome) return null;
            if (neighbourRight.biome != biome)
            {
                if (biomes.ContainsKey(neighbourRight.biome)) biomes[neighbourRight.biome]++;
                else biomes.Add(neighbourRight.biome, 1);
            }
        }

        Biome commonBiome = null;
        int commonAmount = 0;

        foreach (var biome in biomes)
        {
            if (commonBiome == null || biome.Value > commonAmount)
            {
                commonBiome = biome.Key;
                commonAmount = biome.Value;
            }
        }

        return commonBiome;
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
            if (biome)
            {
                if (biome.isWalkable == WalkableType.Blocked)
                {
                    return WalkableType.Blocked;
                }
            }
            if (resource)
            {
                if (resource.IsWalkable == WalkableType.Semi)
                {
                    return WalkableType.Semi;
                }
            }
            if (biome)
            {
                if (biome.isWalkable == WalkableType.Semi)
                {
                    return WalkableType.Semi;
                }
            }
            return WalkableType.Walkable;
        }
    }
}

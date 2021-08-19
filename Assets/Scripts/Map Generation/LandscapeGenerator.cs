using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LandscapeGenerator : MonoBehaviour
{
    [Header("Landscape Settings")]
    [SerializeField] private float maxHeight = 100;
    [SerializeField] private float seaLevel = 40;
    [SerializeField] private float minTempature = -50;
    [SerializeField] private float maxTempature = 50;
    [Space]
    [Header("References")]
    [SerializeField] private Tilemap landscapeTilemap;
    [SerializeField] private Biome[] biomes;
    [SerializeField] private Biome errorBiome;
    [Space]
    [Header("Noise Settings")]
    [SerializeField] private HeightNoise heightNoise = new HeightNoise();
    [SerializeField] private WetnessNoise wetnessNoise = new WetnessNoise();
    [SerializeField] private TempatureNoise tempatureNoise = new TempatureNoise();

    private Vector2Int mapSize;

    public IEnumerator MakeLandscape(Vector2Int mapSize)
    {
        this.mapSize = mapSize;
        if (biomes.Length <= 0)
        {
            //GameGenerator.updateLoadText.Invoke("No biomes in biome list");
            Debug.LogError("No biomes in biome list");
            yield break;
        }
        LoadingScreen.instance.UpdateProgress(0, "Generating landscape");
        yield return StartCoroutine(GenerateLandscape());

        LoadingScreen.instance.UpdateProgress(0, "Generating biomes");
        yield return StartCoroutine(GenerateBiomes());

        LoadingScreen.instance.UpdateProgress(0, "Filtering biome sizes");
        yield return StartCoroutine(FilterBiomeClustersSize());

        LoadingScreen.instance.UpdateProgress(0, "Placing ground tiles");
        yield return StartCoroutine(PlaceTiles());
    }

    public IEnumerator GenerateLandscape()
    {
        heightNoise.Setup();
        wetnessNoise.Setup();
        tempatureNoise.Setup();

        System.DateTime loopStartTime = System.DateTime.Now;
        int loops = 0;
        int lastLoop = mapSize.x * mapSize.y;
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                GenerateLandscapeTile(x, y);
                loops++;
                if ((System.DateTime.Now - loopStartTime).TotalSeconds >= 1)
                {
                    LoadingScreen.instance.UpdateProgress((float)loops / (float)lastLoop, "Generating landscape");
                    yield return null;
                    loopStartTime = System.DateTime.Now;
                }
            }
        }
    }

    public void GenerateLandscapeTile(int x, int y)
    {
        float height = heightNoise.GetValue(x, y, maxHeight);
        float wetness = wetnessNoise.GetValue(x, y);
        float tempature = tempatureNoise.GetValue(x, y, minTempature, maxTempature);
        float seaLevel = this.seaLevel - height;
        float waterLevel = 0;

        MapData.instance.mapTiles[x, y] = new MapTile(null, null, new Vector2Int(x, y), height, tempature, wetness, waterLevel, seaLevel);
    }

    public IEnumerator GenerateBiomes()
    {
        System.DateTime loopStartTime = System.DateTime.Now;
        int loops = 0;
        int lastLoop = mapSize.x * mapSize.y;
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                GenerateBiome(x, y);
                loops++;
                if ((System.DateTime.Now - loopStartTime).TotalSeconds >= 1)
                {
                    LoadingScreen.instance.UpdateProgress((float)loops / (float)lastLoop, "Generating biomes");
                    yield return null;
                    loopStartTime = System.DateTime.Now;
                }
            }
        }
    }

    public void GenerateBiome(int x, int y)
    {
        MapTile mapTile = MapData.instance.mapTiles[x, y];
        List<Biome> AllowedBiomes = new List<Biome>();

        foreach (var biome in biomes)
        {
            if (mapTile.height >= biome.minLandHeight && mapTile.height <= biome.maxLandHeight)
            {
                if (mapTile.wetness >= biome.minWetness && mapTile.wetness <= biome.maxWetness)
                {
                    if (mapTile.tempature >= biome.minTempature && mapTile.tempature <= biome.maxTempature)
                    {
                        if (mapTile.seaLevel >= biome.minSeaLevel && mapTile.seaLevel <= biome.maxSeaLevel)
                        {
                            if (mapTile.waterLevel >= biome.minWaterLevel && mapTile.waterLevel <= biome.maxWaterLevel)
                            {
                                AllowedBiomes.Add(biome);
                            }
                        }
                    }
                }
            }
        }

        if (AllowedBiomes.Count <= 0)
        {
            AllowedBiomes.Add(errorBiome);
            Debug.LogError("Tile at position: x:" + x + ", y:" + y + " Had to be given a error biome. Height: " + mapTile.height + " Wetness: " + mapTile.wetness + " Tempature: " + mapTile.tempature + " WaterLevel: " + mapTile.seaLevel);
        }
        mapTile.biome = AllowedBiomes[Random.Range(0, AllowedBiomes.Count)];
    }

    public IEnumerator FilterBiomeClustersSize()
    {
        System.DateTime loopStartTime = System.DateTime.Now;
        for (int a = 0; a < MapData.instance.biomeClusters.Count; a++)
        {
            int length = MapData.instance.biomeClusters.Count;
            BiomeCluster biomeCluster = MapData.instance.biomeClusters[a];

            if (biomeCluster.mapTiles.Count >= biomeCluster.biome.minSize) continue;
            for (int i = 0; i < biomeCluster.mapTiles.Count; i++)
            {
                int length2 = biomeCluster.mapTiles.Count;
                FilterBiomeCluster(biomeCluster.mapTiles[i]);
                if (biomeCluster.mapTiles.Count < length2) i--;
                if ((System.DateTime.Now - loopStartTime).TotalSeconds >= 1)
                {
                    LoadingScreen.instance.UpdateProgress((float)i / (float)length2, "Filtering biome sizes");
                    yield return null;
                    loopStartTime = System.DateTime.Now;
                }
            }

            if (MapData.instance.biomeClusters.Count < length) a--;
        }
    }

    public void FilterBiomeCluster(MapTile mapTile)
    {
        Dictionary<Biome, int> neighbours = new Dictionary<Biome, int>();

        if (mapTile.gridPosition.x > 0)
        {
            Biome biome = MapData.instance.mapTiles[mapTile.gridPosition.x - 1, mapTile.gridPosition.y].biome;
            if (neighbours.ContainsKey(biome)) neighbours[biome]++;
            else neighbours.Add(biome, 1);
        }
        if (mapTile.gridPosition.x < mapSize.x - 1)
        {
            Biome biome = MapData.instance.mapTiles[mapTile.gridPosition.x + 1, mapTile.gridPosition.y].biome;
            if (neighbours.ContainsKey(biome)) neighbours[biome]++;
            else neighbours.Add(biome, 1);
        }

        if (mapTile.gridPosition.y > 0)
        {
            Biome biome = MapData.instance.mapTiles[mapTile.gridPosition.x, mapTile.gridPosition.y - 1].biome;
            if (neighbours.ContainsKey(biome)) neighbours[biome]++;
            else neighbours.Add(biome, 1);
        }
        if (mapTile.gridPosition.y < mapSize.y - 1)
        {
            Biome biome = MapData.instance.mapTiles[mapTile.gridPosition.x, mapTile.gridPosition.y + 1].biome;
            if (neighbours.ContainsKey(biome)) neighbours[biome]++;
            else neighbours.Add(biome, 1);
        }

        Biome newBiome = errorBiome;
        int sides = 0;
        foreach (var item in neighbours)
        {
            if (sides < item.Value)
            {
                newBiome = item.Key;
                sides = item.Value;
            }
        }
        mapTile.biome = newBiome;
    }

    public IEnumerator PlaceTiles()
    {
        if (!landscapeTilemap)
        {
            Debug.LogError("Tilemap not set");
            yield break;
        }

        landscapeTilemap.ClearAllTiles();
        yield return null;

        System.DateTime loopStartTime = System.DateTime.Now;
        int loops = 0;
        int lastLoop = mapSize.x * mapSize.y;
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                PlaceTile(x, y);
                loops++;
                if ((System.DateTime.Now - loopStartTime).TotalSeconds >= 1)
                {
                    LoadingScreen.instance.UpdateProgress((float)loops / (float)lastLoop, "Placing ground tiles");
                    yield return null;
                    loopStartTime = System.DateTime.Now;
                }
            }
        }
    }

    public void PlaceTile(int x, int y)
    {
        MapTile mapTile = MapData.instance.mapTiles[x, y];
        AddBiomeFilteredMapTile(mapTile);
        if (mapTile.biome)
        {
            if (mapTile.biome.tile)
            {
                landscapeTilemap.SetTile(new Vector3Int(x, y, 0), mapTile.biome.tile);
            }
            else
            {
                Debug.LogError("Tile at position: x:" + x + ", y:" + y + " has a biome, with no tile.");
            }
        }
        else
        {
            Debug.LogError("Tile at position: x:" + x + ", y:" + y + " has no biome");
        }
    }

    private void AddBiomeFilteredMapTile(MapTile mapTile)
    {
        if (MapData.instance.mapTilesFilterByBiome.ContainsKey(mapTile.biome))
            MapData.instance.mapTilesFilterByBiome[mapTile.biome].Add(mapTile);
        else MapData.instance.mapTilesFilterByBiome.Add(mapTile.biome, new List<MapTile> { mapTile });
    }
}

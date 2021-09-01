using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LandscapeGenerator : MonoBehaviour
{
    [SerializeField] private float height;
    [SerializeField] private float wetness;
    [SerializeField] private float tempature;
    [SerializeField] private float oceanLevel;
    [Space]
    [SerializeField] private Biome[] biomes;
    [Space]
    [SerializeField] private TilePlacing tilePlacing;
    [SerializeField] private NoiseGenerator heightNoise;
    [SerializeField] private NoiseGenerator wetnessNoise;
    [SerializeField] private NoiseGenerator tempatureNoise;

    public IEnumerator MakeLandscape()
    {
        heightNoise.Setup();
        wetnessNoise.Setup();
        tempatureNoise.Setup();
        yield return StartCoroutine(GenerateLandscape());
        yield return StartCoroutine(FilterBiomeSize());
        yield return StartCoroutine(FilterRequiredBiome());
        if (tilePlacing != null) yield return StartCoroutine(tilePlacing.PlaceTiles());
    }

    IEnumerator GenerateLandscape()
    {
        System.DateTime loopStartTime = System.DateTime.Now;

        int loops = 0;
        int lastLoop = MapData.instance.Size.x * MapData.instance.Size.y;

        for (int x = 0; x < MapData.instance.Size.x; x++)
        {
            for (int y = 0; y < MapData.instance.Size.y; y++)
            {
                float heightFactor = (heightNoise.GetValue(x, y) + 1) / 2f;
                float height = heightFactor * this.height;

                float tempatureFactor = tempatureNoise.GetValue(x, y);
                float tempature = tempatureFactor * this.tempature;

                float wetnessFactor = (wetnessNoise.GetValue(x, y) + 1) / 2f;
                float wetness = wetnessFactor * this.wetness;

                float oceanLevel = this.oceanLevel - height;

                MapTile mapTile = MapData.instance.SetGridCell(x, y, new MapTile(x, y, MapData.instance, height, heightFactor, tempature, tempatureFactor, wetness, wetnessFactor, oceanLevel));

                List<Biome> validBiomes = new List<Biome>();

                foreach (var biome in biomes)
                {
                    if (!biome.HeightValid(height)) continue;
                    if (!biome.TempatureValid(tempature)) continue;
                    if (!biome.WetnessValid(wetness)) continue;
                    if (!biome.OceanLevelValid(oceanLevel)) continue;
                    validBiomes.Add(biome);
                }

                if (validBiomes.Count > 0) mapTile.biome = validBiomes[Random.Range(0, validBiomes.Count)];

                loops++;
                if ((System.DateTime.Now - loopStartTime).TotalSeconds >= 0.033333f)
                {
                    LoadingScreen.instance.UpdateProgress((float)loops / (float)lastLoop, "Generating landscape");
                    yield return null;
                    loopStartTime = System.DateTime.Now;
                }
            }
        }
    }

    IEnumerator FilterBiomeSize()
    {
        System.DateTime loopStartTime = System.DateTime.Now;

        int clusters = MapData.instance.biomeClusters.Count;

        int loops = 0;
        int lastLoop = MapData.instance.biomeClusters.Count;

        for (int i = 0; i < MapData.instance.biomeClusters.Count; i++)
        {
            BiomeCluster biomeCluster = MapData.instance.biomeClusters[i];

            if (biomeCluster.mapTiles.Count >= biomeCluster.biome.minSize) continue;

            int minX;
            int maxX;
            int minY;
            int maxY;

            biomeCluster.GetMinMax(out minX, out maxX, out minY, out maxY);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    MapTile mapTile = MapData.instance.GetGridCell(x, y);

                    if (mapTile.biomeCluster != biomeCluster) continue;

                    mapTile.biome = mapTile.MostCommonBiomeNeighbourNotThisBiome();
                }
            }

            if (MapData.instance.biomeClusters.Count < clusters)
            {
                clusters--;
                i--;
            }

            loops++;
            if ((System.DateTime.Now - loopStartTime).TotalSeconds >= 0.033333f)
            {
                LoadingScreen.instance.UpdateProgress((float)loops / (float)lastLoop, "Filter biome size");
                yield return null;
                loopStartTime = System.DateTime.Now;
            }
        }
    }

    IEnumerator FilterRequiredBiome()
    {
        System.DateTime loopStartTime = System.DateTime.Now;

        int clusters = MapData.instance.biomeClusters.Count;

        int loops = 0;
        int lastLoop = MapData.instance.biomeClusters.Count;

        for (int i = 0; i < MapData.instance.biomeClusters.Count; i++)
        {
            BiomeCluster biomeCluster = MapData.instance.biomeClusters[i];

            if (biomeCluster.RequiredBiomeNeighbour()) continue;

            int minX;
            int maxX;
            int minY;
            int maxY;

            biomeCluster.GetMinMax(out minX, out maxX, out minY, out maxY);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    MapTile mapTile = MapData.instance.GetGridCell(x, y);

                    if (mapTile.biomeCluster != biomeCluster) continue;

                    mapTile.biome = mapTile.MostCommonBiomeNeighbourNotThisBiome();
                }
            }

            if (MapData.instance.biomeClusters.Count < clusters)
            {
                clusters--;
                i--;
            }

            loops++;
            if ((System.DateTime.Now - loopStartTime).TotalSeconds >= 0.033333f)
            {
                LoadingScreen.instance.UpdateProgress((float)loops / (float)lastLoop, "Filter required biome");
                yield return null;
                loopStartTime = System.DateTime.Now;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private ResourceEntry[] resourceEntries;
    [SerializeField] private PlantSystem plantSystem;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap secondTilemap;

    private Dictionary<Biome, List<ResourceEntry>> resourcesFilter = new Dictionary<Biome, List<ResourceEntry>>();

    public IEnumerator GenerateResources(Vector2Int mapSize)
    {
        FilterResources();

        System.DateTime loopStartTime = System.DateTime.Now;
        int loops = 0;
        int lastLoop = mapSize.x * mapSize.y;
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                GenerateResource(new Vector2Int(x, y));
                loops++;
                if ((System.DateTime.Now - loopStartTime).TotalSeconds >= 1)
                {
                    LoadingScreen.instance.UpdateProgress((float)loops / (float)lastLoop, "Generating resources");
                    yield return null;
                    loopStartTime = System.DateTime.Now;
                }
            }
        }
    }

    private void FilterResources()
    {
        foreach (var resourceEntry in resourceEntries)
        {
            foreach (var biome in resourceEntry.biomes)
            {
                if (resourcesFilter.ContainsKey(biome)) resourcesFilter[biome].Add(resourceEntry);
                else resourcesFilter.Add(biome, new List<ResourceEntry> { resourceEntry });
            }
        }
    }

    private void GenerateResource(Vector2Int gridPosition)
    {
        MapTile mapTile = MapData.instance.mapTiles[gridPosition.x, gridPosition.y];
        if (!resourcesFilter.ContainsKey(mapTile.biome)) return;

        float random = Random.Range(0, resourcesFilter[mapTile.biome].Count * 100f);
        float lastValue = 0;
        Resource resource = null;
        for (int i = 0; i < resourcesFilter[mapTile.biome].Count; i++)
        {
            if (random <= lastValue + resourcesFilter[mapTile.biome][i].change)
            {
                resource = resourcesFilter[mapTile.biome][i].resource;
                break;
            }
            lastValue += resourcesFilter[mapTile.biome][i].change;
        }

        if (!resource)
        {
            AddBiomeFilteredMapTileNoResource(mapTile);
            return;
        }
        Plant plant = (Plant)resource;
        Plant plantedPlant = plant.Create(plant, new Tilemap[] { tilemap, secondTilemap }, gridPosition, Random.Range(0, plant.growthTime));
        mapTile.resource = plantedPlant;
        plantSystem.plantedPlants.Add(plantedPlant);
    }

    [System.Serializable]
    public class ResourceEntry
    {
        public Resource resource;
        public Biome[] biomes;
        public float change = 10;
    }

    private void AddBiomeFilteredMapTileNoResource(MapTile mapTile)
    {
        if (MapData.instance.mapTilesFilterByBiomeNoResource.ContainsKey(mapTile.biome))
            MapData.instance.mapTilesFilterByBiomeNoResource[mapTile.biome].Add(mapTile);
        else MapData.instance.mapTilesFilterByBiomeNoResource.Add(mapTile.biome, new List<MapTile> { mapTile });
    }
}

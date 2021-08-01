using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Plant", menuName = "Plant/Plant")]
public class Plant : Resource
{
    [Header("Plant")]
    public TileBase[] spritePhases;
    [Tooltip("Time in seconds")] public int growthTime = 60;
    [HideInInspector] public bool grown = false;

    protected float timeBetweenPhases = 0;
    protected int growthPhase = 0;
    [HideInInspector] public float age = 0;
    protected Tilemap tilemap;
    protected int visibleGrowthPhase = 0;

    public Plant(Plant plant, Vector2Int gridPosition, float maxHealth) : base(plant, gridPosition, maxHealth)
    {
        growthTime = 60;
    }

    public Plant(Plant plant, Tilemap tilemap, Vector2Int gridPosition) : base(plant, gridPosition, plant.maxHealth)
    {
        this.spritePhases = plant.spritePhases;
        this.growthTime = plant.growthTime;
        this.grown = false;
        this.tilemap = tilemap;
        this.timeBetweenPhases = this.growthTime / spritePhases.Length;
        growthPhase = 0;
        age = 0;
        UpdateTilemap();
    }

    public Plant(Plant plant, Tilemap tilemap, Vector2Int gridPosition, float age) : base(plant, gridPosition, plant.maxHealth)
    {
        this.spritePhases = plant.spritePhases;
        this.growthTime = plant.growthTime;
        this.grown = false;
        this.tilemap = tilemap;
        this.timeBetweenPhases = this.growthTime / spritePhases.Length;
        growthPhase = Mathf.Clamp(Mathf.FloorToInt(age / timeBetweenPhases), 0, spritePhases.Length - 1);
        this.age = age;
        UpdateTilemap();
    }

    public virtual Plant Create(Plant plant, Tilemap[] tilemaps, Vector2Int gridPosition)
    {
        return new Plant(plant, tilemaps[0], gridPosition);
    }

    public virtual Plant Create(Plant plant, Tilemap[] tilemaps, Vector2Int gridPosition, float growth)
    {
        return new Plant(plant, tilemaps[0], gridPosition, growth);
    }

    public Vector2Int GridPosition 
    {
        get => gridPosition;
    }

    public void SetGrowthPhase(int value, bool visible)
    {
        growthPhase = Mathf.Clamp(value, 0, spritePhases.Length - 1);
        if (visible) UpdateTilemap();
    }

    public void SetAge(float value, bool visible)
    {
        age = value;
        if (age >= growthTime) grown = true;
        else grown = false;
        if (age >= timeBetweenPhases * (growthPhase + 1) && growthPhase < spritePhases.Length - 1) SetGrowthPhase(growthPhase + 1, visible);
        if (visible) if (visibleGrowthPhase != growthPhase) UpdateTilemap();
    }

    public virtual void UpdateTilemap()
    {
        visibleGrowthPhase = growthPhase;
        try
        {
            tilemap.SetTile((Vector3Int)gridPosition, spritePhases[growthPhase]);
        }
        catch
        {
            Debug.LogError(growthPhase);
            Debug.LogError(spritePhases.Length);
        }
    }

    public override void RemoveFromTilemap()
    {
        tilemap.SetTile((Vector3Int)gridPosition, null);
    }

    public override void SetHealth(float value, HarvestAttack harvestAttack)
    {
        health = Mathf.Clamp(health + value, 0, maxHealth);
        if (health <= 0)
        {
            RemoveFromTilemap();
            PlantSystem.instance.plantedPlants.Remove(this);

            foreach (var randomItem in randomItems)
            {
                foreach (var inventory in harvestAttack.inventories)
                {
                    if (inventory.HasSpace() || inventory.HasItemSlotWithSpace(randomItem.item))
                    {
                        int amount = randomItem.GetAmount(harvestAttack.change);
                        inventory.AddItem(randomItem.item, amount);
                        break;
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Large Plant", menuName = "Plant/Large Plant")]
public class LargePlant : Plant
{
    [Header("Large plant")]
    public TileBase[] secondSpritePhases;
    private Tilemap secondTilemap;

    public LargePlant(LargePlant plant, Tilemap tilemap, Tilemap secondTilemap, Vector2Int gridPosition) : base(plant, gridPosition, plant.maxHealth)
    {
        this.secondSpritePhases = plant.secondSpritePhases;
        this.secondTilemap = secondTilemap;
        this.spritePhases = plant.spritePhases;
        this.growthTime = plant.growthTime;
        this.grown = false;
        this.tilemap = tilemap;
        this.timeBetweenPhases = this.growthTime / spritePhases.Length;
        SetGrowthPhase(0, true);
        SetAge(0, true);
    }

    public LargePlant(LargePlant plant, Tilemap tilemap, Tilemap secondTilemap, Vector2Int gridPosition, float age) : base(plant, gridPosition, plant.maxHealth)
    {
        this.secondSpritePhases = plant.secondSpritePhases;
        this.secondTilemap = secondTilemap;
        this.spritePhases = plant.spritePhases;
        this.growthTime = plant.growthTime;
        this.grown = false;
        this.tilemap = tilemap;
        this.timeBetweenPhases = this.growthTime / spritePhases.Length;
        growthPhase = Mathf.Clamp(Mathf.FloorToInt(age / timeBetweenPhases), 0, spritePhases.Length - 1);
        this.age = age;
        UpdateTilemap();
    }

    public override Plant Create(Plant plant, Tilemap[] tilemaps, Vector2Int gridPosition)
    {
        return new LargePlant((LargePlant)plant, tilemaps[0], tilemaps[1], gridPosition);
    }

    public override Plant Create(Plant plant, Tilemap[] tilemaps, Vector2Int gridPosition, float growth)
    {
        return new LargePlant((LargePlant)plant, tilemaps[0], tilemaps[1], gridPosition, growth);
    }

    public override void UpdateTilemap()
    {
        visibleGrowthPhase = growthPhase;
        tilemap.SetTile((Vector3Int)gridPosition, spritePhases[growthPhase]);
        if (secondSpritePhases[growthPhase] != null) secondTilemap.SetTile((Vector3Int)gridPosition + new Vector3Int(0, 1, 0), secondSpritePhases[growthPhase]);
    }

    public override void RemoveFromTilemap()
    {
        base.RemoveFromTilemap();
        secondTilemap.SetTile((Vector3Int)gridPosition + new Vector3Int(0, 1, 0), null);
    }
}

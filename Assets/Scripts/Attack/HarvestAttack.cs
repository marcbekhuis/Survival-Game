using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HarvestAttack : AttackSystem
{
    [SerializeField] private Tilemap tilemap;

    public override void Attack(Vector2 direction)
    {
        Vector2Int positionInt = Vector2Int.RoundToInt((Vector2)transform.position - new Vector2(0.5f, 0.5f));
        int radiusInt = Mathf.CeilToInt(radius);
        for (int x = positionInt.x - radiusInt; x < positionInt.x + radiusInt; x++)
        {
            for (int y = positionInt.y - radiusInt; y < positionInt.y + radiusInt; y++)
            {
                if (x < 0 || y < 0) continue;
                if (x >= MapData.instance.Size.x || y >= MapData.instance.Size.y) continue;
                if (MapData.instance.GetGridCell(x, y).resource) MapData.instance.GetGridCell(x, y).resource.SetHealth(-damage, this);
            }
        }
    }
}

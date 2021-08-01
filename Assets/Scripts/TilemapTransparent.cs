using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapTransparent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Vector2 checkMinPosition = -Vector2.one;
    [SerializeField] private Vector2 checkMaxPosition = Vector2.one;
    [SerializeField] private Color playerBehindColor;

    private Tilemap tilemap;
    private List<Vector3Int> transparentTiles = new List<Vector3Int>();

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        while (transparentTiles.Count > 0)
        {
            tilemap.SetColor(transparentTiles[0], new Color(1, 1, 1, 1));
            transparentTiles.RemoveAt(0);
        }

        Vector2Int gridStartPosition = Vector2Int.RoundToInt((Vector2)playerSprite.transform.position + checkMinPosition);
        Vector2Int gridEndPosition = Vector2Int.RoundToInt((Vector2)playerSprite.transform.position + checkMaxPosition);
        for (int x = gridStartPosition.x; x < gridEndPosition.x; x++)
        {
            for (int y = gridStartPosition.y; y < gridEndPosition.y; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                if (!tilemap.HasTile(position)) continue;
                if(!SpriteBehindSprite(tilemap.GetTile<Tile>(position).sprite, position)) continue;
                tilemap.SetTileFlags(position, TileFlags.None);
                tilemap.SetColor(position, playerBehindColor);
                transparentTiles.Add(position);
            }
        }
    }

    private bool SpriteBehindSprite(Sprite sprite, Vector3 tilePosition)
    {
        Bounds bounds = sprite.bounds;
        bounds.center = bounds.center + tilePosition;
        return playerSprite.bounds.Intersects(bounds);
    }
}

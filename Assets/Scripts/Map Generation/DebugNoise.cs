using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class DebugNoise : TilePlacing
{
    [SerializeField] private DebugType debugType = DebugType.Height;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase tileBase;

    public enum DebugType 
    {
        Height,
        Wetness,
        Tempature
    }


    public override IEnumerator PlaceTiles()
    {
        DateTime loopStartTime = DateTime.Now;

        for (int x = 0; x < MapData.instance.Size.x; x++)
        {
            for (int y = 0; y < MapData.instance.Size.y; y++)
            {
                MapTile mapTile = MapData.instance.GetGridCell(x,y);

                Vector3Int position = new Vector3Int(x, y, 0);
                tilemap.SetTile(position, tileBase);
                tilemap.SetTileFlags(position, TileFlags.LockTransform | TileFlags.KeepGameObjectRuntimeOnly | TileFlags.InstantiateGameObjectRuntimeOnly);

                switch (debugType)
                {
                    case DebugType.Height:
                        tilemap.SetColor(position, new Color(mapTile.heightFactor, mapTile.heightFactor, mapTile.heightFactor));
                        break;
                    case DebugType.Wetness:
                        tilemap.SetColor(position, new Color(mapTile.wetnessFactor, mapTile.wetnessFactor, mapTile.wetnessFactor));
                        break;
                    case DebugType.Tempature:
                        float green = 0;
                        if(mapTile.tempatureFactor <= 0.4f && mapTile.tempatureFactor >= -0.4f) green = (Mathf.Clamp(mapTile.tempatureFactor, -0.4f, 0.4f) + 0.4f) * 1.25f;
                        tilemap.SetColor(position, new Color(mapTile.tempatureFactor, green, mapTile.tempatureFactor * -1));
                        break;
                    default:
                        break;
                }

                if ((DateTime.Now - loopStartTime).TotalSeconds >= 0.033333f)
                {
                    yield return null;
                    loopStartTime = DateTime.Now;
                }
            }
        }
    }
}

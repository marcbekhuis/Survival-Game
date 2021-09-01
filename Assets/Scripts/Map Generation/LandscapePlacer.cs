using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class LandscapePlacer : TilePlacing
{
    [SerializeField] private Tilemap tilemap;

    public override IEnumerator PlaceTiles()
    {
        DateTime loopStartTime = DateTime.Now;

        int loops = 0;
        int lastLoop = MapData.instance.Size.x * MapData.instance.Size.y;

        for (int x = 0; x < MapData.instance.Size.x; x++)
        {
            for (int y = 0; y < MapData.instance.Size.y; y++)
            {
                MapTile mapTile = MapData.instance.GetGridCell(x, y);
                if (mapTile == null) continue;

                Vector3Int position = new Vector3Int(x, y, 0);
                if(mapTile.biome) if(mapTile.biome.tile) tilemap.SetTile(position, mapTile.biome.tile);

                loops++;
                if ((System.DateTime.Now - loopStartTime).TotalSeconds >= 0.033333f)
                {
                    LoadingScreen.instance.UpdateProgress((float)loops / (float)lastLoop, "Placing landscape");
                    yield return null;
                    loopStartTime = System.DateTime.Now;
                }
            }
        }
    }
}

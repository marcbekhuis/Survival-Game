using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public MapTile mapTile;
    public int x;
    public int y;

    public WalkableType isWalkable;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode cameFromNode;

    public PathNode(MapTile mapTile, int x, int y, WalkableType isWalkable)
    {
        this.mapTile = mapTile;
        this.x = x;
        this.y = y;
        this.isWalkable = isWalkable;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}

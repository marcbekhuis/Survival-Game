using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : Singleton<PathFinding>
{
    PathNode[,] grid;
    private List<PathNode> openList = new List<PathNode>();
    private List<PathNode> closedList = new List<PathNode>();

    private int MOVE_STRAIGHT_COST = 10;
    private int MOVE_DIAGONAL_COST = 14;

    private new void Awake()
    {
        base.Awake();
    }

    public void SetupPathFinding()
    {
        grid = new PathNode[MapData.instance.Size.x, MapData.instance.Size.y];
        for (int x = 0; x < MapData.instance.Size.x; x++)
        {
            for (int y = 0; y < MapData.instance.Size.y; y++)
            {
                PathNode pathNode = new PathNode(MapData.instance.GetGridCell(x, y), x, y, MapData.instance.GetGridCell(x, y).IsWalkable);
                grid[x, y] = pathNode;
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }
    }

    public List<Vector2> FindPath(Vector2Int startGridPosition, Vector2Int endGridPosition)
    {
        List<PathNode> path = FindPath(startGridPosition.x, startGridPosition.y,
            endGridPosition.x, endGridPosition.y);

        if (path == null) return null;

        List<Vector2> pathPositions = new List<Vector2>();
        foreach (var pathNode in path)
        {
            pathPositions.Add(new Vector2(pathNode.x, pathNode.y));
        }

        return pathPositions;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        if (startX < 0 || startX >= MapData.instance.Size.x)
        {
            Debug.LogError("PathFinding: Outside map!");
            return null;
        }
        if (startY < 0 || startY >= MapData.instance.Size.y)
        {
            Debug.LogError("PathFinding: Outside map!");
            return null;
        }
        if (endX < 0 || endX >= MapData.instance.Size.x)
        {
            Debug.LogError("PathFinding: Outside map!");
            return null;
        }
        if (endY < 0 || endY >= MapData.instance.Size.y)
        {
            Debug.LogError("PathFinding: Outside map!");
            return null;
        }

        PathNode startNode = grid[startX, startY];
        PathNode endNode = grid[endX, endY];

        if (startNode.isWalkable == WalkableType.Blocked)
        {
            Debug.LogError("PathFinding: Start node is not walkable!");
            return null;
        }
        if (endNode.isWalkable == WalkableType.Blocked)
        {
            Debug.LogError("PathFinding: End node is not walkable!");
            return null;
        }

        foreach (var pathNode in closedList)
        {
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
        }
        foreach (var pathNode in openList)
        {
            pathNode.gCost = int.MaxValue;
            pathNode.CalculateFCost();
            pathNode.cameFromNode = null;
        }

        openList.Clear();
        openList.Add(startNode);
        closedList.Clear();

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if(currentNode == endNode)
            {
                //Final node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (neighbourNode.isWalkable != WalkableType.Walkable && neighbourNode != endNode) continue;
                if (grid[neighbourNode.x, currentNode.y].isWalkable == WalkableType.Blocked) continue;
                if (grid[currentNode.x, neighbourNode.y].isWalkable == WalkableType.Blocked) continue;

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if(tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) openList.Add(neighbourNode);
                }
            }
        }

        // Out of Nodes
        Debug.LogError("PathFinding: Ran out of nodes!");
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if(currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(grid[currentNode.x - 1, currentNode.y]);
            // Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(grid[currentNode.x - 1, currentNode.y - 1]);
            // Left Up
            if (currentNode.y + 1 < MapData.instance.Size.y) neighbourList.Add(grid[currentNode.x - 1, currentNode.y + 1]);
        }
        if(currentNode.x + 1 < MapData.instance.Size.x)
        {
            // Right
            neighbourList.Add(grid[currentNode.x + 1, currentNode.y]);
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(grid[currentNode.x + 1, currentNode.y - 1]);
            // Right Up
            if (currentNode.y + 1 < MapData.instance.Size.y) neighbourList.Add(grid[currentNode.x + 1, currentNode.y + 1]);
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(grid[currentNode.x, currentNode.y - 1]);
        // Up
        if (currentNode.y + 1 < MapData.instance.Size.y) neighbourList.Add(grid[currentNode.x, currentNode.y + 1]);

        return neighbourList;
    }

    public void PathNodeChangeIsWalkable(int x, int y, WalkableType walkableType)
    {
        grid[x, y].isWalkable = walkableType;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();

        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();

        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if(pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }
}

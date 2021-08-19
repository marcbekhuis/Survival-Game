using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Biome", menuName = "ScriptableObjects/Biome")]
public class Biome : ScriptableObject
{
    [Header("AI Movement")]
    public WalkableType isWalkable = WalkableType.Walkable;
    [Space]
    [Header("Map Generation")]
    [Tooltip("Tempature in celcius")]public float minTempature = 10;
    [Tooltip("Tempature in celcius")] public float maxTempature = 20;
    [Space]
    [Tooltip("Wetness in procentage")] [Range(0, 100)] public float minWetness = 0;
    [Tooltip("Wetness in procentage")] [Range(0, 100)] public float maxWetness = 0;
    [Space]
    public float minLandHeight = 0;
    public float maxLandHeight = 1000;
    [Space]
    public float minSeaLevel = -1000;
    public float maxSeaLevel = 0;
    [Space]
    public float minWaterLevel = -1000;
    public float maxWaterLevel = 0;
    [Space]
    public int minSize = 1;
    public Biome[] requiredNeighbourBiomes;
    [Space]
    [Header("Tilemap")]
    public TileBase tile;
}
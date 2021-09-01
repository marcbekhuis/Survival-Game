using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Biome", menuName = "ScriptableObjects/Biome")]
public class Biome : ScriptableObject
{
    public TileBase tile;
    [Header("Height")]
    [SerializeField] private float maxHeight = 100;
    [SerializeField] private float minHeight = 0;
    [Header("Wetness")]
    [SerializeField] private float maxWetness = 100;
    [SerializeField] private float minWetness = 0;
    [Header("Tempature")]
    [SerializeField] private float maxTempature = 50;
    [SerializeField] private float minTempature = -50;
    [Header("Ocean Level")]
    [SerializeField] private float maxOceanLevel = 0;
    [SerializeField] private float minOceanLevel = -100;
    [Header("AI Movement")]
    public WalkableType isWalkable = WalkableType.Walkable;
    [Header("Filter")]
    public int minSize = 1;
    public Biome[] requiredBiomeNeighbours;

    public bool TempatureValid(float tempature)
    {
        return tempature <= maxTempature && tempature >= minTempature;
    }

    public bool HeightValid(float height)
    {
        return height <= maxHeight && height >= minHeight;
    }

    public bool WetnessValid(float wetness)
    {
        return wetness <= maxWetness && wetness >= minWetness;
    }

    public bool OceanLevelValid(float oceanLevel)
    {
        return oceanLevel <= maxOceanLevel && oceanLevel >= minOceanLevel;
    }
}
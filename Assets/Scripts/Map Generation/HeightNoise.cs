using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HeightNoise
{
    [SerializeField] private FastNoise.NoiseType noiseType = FastNoise.NoiseType.PerlinFractal;
    [Space]
    [Tooltip("the number of levels of detail you want you perlin noise to have.")] [SerializeField] private int octaves = 6;
    [Tooltip("number that determines at what distance to view the noisemap. (Frequency)")] [SerializeField] private float scale = 0.02f;
    [Tooltip("Noisyness")] [SerializeField] private float gain = 0.5f;
    [Tooltip("number that determines how much detail is added or removed at each octave (adjusts frequency).")] [SerializeField] private float lacunarity = 2;
    [SerializeField] private bool randomSeed = false;
    [SerializeField] private int seed = 84304;

    private FastNoise fastNoise;

    public void Setup()
    {
        if (randomSeed) seed = UnityEngine.Random.Range(0, 10000);
        fastNoise = new FastNoise(seed);
        fastNoise.SetFractalOctaves(octaves);
        fastNoise.SetFrequency(scale);
        fastNoise.SetFractalGain(gain);
        fastNoise.SetFractalLacunarity(lacunarity);
    }

    public float GetValue(int x, int y, float maxHeight)
    {
        float rawValue = 0;

        switch (noiseType)
        {
            case FastNoise.NoiseType.Value:
                rawValue = fastNoise.GetValue(x, y);
                break;
            case FastNoise.NoiseType.ValueFractal:
                rawValue = fastNoise.GetValueFractal(x, y);
                break;
            case FastNoise.NoiseType.Perlin:
                rawValue = fastNoise.GetPerlin(x, y);
                break;
            case FastNoise.NoiseType.PerlinFractal:
                rawValue = fastNoise.GetPerlinFractal(x, y);
                break;
            case FastNoise.NoiseType.Simplex:
                rawValue = fastNoise.GetSimplex(x, y);
                break;
            case FastNoise.NoiseType.SimplexFractal:
                rawValue = fastNoise.GetSimplexFractal(x, y);
                break;
            case FastNoise.NoiseType.Cellular:
                rawValue = fastNoise.GetCellular(x, y);
                break;
            case FastNoise.NoiseType.WhiteNoise:
                rawValue = fastNoise.GetWhiteNoise(x, y);
                break;
            case FastNoise.NoiseType.Cubic:
                rawValue = fastNoise.GetCubic(x, y);
                break;
            case FastNoise.NoiseType.CubicFractal:
                rawValue = fastNoise.GetCubicFractal(x, y);
                break;
            default:
                rawValue = 0f;
                break;
        }
        return (rawValue + 1f) * 0.5f * maxHeight;
    }
}

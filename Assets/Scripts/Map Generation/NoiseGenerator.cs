using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseGenerator
{
    public int seed = 237589;
    public bool randomSeed = false;
    public float frequency = 0.01f;
    public int octaves = 3;
    public float lacunarity = 2;
    public float gain = 0.5f;
    public FastNoise.NoiseType noiseType;
    public FastNoise.FractalType fractalType;
    [Tooltip("Changes the interpolation method used to smooth between noise values")] public FastNoise.Interp interp;

    FastNoise fastNoise;

    public void Setup()
    {
        if (randomSeed) fastNoise = new FastNoise(Random.Range(int.MinValue, int.MaxValue));
        else fastNoise = new FastNoise(seed);

        fastNoise.SetFrequency(frequency);
        fastNoise.SetFractalOctaves(octaves);
        fastNoise.SetFractalLacunarity(lacunarity);
        fastNoise.SetFractalGain(gain);
        fastNoise.SetNoiseType(noiseType);
        fastNoise.SetFractalType(fractalType);
        fastNoise.SetInterp(interp);
    }

    public float GetValue(int x, int y)
    {
        return fastNoise.GetNoise(x,y);
    }
}

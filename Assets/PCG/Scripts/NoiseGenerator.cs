using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator
{
    // Generates a new noise map based on parameters
    // Returns a 2d float array
    public static float[,] GenerateNoiseMap (int noiseSampleSize, float scale, Wave[] waves, int resolution = 1)
    {
        float[,] noiseMap = new float[noiseSampleSize * resolution, noiseSampleSize * resolution];

        for(int x = 0; x < noiseSampleSize * resolution; x++) {
            for(int y = 0; y < noiseSampleSize * resolution; y++) {
                float samplePosX = (float)x /  scale / (float)resolution;
                float samplePosY = (float)y / scale / (float)resolution;

                float noise = 0.0f;
                float normalization = 0.0f;

                foreach(Wave wave in waves) {
                    noise += wave.amplitude * Mathf.PerlinNoise(samplePosX * wave.frequency + wave.seed, samplePosY * wave.frequency + wave.seed);
                    normalization += wave.amplitude;
                }

                noise /= normalization; // Normalise this to the average of all the waves
                noiseMap[x, y] = noise;
            }
        }

        return noiseMap;
    }
}

[System.Serializable] // Make this visible in the Unity Inspector
public class Wave {
    public float seed; // Provides a random offset applied to the noise map when we sample it
    public float frequency; // Frequency of peaks in the area of the noisemap
    public float amplitude; // Height of mountain peaks
}
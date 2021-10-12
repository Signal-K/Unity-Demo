using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureBuilder
{
    public static Texture2D BuildTexture (float[,] noiseMap, TerrainType[] terrainTypes) { // Accept different parameters to customize what the terrain will look like and how it will behave
        Color[] pixels = new Color[noiseMap.Length];

        int pixelLength = noiseMap.GetLength(0);

        for(int x = 0; x < pixelLength; x++) {
            for(int z = 0; z < pixelLength; z++)
            {
                int index = (x * pixelLength) + z;

                // Loop through each terrain type
                for(int t = 0; t < terrainTypes.Length; t++) {
                    // Is this current noise map less than the terrainTypes threshold?
                    if(noiseMap[x, z] < terrainTypes[t].threshold) { // If yes, we are within this gradient
                        float minVal = t == 0 ? 0 : terrainTypes[t - 1].threshold;
                        float maxVal = terrainTypes[t].threshold;

                        pixels[index] = terrainTypes[t].colorGradient.Evaluate(1.0f - (maxVal - noiseMap[x, z]) / (maxVal - minVal));
                        break; // Break out of the for loop
                    }
                }
            }
        }

        Texture2D texture = new Texture2D(pixelLength, pixelLength); // Height & width params
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear; // Change to FilterMode.Point to see individual pixels in the tilemap i.e. no filtering, showing at full sharpness
        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }
}

// Notes https://www.notion.so/skinetics/Notes-on-Procedural-Generation-df605ec522b84c4db2aaf8d37fa7f35d#88644b0714a04207baf0c4c78e77537f
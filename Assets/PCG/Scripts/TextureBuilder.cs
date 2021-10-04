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
                foreach(TerrainType terrainType in terrainTypes)
                {
                    if(noiseMap[x, z] < terrainType.threshold) // If true, make terrain this colour
                    {
                        //pixels[index] = terrainType.color;
                        break;
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

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

    public static TerrainType[,] CreateTerrainTypeMap (float[,] noiseMap, TerrainType[] terrainTypes)
    {
        int size = noiseMap.GetLength(0);
        TerrainType[,] outputMap = new TerrainType[size, size];

        for(int x = 0; x < size; x++)
        {
            for(int z = 0; z < size; z++)
            {
                for(int t = 0; t < terrainTypes.Length; t++)
                {
                    if(noiseMap[x, z] < terrainTypes[t].threshold)
                    {
                        outputMap[x, z] = terrainTypes[t];
                        break;
                    }
                }
            }
        }

        return outputMap;
    }
/*
    public static TerrainType[,] CreateTerrainTypeMap (float[,] noiseMap, TerrainType terrainTypes) {
        int size = noiseMap.GetLength(0); // Height of the 2D array
        terrainType[,] outputMap = new TerrainType[size, size];

        for(int x = 0; x < size; x++) {
            for(int z = 0; z < size; z++) {
                for(int t = 0; t < terrainTypes.Length; t++) {
                    if(noiseMap[x, z] < terrainTypes[t].threshold) {
                        outputMap[x, z] = terrainTypes[t];
                        break;
                    }
                }
            }
        }
        return outputMap;
    }*/
}

// Notes https://www.notion.so/skinetics/Notes-on-Procedural-Generation-df605ec522b84c4db2aaf8d37fa7f35d#88644b0714a04207baf0c4c78e77537f

/*
https://www.notion.so/skinetics/Notes-on-Procedural-Generation-df605ec522b84c4db2aaf8d37fa7f35d#68a5490cd08d4855aa396461777d2ab9
for(int x = 0; x < pixelLength; x++)
{
	for(int z = 0; z < pixelLength; z++)
	{
		int index = (x * pixelLength) + z;

		for(int t = 0; t < terrainTypes.Length; t++)
		{
			if(noiseMap[x, z] < terrainTypes[t].threshold) // If true, we are in this gradient
			{
				// Where is this noise map value in the threshold?
				float minVal = t == 0 ? 0 : terrainTypes[t - 1].threshold; // All the different minimum values of the thresholds of the terrain types
				float maxVal = terrainTypes[t].threshold;

				pixels[index] = terrainTypes[t].colorGradient.Evaluate(1.0f - (maxVal - noiseMap[x, z]) / (maxVal - minVal)); // Defining the colour of the pixel as a sample of the gradient based on its position compared to the terraintype threshold
				break;
 			}
		}
	}
}
*/
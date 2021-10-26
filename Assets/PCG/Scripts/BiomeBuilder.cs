using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BiomeType {
    Desert,
    Tundra,
    Savana,
    Forest,
    Rainforest
}

public class BiomeBuilder : MonoBehaviour
{
    public Biome[] biomes;
    public BiomeRow[] tableRows;

    public static BiomeBuilder instance;
    void Awake() {
        instance = this;
    }

    // Build the texture for the biome
    public Texture2D BuildTexture (TerrainType[,] heatMapTypes, TerrainType[,] moistureMapTypes) {
        int size = heatMapTypes.GetLength(0);
        Color[] pixels = new Color[size * size];

        for(int x = 0; x < size; x++) {
            for(int z = 0; z < size; z++) {
                int index = (x * size) + z;
                int heatMapIndex = heatMapTypes[x, z].index;
                int moistureMapIndex = moistureMapTypes[x, z].index;

                Biome biome = null;

                foreach(Biome b in biomes) {
                    if(b.type == tableRows[moistureMapIndex].tableColumns[heatMapIndex]) {
                        biome = b;
                        break;
                    }
                }
                pixels[index] = biome.color;
            }
        }

        // Assign the colour of the biome to a texture
        Texture2D texture = new Texture2D(size, size);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;
        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }

    // Set the biome for TileGenerator.cs > CreateDataMap
    public Biome GetBiome(TerrainType heatTerrainType, TerrainType moistureTerrainType) {
        foreach(Biome b in biomes) {
            if(b.type == tableRows[moistureMapIndex].tableColumns[heatMapIndex]) {
                return b;
            }
        }
        return null; // Didn't match with any of the biomes in the biome table, so return null 
    }
}

[System.Serializable]
public class BiomeRow {
    public BiomeType[] tableColumns;
}

[System.Serializable]
public class Biome {
    public BiomeType type; // Make the biome type selectable in the Unity inspector for each point on the grid
    public Color color;
    public bool spawnPrefabs; // Do we want prefabs to spawn on these biomes?
    public GameObject[] spawnablePrefabs; // Spawn prefabs on top of the grid (in the biomes)

    [Range(0.0f, 3.0f)] // 0 = 0 prefabs per position, 3 = 3 per position
    public float density = 1.0f; // How densely packed are the prefabs? 
}

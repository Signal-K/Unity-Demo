using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainVisualization { // Visualize different maps dropdown selection
    Height,
    Heat,
    Moisture,
    Biome
}

public class TileGenerator : MonoBehaviour
{
    [Header("Parameters")]
    public int noiseSampleSize;
    public float scale;
    public float maxHeight = 1.0f;
    public TerrainVisualization visualizationType;

    private MeshRenderer tileMeshRenderer;
    private MeshFilter tileMeshFilter;
    private MeshCollider tileMeshCollider;

    // Params for UniformNoiseMap
    private MeshGenerator meshGenerator;
    private MapGenerator mapGenerator;

    private TerrainData[,] dataMap;
    
    public int textureResolution = 1;

    // Hide it from the inspector
    [HideInInspector]
    public Vector2 offset;


    [Header("Terrain Types")]
    public TerrainType[] heightTerrainTypes; // Send this to textureBuilder.cs
    public TerrainType[] heatTerrainTypes;
    public TerrainType[] moistureTerrainTypes;

    [Header("Waves")]
    public Wave[] waves;
    public Wave[] heatWaves;
    public Wave[] moistureWaves;

    [Header("Curves")]
    public AnimationCurve heightCurve;

    void Start()
    {
        // Get the tile components
        tileMeshRenderer = GetComponent<MeshRenderer>();
        tileMeshFilter = GetComponent<MeshFilter>();
        tileMeshCollider = GetComponent<MeshCollider>();

        meshGenerator = GetComponent<MeshGenerator>();
        mapGenerator = FindObjectOfType<MapGenerator>();

        GenerateTile();
    }

    void GenerateTile()
    {
        // Generate a new height map
        float[,] heightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale, waves, offset);
        float[,] hdHeightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale, waves, offset, textureResolution);

        Vector3[] verts = tileMeshFilter.mesh.vertices; // Puts all the vertices inside this array

        for(int x = 0; x < noiseSampleSize; x++)
        {
            for(int z = 0; z < noiseSampleSize; z++) // Get index of vertices currently on
            {
                int index = ( x * noiseSampleSize) + z;

                verts[index].y = heightCurve.Evaluate(heightMap[x, z]) * maxHeight;
            }
        }
        // Apply array to the mesh
        tileMeshFilter.mesh.vertices = verts;
        // Recalculate boundaries of the mesh
        tileMeshFilter.mesh.RecalculateBounds();
        tileMeshFilter.mesh.RecalculateNormals();
        // Update mesh collider
        tileMeshCollider.sharedMesh = tileMeshFilter.mesh;

        // Create the height map texture
        Texture2D heightMapTexture = TextureBuilder.BuildTexture(hdHeightMap, heightTerrainTypes);

        float[,] heatMap = GenerateHeatMap(heightMap);
        float[,] moistureMap = GenerateMoistureMap(heightMap);

        TerrainType[,] heatTerrainTypeMap = TextureBuilder.CreateTerrainTypeMap(heatMap, heatTerrainTypes);
        TerrainType[,] moistureTerrainTypeMap = TextureBuilder.CreateTerrainTypeMap(moistureMap, moistureTerrainTypes);

        switch(visualizationType) {
            case TerrainVisualization.Height:
                tileMeshRenderer.material.mainTexture = TextureBuilder.BuildTexture(hdHeightMap, heightTerrainTypes);
                break;
            case TerrainVisualization.Heat:
                tileMeshRenderer.material.mainTexture = TextureBuilder.BuildTexture(heatMap, heatTerrainTypes);
                break;
            case TerrainVisualization.Moisture:
                tileMeshRenderer.material.mainTexture = TextureBuilder.BuildTexture(moistureMap, moistureTerrainTypes);
                break;
            case TerrainVisualization.Biome:
                tileMeshRenderer.material.mainTexture = BiomeBuilder.instance.BuildTexture(heatTerrainTypeMap, moistureTerrainTypeMap);
                break;
        }

        CreateDataMap(heatTerrainTypeMap, moistureTerrainTypeMap);
        TreeSpawner.instance.Spawn(dataMap);
    }

    // Create a data map/terrain data
    void CreateDataMap(TerrainType[,] heatTerrainTypeMap, TerrainType[,] moistureTerrainTypeMap) {
        dataMap = new TerrainData[noiseSampleSize, noiseSampleSize];
        Vector3[] verts = tileMeshFilter.mesh.vertices;

        for(int x = 0; x < noiseSampleSize; x++) {
            for(int z = 0; z < noiseSampleSize; z++) {
                TerrainData data = new TerrainData();

                // Set the position of the data point (the corresponding vertice that is currently being calculated - calculate the point)
                data.position = transform.position + verts[(x * noiseSampleSize) + z]; // transform.position added to specify the current tile, otherwise this would set data for the vertice value/position on EACH tile (currently we have 2*2 tile configuration)
                data.heatTerrainType = heatTerrainTypeMap[x, z];
                data.moistureTerrainType = moistureTerrainTypeMap[x, z];
                // Set the biome
                data.biome = BiomeBuilder.instance.GetBiome(data.heatTerrainType, data.moistureTerrainType);                

                // Apply this data object to the data array
                dataMap[x, z] = data;
            }
        }
    }

    float[,] GenerateHeatMap (float[,] heightMap) {
        // Generate a uniform noise map
        float[,] uniformHeatMap = NoiseGenerator.GenerateUniformNoiseMap(noiseSampleSize, transform.position.z * (noiseSampleSize / meshGenerator.xSize), (noiseSampleSize / 2 * mapGenerator.numX) + 1);
        // Generate normal noise map
        float[,] randomHeatMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale, heatWaves, offset);

        float[,] heatMap = new float[noiseSampleSize, noiseSampleSize];
        // Add temperature to different heights in the maps
        for(int x = 0; x < noiseSampleSize; x++) {
            for(int z = 0; z < noiseSampleSize; z++) {
                heatMap[x, z] = randomHeatMap[x, z] * uniformHeatMap[x, z];
                heatMap[x, z] += 0.5f * heightMap[x, z]; // The closer to 0 the point is, the warmer it will be (cooler as it gets to 1)

                // Clamp the height value from 1.2 to 1 (number can't be >1)
                heatMap[x, z] = Mathf.Clamp(heatMap[x, z], 0.0f, 0.99f); // Place the value of the point between the two points 0 and 1 inclusive
            }
        }

        return heatMap;
    }

    float[,] GenerateMoistureMap (float[,] heightMap)
    {
        float[,] moistureMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale, moistureWaves, offset);

        for(int x = 0; x < noiseSampleSize; x++) {
            for(int z = 0; z < noiseSampleSize; z++) {
                moistureMap[x, z] -= 0.1f * heightMap[x, z];
            }
        }

        return moistureMap;
    }
}

[System.Serializable]
public class TerrainType {
    public int index;
    [Range(0.0f, 1.0f)] // Slider inside Unity editor between 0 and 1
    public float threshold; // Height at which this terrain type ends
    public Gradient colorGradient; // Colour of the terrain type
}

public class TerrainData {
    public Vector3 position; // Position of the point on the terrain model / terrain data 2d array
    public TerrainType heatTerrainType;
    public TerrainType moistureTerrainType; // How wet/dry the point is
    public Biome biome; // What biome is this position in?
}
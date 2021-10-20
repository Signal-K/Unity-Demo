using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [Header("Parameters")]
    public int noiseSampleSize;
    public float scale;
    public float maxHeight = 1.0f;

    private MeshRenderer tileMeshRenderer;
    private MeshFilter tileMeshFilter;
    private MeshCollider tileMeshCollider;

    // Params for UniformNoiseMap
    private MeshGenerator meshGenerator;
    private MapGenerator mapGenerator;
    
    public int textureResolution = 1;

    // Hide it from the inspector
    [HideInInspector]
    public Vector2 offset;


    [Header("Terrain Types")]
    public TerrainType[] heightTerrainTypes; // Send this to textureBuilder.cs
    public TerrainType[] heatTerrainTypes;

    [Header("Waves")]
    public Wave[] waves;
    public Wave[] heatWaves;

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

        // Apply the height map texture to the MeshRenderer
        //tileMeshRenderer.material.mainTexture = heightMapTexture;
        // Heat map application to texture
        float[,] heatMap = GenerateHeatMap(heightMap);
        // Render as a texture
        tileMeshRenderer.material.mainTexture = TextureBuilder.BuildTexture(heatMap, heatTerrainTypes);
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
}

[System.Serializable]
public class TerrainType {
    [Range(0.0f, 1.0f)] // Slider inside Unity editor between 0 and 1
    public float threshold; // Height at which this terrain type ends
    public Gradient colorGradient; // Colour of the terrain type
}
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
    public int textureResolution = 1;

    [Header("Terrain Types")]
    public TerrainType[] heightTerrainTypes; // Send this to textureBuilder.cs

    void Start()
    {
        // Get the tile components
        tileMeshRenderer = GetComponent<MeshRenderer>();
        tileMeshFilter = GetComponent<MeshFilter>();
        tileMeshCollider = GetComponent<MeshCollider>();

        GenerateTile();
    }

    void GenerateTile()
    {
        // Generate a new height map
        float[,] heightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale);
        float[,] hdHeightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale, textureResolution);

        Vector3[] verts = tileMeshFilter.mesh.vertices; // Puts all the vertices inside this array

        for(int x = 0; x < noiseSampleSize; x++)
        {
            for(int z = 0; z < noiseSampleSize; z++) // Get index of vertices currently on
            {
                int index = ( x * noiseSampleSize) + z;

                verts[index].y = heightMap[x, z] * maxHeight;
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
        tileMeshRenderer.material.mainTexture = heightMapTexture;
    }
}

[System.Serializable]
public class TerrainType {
    [Range(0.0f, 1.0f)] // Slider inside Unity editor between 0 and 1
    public float threshold; // Height at which this terrain type ends
    public Color color; // Colour of the terrain type
}
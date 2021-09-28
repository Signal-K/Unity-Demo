using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [Header("Parameters")]
    public int noiseSampleSize;
    public float scale;

    private MeshRenderer tileMeshRenderer;
    private MeshFilter tileMeshFilter;
    private MeshCollider tileMeshCollider;

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
        float[,] heightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale);

        Texture2D heightMapTexture = TextureBuilder.BuildTexture(heightMap);
        tileMeshRenderer.material.mainTexture = heightMapTexture;
    }
}

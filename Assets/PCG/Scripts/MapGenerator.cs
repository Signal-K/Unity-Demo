using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public TileGenerator tilePrefab;
     public int numX = 2; // How many tiles along the x axis
    public int numZ = 2; // 2 x 2 grid formation

    void Start ()
    {
        GenerateTiles();
    }

    void GenerateTiles() {
        float tileSize = tilePrefab.GetComponent<MeshGenerator>().xSize;

        // How many times we spawn this tile in - loop 2 times for x and 2 times for z axis (based on numX & numZ)
        for(int x = 0; x < numX; x++) {
            for(int z = 0; z < numZ; z++) { // IF we have 2x2, this code will be called 4 times
                GameObject tileObj = Instantiate(tilePrefab.gameObject, transform);
                tileObj.transform.position = new Vector3((x - ((float)numX / 2)) * tileSize, 0, (z - ((float)numZ / 2)) * tileSize);

                // Set the tile's offset rate
                float offsetRate = (tilePrefab.noiseSampleSize - 1) / tilePrefab.scale;
                tileObj.GetComponent<TileGenerator>().offset = new Vector2(x * offsetRate, z * offsetRate);
            }
        }
    }
}

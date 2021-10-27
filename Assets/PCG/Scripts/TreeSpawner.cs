using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public float MaxTreeOffset; // // Add an offset to the position (without the RANDOM offset the prefabs would spawn in the same pattern, looking uniform and not natural)
    public static TreeSpawner instance;

    void Awake() {
        instance = this;
    }

    public void Spawn (TerrainData[,] dataMap) {
        int size = dataMap.GetLength(0);

        for(int x = 0; x < size; x++) {
            for(int z = 0; z < size; z++) {
                // Check for the biome we are currently on
                if(dataMap[x, z].biome.spawnPrefabs) {
                    // If prefabs can be spawned on this biome
                    float density = dataMap[x, z].biome.density;

                    if(density > 1.0f) {
                        SpawnTree(dataMap[x, z].position, dataMap[x, z].biome.spawnablePrefabs);

                        int extraTreesToSpawn = Mathf.RoundToInt(Random.Range(1.0f, density));
                        for(int e = 0; e < extraTreesToSpawn; e++) {
                            SpawnTree(dataMap[x, z].position, dataMap[x, z].biome.spawnablePrefabs);
                        }
                    }
                    else { // If under 1, random chance to spawn a tree
                        if(Random.Range(0.0f, 1.0f) < density) {
                            SpawnTree(dataMap[x, z].position, dataMap[x, z].biome.spawnablePrefabs);
                        }
                    }
                }
            }
        }
    }

    void SpawnTree(Vector3 position, GameObject[] availablePrefabs) {
        Vector3 truePos = new Vector3(position.x + (Random.value * MaxTreeOffset), position.y, position.z + (Random.value * MaxTreeOffset)); // Actual position of each prefab that will be spawned in
        GameObject treeObj = Instantiate(availablePrefabs[Random.Range(0, availablePrefabs.Length)], truePos, Quaternion.identity);
    }
}
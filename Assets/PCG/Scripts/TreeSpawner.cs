using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
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
                        // Spawn a tree

                        int extraTreesToSpawn = Mathf.RoundToInt(Random.Range(1.0f, density));
                        for(int e = 0; e < extraTreesToSpawn; e++) {
                            // Spawn tree
                        }
                    }
                    else { // If under 1, random chance to spawn a tree
                        if(Random.Range(0.0f, 1.0f) < density) {
                            // Spawn a tree
                        }
                    }
                }
            }
        }
    }
}

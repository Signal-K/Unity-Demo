using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeBuilder : MonoBehaviour
{
    public BiomeRow[] biomeRows;

    // Build the texture for the biome
    public Texture2D BuildTexture () {
        
    }
}

[System.Serializable]
public class BiomeRow {
    public Biome[] biomes;
}

[System.Serializable]
public class Biome {
    public string name;
    public Color color;
}

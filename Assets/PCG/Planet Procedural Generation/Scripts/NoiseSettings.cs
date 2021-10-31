using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings {
    // Noise attributes
    public float strength = 1; // Strength of the noise
    [Range(1,8)]
    public int numLayers = 1;
    public float baseRoughness = 1;
    public float roughness = 2;;
    public float persistence = .5; // Half the amplitude with each layer
    public Vector3 centre; // Allow us to move the noise around
}
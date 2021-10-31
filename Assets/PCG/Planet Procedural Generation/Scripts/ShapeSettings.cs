using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject {

    public float planetRadius = 1;
    public NoiseLayer[] noiseLayers;

    [System.Serializable]
    public class NoiseLayer {
        // Toggle visibility of individual layers
        public bool enabled = true;
        public bool useFirstLayerAsMask;
        public NoiseSettings noiseSettings;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter {
    NoiseSettings settings;
    Noise noise = new Noise();

    public NoiseFilter(NoiseSettings settings) {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point) {
        float noiseValue = (noise.Evaluate(point * settings.roughness + settings.centre)+1)*.5f; // Values are initially from -1 to 1, let's squash this to 0to1
        return noiseValue * settings.strength;
    }
}
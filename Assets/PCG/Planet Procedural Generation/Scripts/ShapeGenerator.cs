using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator {
    ShapeSettings settings;
    INoiseFilter[] noiseFilters;
    public MinMax elevationMinMax;

    public ShapeGenerator(ShapeSettings settings)
    {
        this.settings = settings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for(int i = 0; i < noiseFilters.Length; i++) {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
        elevationMinMax = new MinMax();
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if(noiseFilters.Length > 0) {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if(settings.noiseLayers[0].enabled) {
                elevation = firstLayerValue;
            }
        }
        for(int i = 1; i < noiseFilters.Length; i++) {
            if(settings.noiseLayers[i].enabled) {
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask)?firstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }
        // Calculate the final elevation of the point before returning it
        elevation = settings.planetRadius *  (1+elevation);
        elevationMinMax.AddValue(elevation); // Keeping track of the minimum and maximum elevation for all the vertices in the map
        return pointOnUnitSphere * elevation;
    }
}

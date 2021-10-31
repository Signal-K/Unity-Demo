using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator 
{
    ShapeSettings settings; // Assigned in the constructor
    public ShapeGenerator(ShapeSettings settings) {
        this.settings = settings;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere) {
        return pointOnUnitSphere * settings.planetRadius;
    }
}

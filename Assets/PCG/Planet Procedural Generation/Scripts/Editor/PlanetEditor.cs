using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        DrawSettingsEditor(Planet.shapeSettings);
        DrawSettingsEditor(Planet.colorSettings);
    }

    void DrawSettingsEditor(Object settings) {
        Editor editor = CreateEditor(settings);
        // Display it
        editor.OnInspectorGUI();
    }

    private void OnEnable() {
        planet = (Planet)target;
    }
}

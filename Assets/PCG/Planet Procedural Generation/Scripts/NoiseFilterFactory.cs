using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory {
    public static INoiseFilter CreateNoiseFilter(NoiseSettings settings) {
        // Type of noise that will be created depends on the filter type specifed in the settings (NoiseSettings.cs main class)
        switch(settings.filterType) {
            case NoiseSettings.FilterType.Simple:
                return new SimpleNoiseFilter(settings.simpleNoiseSettings);
            case NoiseSettings.FilterType.Rigid:
                return new RigidNoiseFilter(settings.rigidNoiseSettings); 
        }       
        return null;
    }
}
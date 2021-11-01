using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INoiseFilter {
    // Define the method that each class has to have to qualify as a noise filter
    float Evaluate(Vector3 point);
}
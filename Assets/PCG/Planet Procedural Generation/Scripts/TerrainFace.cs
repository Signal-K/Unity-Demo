using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace {
    Mesh mesh;
    int resolution;
    Vector3 localUp; // What way it's facing
    Vector3 axisA;
    Vector3 axisB;

    public TerrainFace(Mesh mesh, int resolution, Vector3 localup) {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = new Vector3.Cross(localUp, axisA);
    }

    public ConstructMesh() {
        Vector3[] vertices = new Vector3[resolution * resolution]; // Resolution = number of verices across a single face
        int[] triangles = new int[(resolution-1)*(resolution-1)*2*3];

        for(int y = 0; y < resolution; y++) {
            for(int x = 0; x < resolution; x++) {
                int i = x + y * resolution; // Number of iterations of the inner loop + outer loop
                Vector2 percent = new Vector2(x, y) / (resolution - 1); // When x is at highest point, it will be equal to 1% (lowest point 0%) # how close to complete each loop is - where should the vertex be on each face?
                Vector3 pointOnUnitCube = localUp + (percent.x-.5f)*2*axisA + (percent.y - .5f) * 2 * axisB;// ^How far along the axis we are
                vertices[i] = pointOnUnitCube;
            }
        }
    }
}

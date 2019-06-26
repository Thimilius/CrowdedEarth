using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdedEarth.Visualization {
    public static class MeshBuilder {
        public static Mesh BuildNewCylinder(float fill) {
            Mesh mesh = new Mesh();

            Vector3 topCenter = new Vector3(0, 0.5f, 0);
            Vector3 bottomCenter = new Vector3(0, -0.5f, 0);

            const int SAMPLES = 500;
            float tau = Mathf.PI * 2;
            float deltaTheta = tau / SAMPLES;

            int limit = (int)(fill * SAMPLES) + 1;

            float radius = 1;

            List<Vector3> verticies = new List<Vector3>();
            for (int i = 0; i < limit - 1; i++) {
                float theta0 = i * deltaTheta;
                float theta1 = (i + 1) * deltaTheta;

                Vector3 v0 = GetVectorByTheata(theta0, 0);
                Vector3 v1 = GetVectorByTheata(theta1, 0);

                Vector3 topP0 = topCenter;
                Vector3 topP1 = topCenter + radius * v0;
                Vector3 topP2 = topCenter + radius * v1;
                Vector3 bottomP0 = bottomCenter;
                Vector3 bottomP1 = bottomCenter + radius * v0;
                Vector3 bottomP2 = bottomCenter + radius * v1;

                verticies.Add(topP1);
                verticies.Add(topP0);
                verticies.Add(topP2);

                verticies.Add(bottomP0);
                verticies.Add(bottomP1);
                verticies.Add(bottomP2);

                verticies.Add(bottomP1);
                verticies.Add(topP2);
                verticies.Add(bottomP2);

                verticies.Add(topP1);
                verticies.Add(topP2);
                verticies.Add(bottomP1);
            }

            int[] indicies = new int[verticies.Count];
            for (int i = 0; i < verticies.Count; i++) {
                indicies[i] = i;
            }

            mesh.SetVertices(verticies);
            mesh.SetIndices(indicies, MeshTopology.Triangles, 0);
            mesh.RecalculateNormals();

            return mesh;
        }

        private static Vector3 GetVectorByTheata(float theta, float y) {
            return new Vector3(Mathf.Cos(theta), y, Mathf.Sin(theta)); 
        }
    }
}

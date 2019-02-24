using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AheadToolRangeAsChild : AheadToolRange
{
    public override void UpdateLine(Transform basePosition, int layer, bool visible)
    {
        FindNearSelectableObject(basePosition, layer, visible);
        ToLocalVertex(basePosition, vertices);
        CreateMesh(basePosition, vertices);
    }

    private void CreateMesh(Transform basePosition, Vector3[] vertices)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    private void ToLocalVertex(Transform basePosition, Vector3[] vertexes)
    {
        for (int i = 0; i < vertexes.Length; i++)
        {
            vertexes[i] = basePosition.worldToLocalMatrix.MultiplyPoint3x4(vertexes[i]);
        }
    }
}

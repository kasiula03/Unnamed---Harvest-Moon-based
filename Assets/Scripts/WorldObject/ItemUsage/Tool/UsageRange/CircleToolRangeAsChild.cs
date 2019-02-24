using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleToolRangeAsChild : CircleToolRange
{
    public override void UpdateLine(Transform basePosition, int layer, bool visible)
    {
        int[] indices = FindNearSelectableObject(basePosition, layer, visible);
        CreateMesh(basePosition, indices);
    }

    private void CreateMesh(Transform basePosition, int[] triangles)
    {
        Vector3[] vertexes = vertexList.ToArray();
        ToLocalVertex(basePosition, vertexes);
        mesh.Clear();
        mesh.vertices = vertexes;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private void ToLocalVertex(Transform basePosition, Vector3[] vertexes)
    {
        for (int i = 0; i < vertexes.Length; i++)
        {
            vertexes[i] = basePosition.worldToLocalMatrix.MultiplyPoint3x4(vertexes[i]);
        }
    }
}

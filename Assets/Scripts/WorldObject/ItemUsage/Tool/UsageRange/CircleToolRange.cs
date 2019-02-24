using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleToolRange : ToolRange
{
    public int toolRange = 10;
    public int segments = 20;

    protected List<int> triangleList = new List<int>();
    protected List<Vector3> vertexList = new List<Vector3>();

    public override Collider[] FindObjectInRange(GameObject player, int selectableLayer)
    {
        return Physics.OverlapSphere(player.transform.position + player.transform.forward, toolRange, selectableLayer);
    }

    public override void UpdateLine(Transform basePosition, int selectableLayer, bool visible)
    {
        int[] indices = FindNearSelectableObject(basePosition, selectableLayer, visible);
        CreateMesh(indices);

    }

    protected int[] FindNearSelectableObject(Transform basePosition, int selectableLayer, bool visible)
    {
        if (!visible)
        {
            mesh.Clear();
            return new int[0];
        }
        triangleList.Clear();
        vertexList.Clear();
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < segments; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * toolRange + basePosition.position.x;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * toolRange + basePosition.position.z;
            vertexList.Add(new Vector3(x, 0.2f, z) + basePosition.forward);

            angle += (360f / segments);
        }
        List<Vector2> vecs = ToVector2(vertexList);
        Triangulator tr = new Triangulator(vecs.ToArray());
        return tr.Triangulate();
    }

    private List<Vector2> ToVector2(List<Vector3> vertexes)
    {
        List<Vector2> verts = new List<Vector2>();
        foreach (Vector3 vec in vertexes)
        {
            verts.Add(new Vector2(vec.x, vec.z));
        }
        return verts;
    }

    private void CreateMesh(int[] triangles)
    {
        mesh.Clear();
        mesh.vertices = vertexList.ToArray();
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}

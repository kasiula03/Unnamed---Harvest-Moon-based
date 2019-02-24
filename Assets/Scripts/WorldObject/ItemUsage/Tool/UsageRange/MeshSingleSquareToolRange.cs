using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeshSingleSquareToolRange : ToolRange
{
    protected int[] triangles = new int[] { 0, 1, 2, 2, 1, 3 };
    protected Vector3[] vertices = new Vector3[4];

    public override Collider[] FindObjectInRange(GameObject player, int layer)
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, player.transform.forward - player.transform.up, out hit, 2f, layer))
        {
            if (hit.triangleIndex > 0)
            {
                Collider[] colliders = new Collider[1];
                SingleMeshSquare square = SinglePointWithMirror(hit);
                FakeMeshCollider collider = (FakeMeshCollider)((MeshCollider)hit.collider);
                if(square.startPoint.z > square.mirrorPoint.z)
                {
                    collider.startPoint = square.startPoint;
                }
                else
                {
                    collider.startPoint = square.mirrorPoint;
                }

                colliders[0] = collider;
                return colliders;
            }
            else
            {
                Collider[] colliders = new Collider[0];
                return colliders;
            }
        }
        else
        {
            Collider[] colliders = new Collider[0];
            return colliders;
        }
    }

    public override void UpdateLine(Transform basePosition, int layer, bool visible)
    {
        RaycastHit hit;
        if (Physics.Raycast(basePosition.position, basePosition.forward - basePosition.up, out hit, 2f, layer))
        {
            if (hit.triangleIndex > 0)
            {
                SingleMeshSquare square = SinglePointWithMirror(hit);
                DrawSqureByPoint(square.startPoint, square.mirrorPoint, square.direction, square.width, basePosition);
            }
        }
        CreateMesh(vertices);

    }

    private SingleMeshSquare SinglePointWithMirror(RaycastHit hit)
    {
        int index = hit.triangleIndex;
        Collider hitCollider = hit.collider;
        MeshCollider meshColldier = hitCollider.gameObject.GetComponent<MeshCollider>();
        Mesh mesh = hitCollider.gameObject.GetComponent<MeshFilter>().mesh;

        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        Vector3 p0 = hitCollider.gameObject.transform.position + hitCollider.gameObject.transform.localScale.x * vertices[triangles[index * 3 + 0]];
        Vector3 p1 = hitCollider.gameObject.transform.position + hitCollider.gameObject.transform.localScale.x * vertices[triangles[index * 3 + 1]];
        Vector3 p2 = hitCollider.gameObject.transform.position + hitCollider.gameObject.transform.localScale.x * vertices[triangles[index * 3 + 2]];

        float edge1 = Vector3.Distance(p0, p1);
        float edge2 = Vector3.Distance(p0, p2);
        float edge3 = Vector3.Distance(p1, p2);

        Vector3 shared1;
        Vector3 shared2;
        Vector3 single;
        if (edge1 > edge2 && edge1 > edge3)
        {
            shared1 = p0;
            shared2 = p1;
            single = p2;
        }
        else if (edge2 > edge1 && edge2 > edge3)
        {
            shared1 = p0;
            shared2 = p2;
            single = p1;
        }
        else
        {
            shared1 = p1;
            shared2 = p2;
            single = p0;
        }
        //if left and up
        int direction;

        if (shared1.z - single.z > 0 || shared2.z - single.z > 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        Vector3 mirrorPoint = single + new Vector3(Vector3.Distance(single, shared2) * -direction, 0, Vector3.Distance(single, shared2) * direction);

        float width = Vector3.Distance(single, shared2);

        return new SingleMeshSquare(single, mirrorPoint, direction, width);
    }

    private void DrawSqureByPoint(Vector3 p0, Vector3 p3, int direction, float size, Transform basePosition)
    {
        Vector3 p1 = p0 + new Vector3(size * -direction, 0, 0);
        Vector3 p2 = p0 + new Vector3(0, 0, size * direction);

        vertices[0] = basePosition.InverseTransformPoint(p0);
        vertices[1] = basePosition.InverseTransformPoint(p1);
        vertices[2] = basePosition.InverseTransformPoint(p2);
        vertices[3] = basePosition.InverseTransformPoint(p3);
    }

    private void CreateMesh(Vector3[] vertices)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}

public class FakeMeshCollider: Collider
{
    public Vector3 startPoint { get; set; }
    public GameObject relatedGameObject { get; set; }

    public static explicit operator FakeMeshCollider(MeshCollider collider)
    {
        FakeMeshCollider customCollider = new FakeMeshCollider();
        customCollider.relatedGameObject = collider.gameObject;
        return customCollider;
    }
}

public struct SingleMeshSquare
{
    public Vector3 startPoint;
    public Vector3 mirrorPoint;
    public int direction;
    public float width;

    public SingleMeshSquare(Vector3 startP, Vector3 mirrorP, int dir, float wid)
    {
        startPoint = startP;
        mirrorPoint = mirrorP;
        direction = dir;
        width = wid;
    }
}
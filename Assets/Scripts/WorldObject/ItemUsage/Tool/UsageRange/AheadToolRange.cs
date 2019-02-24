using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AheadToolRange : ToolRange
{
    public int toolRange = 1;

    private Vector3 leftDownCorner = new Vector3(0.5f, 0, 0f);
    private Vector3 leftUpCorner = new Vector3(0.5f, 0, 1f);
    private Vector3 rightUpCorner = new Vector3(1.5f, 0, 1f);
    private Vector3 rightDownCorner = new Vector3(1.5f, 0, 0f);

    protected int[] triangles = new int[] { 0, 1, 2, 2, 1, 3 };
    protected Vector3[] vertices = new Vector3[4];

    public override Collider[] FindObjectInRange(GameObject player, int selectableLayer)
    {
        Collider[] hitColiders = Physics.OverlapBox(player.transform.position + player.transform.forward, (player.transform.localScale / 2) * toolRange, Quaternion.identity, selectableLayer);
        if (hitColiders.Length > 0)
        {
            Collider[] colliders = new Collider[1];
            colliders[0] = hitColiders[0];
            return colliders;
        }
        else
        {
            return hitColiders;
        }
    }

    public override void UpdateLine(Transform basePosition, int layer, bool visible)
    {
        FindNearSelectableObject(basePosition, layer, visible);
        CreateMesh(vertices);
    }

    protected void FindNearSelectableObject(Transform basePosition, int layer, bool visible)
    {
        Collider[] hitColliders = Physics.OverlapBox(basePosition.position + basePosition.forward, (basePosition.localScale / 2) * toolRange, Quaternion.identity, layer);

        if (!visible)
        {
            mesh.Clear();
            return;
        }

        if (hitColliders.Length > 0)
        {
            Collider selectCollider = hitColliders[0];
            Vector3 lower = selectCollider.bounds.extents * 2;
            lower.y = 0;
            Vector3 leftDown = selectCollider.bounds.center - selectCollider.bounds.extents;
            Vector3 rightUp = leftDown + lower;
            Vector3 leftUp = leftDown + new Vector3(0, 0, lower.z);
            Vector3 rightDown = leftDown + new Vector3(lower.x, 0, 0);

            vertices[0] = leftDown + (Vector3.left + Vector3.back) * 0.05f;
            vertices[1] = leftUp + (Vector3.left + Vector3.forward) * 0.05f;
            vertices[1] = leftUp + (Vector3.left + Vector3.forward) * 0.05f;
            vertices[3] = rightUp + (Vector3.right + Vector3.forward) * 0.05f;
            vertices[2] = rightDown + (Vector3.right + Vector3.back) * 0.05f;
        }
        else
        {
            Vector3 leftDown = -basePosition.right / 2;
            Vector3 leftUp = -basePosition.right / 2 + basePosition.forward;
            Vector3 rightDown = basePosition.right / 2;
            Vector3 rightUp = basePosition.right / 2 + basePosition.forward;
            vertices[0] = leftDown * toolRange + basePosition.position + basePosition.forward;
            vertices[1] = leftUp * toolRange + basePosition.position + basePosition.forward;
            vertices[2] = rightDown * toolRange + basePosition.position + basePosition.forward;
            vertices[3] = rightUp * toolRange+ basePosition.position + basePosition.forward;
        }
    }

    private void CreateMesh(Vector3[] vertices)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}

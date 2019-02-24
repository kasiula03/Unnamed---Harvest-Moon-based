using System.Collections;
using UnityEngine;
using System.Linq;

public class HoeUsage : ToolUsage
{
    public GameObject fieldPrefab;

    protected new void OnStart()
    {
        base.OnStart();
    }

    protected override bool HitObjectFilter(Collider collider)
    {
        return true;
    }

    protected override IEnumerator ToolAcion(Collider collider)
    {
        FakeMeshCollider meshCollider = ((FakeMeshCollider)collider);
        Vector3 startPoint = meshCollider.startPoint + new Vector3(1f, 0, -1f);
        startPoint.y = meshCollider.relatedGameObject.transform.InverseTransformPoint(new Vector3(0, 0.3f, 0)).y;

        if (!FieldAlreadyBuried(meshCollider.relatedGameObject, startPoint))
        {
            yield return PlayAndWaitPercentFinish(0.6f);
            Instantiate(fieldPrefab, startPoint, fieldPrefab.transform.rotation, meshCollider.relatedGameObject.transform);
        }

        yield return null;
    }

    private bool FieldAlreadyBuried(GameObject relatedGameObject, Vector3 startPoint)
    {
        Vector3 localPoint = relatedGameObject.gameObject.transform.InverseTransformPoint(startPoint);

        return relatedGameObject.gameObject.transform.GetComponentsInChildren<Transform>()
            .Where(child => child.GetInstanceID() != relatedGameObject.transform.GetInstanceID())
            .Where(child => child.localPosition.x == localPoint.x && child.localPosition.z == localPoint.z).Count() > 0;
    }

}

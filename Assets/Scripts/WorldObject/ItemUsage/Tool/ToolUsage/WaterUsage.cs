using System.Collections;
using UnityEngine;

public class WaterUsage : ToolUsage
{
    protected override IEnumerator ToolAcion(Collider collider)
    {
        Debug.Log(collider.gameObject.name);
        Crop crop = collider.gameObject.GetComponent<Crop>();
        if (crop != null)
        {
            crop.Water();
        }
        yield return null;
    }

    protected override bool HitObjectFilter(Collider collider)
    {
        return collider.GetComponent<Crop>();
    }
}

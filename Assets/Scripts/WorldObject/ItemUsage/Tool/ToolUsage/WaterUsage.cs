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
}

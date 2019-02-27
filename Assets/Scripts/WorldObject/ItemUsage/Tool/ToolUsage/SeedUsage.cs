using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedUsage : ToolUsage
{
    public GameObject cropToSpawn;

    protected override IEnumerator ToolAcion(Collider collider)
    {
        ThrowSeed(collider.gameObject);

        yield return null;
    }

    protected override bool HitObjectFilter(Collider collider)
    {
        return true;
    }

    private void ThrowSeed(GameObject field)
    {
        GameObject obj = Instantiate(cropToSpawn, field.transform, true);
        obj.transform.localPosition = new Vector3(0, 0, 0.05f);
        Debug.Log("Seed " + cropToSpawn.name + " has been planted");
    }
}

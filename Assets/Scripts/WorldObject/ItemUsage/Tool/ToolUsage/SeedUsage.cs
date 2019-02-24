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
        Instantiate(cropToSpawn, Vector3.zero, cropToSpawn.transform.rotation, field.transform);
        Debug.Log("Seed " + cropToSpawn.name + " has been planted");
    }
}

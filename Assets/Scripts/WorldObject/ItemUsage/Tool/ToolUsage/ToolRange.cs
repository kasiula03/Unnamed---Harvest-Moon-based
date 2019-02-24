using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ToolRange : MonoBehaviour {
    [HideInInspector]
    public Mesh mesh;

    public void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    public abstract Collider[] FindObjectInRange(GameObject player, int layer);

    public abstract void UpdateLine(Transform basePosition, int layer, bool visible);


}

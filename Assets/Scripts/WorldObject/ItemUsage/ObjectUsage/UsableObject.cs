using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class UsableObject : InterableObject
{
    [HideInInspector]
    public List<ToolType> usableByTools = new List<ToolType>();

}

//with the exception of some minor additions, this is not the work of GameDevStudent. The script was downloaded from here: https://github.com/masterprompt/ModelStitching 

using UnityEngine;
using System.Collections.Generic;

public class Stitcher
{
    /// <summary>
    /// Stitch clothing onto an avatar.  Both clothing and avatar must be instantiated however clothing may be destroyed after.
    /// </summary>
    /// <param name="sourceClothing"></param>
    /// <param name="targetAvatar"></param>
    ///
    /// <returns>Newly created clothing on avatar</returns>

    public GameObject Stitch(GameObject sourceClothing, GameObject targetAvatar)
    {
        TransformCatalog boneCatalog = new TransformCatalog(targetAvatar.transform);
        SkinnedMeshRenderer[] skinnedMeshRenderers = sourceClothing.GetComponentsInChildren<SkinnedMeshRenderer>();
        GameObject targetClothing = AddChild(sourceClothing, targetAvatar.transform);
        foreach (SkinnedMeshRenderer sourceRenderer in skinnedMeshRenderers)
        {
            SkinnedMeshRenderer targetRenderer = AddSkinnedMeshRenderer(sourceRenderer, targetClothing);
            targetRenderer.bones = TranslateTransforms(sourceRenderer.bones, boneCatalog);
        }
        return targetClothing;
    }

    private GameObject AddChild(GameObject source, Transform parent)
    {
        GameObject target = new GameObject(source.name);
        target.transform.parent = parent;
        target.transform.localPosition = source.transform.localPosition;
        target.transform.localRotation = source.transform.localRotation;
        target.transform.localScale = source.transform.localScale;
       /* if(source.GetComponent<PickableObject>().isTool)
        {
            target.AddComponent<PickableObject>().isTool = true;
            System.Type usageType = source.GetComponent<UsableObject>().GetType();
            System.Type toolRange = source.GetComponent<ToolRange>().GetType();
            target.AddComponent(usageType);
            target.GetComponent<UsableObject>().owner = source.GetComponent<UsableObject>().owner;
            target.GetComponent<UsableObject>().ownerName = source.GetComponent<UsableObject>().ownerName;
            target.AddComponent<MeshFilter>();
            target.AddComponent<MeshRenderer>();
            target.AddComponent(toolRange);
        }*/
        return target;
    }

    private SkinnedMeshRenderer AddSkinnedMeshRenderer(SkinnedMeshRenderer source, GameObject parent)
    {
        SkinnedMeshRenderer target = parent.AddComponent<SkinnedMeshRenderer>();
        target.sharedMesh = source.sharedMesh;
        target.materials = source.materials;
     

        return target;
    }

    private Transform[] TranslateTransforms(Transform[] sources, TransformCatalog transformCatalog)
    {
        Transform[] targets = new Transform[sources.Length];
        for (int index = 0; index < sources.Length; index++)
            targets[index] = DictionaryExtensions.Find(transformCatalog, sources[index].name);
        return targets;
    }


    #region TransformCatalog
    private class TransformCatalog : Dictionary<string, Transform>
    {
        #region Constructors
        public TransformCatalog(Transform transform)
        {
            Catalog(transform);
        }
        #endregion

        #region Catalog
        private void Catalog(Transform transform)
        {
            if (ContainsKey(transform.name))
            {
                Remove(transform.name);
                Add(transform.name, transform);
            }
            else
                Add(transform.name, transform);
            foreach (Transform child in transform)
                Catalog(child);
        }
        #endregion
    }
    #endregion


    #region DictionaryExtensions
    private class DictionaryExtensions
    {
        public static TValue Find<TKey, TValue>(Dictionary<TKey, TValue> source, TKey key)
        {
            TValue value;
            source.TryGetValue(key, out value);
            return value;
        }
    }
    #endregion

}


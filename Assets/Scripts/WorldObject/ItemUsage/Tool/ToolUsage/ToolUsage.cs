using System;
using System.Collections;
using UnityEngine;
using System.Linq;

public abstract class ToolUsage : InterableObject
{
    public String itemActionKey = "ToolAction";
    public LayerMask selectableLayer;

    public bool isUsed = false;
    public bool toolRangeVisible = true;

    protected ToolRange toolRange;
    public ToolType type;

    public void Start()
    {
        base.OnStart();
        toolRange = gameObject.GetComponent<ToolRange>();
        if (!toolRange)
        {
            Debug.LogError("Tool range not set for " + gameObject.name);
        }
    }

    public void Awake()
    {
        base.OnStart();
    }

    public void Update()
    {
        toolRange.UpdateLine(owner.transform, selectableLayer, toolRangeVisible);
        if (itemActionKey.Length > 0 && Input.GetButtonUp(itemActionKey))
        {
            InteractMyself();
        }
    }

    protected override IEnumerator Interact()
    {
        //todo will be good not to find colliders when type is not match
        Collider[] hitColliders = toolRange.FindObjectInRange(owner.gameObject, selectableLayer);
        Collider[] filterObjects = hitColliders
            .Where(collider => HitObjectFilter(collider)).ToArray();
        foreach (Collider collider in filterObjects)
        {
            yield return StartCoroutine(ToolAcion(collider));
        }
        //todo if types not match display ! mark above object?
        yield return null;
    }

    protected virtual bool HitObjectFilter(Collider collider)
    {
        return collider.GetComponent<UsableObject>() && collider.GetComponent<UsableObject>().usableByTools.Contains(type);
    }

    protected abstract IEnumerator ToolAcion(Collider collider);

    protected IEnumerator PlayAndWaitPercentFinish(float animationPercentFinished)
    {
        TriggerAnimations();

        yield return new WaitForSeconds(1f);

        float length = owner.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(length * animationPercentFinished);
    }

    public void TriggerAnimations()
    {
        if (playerRelatedAnimation.Length > 0)
        {
            owner.GetComponent<Animator>().SetTrigger(playerRelatedAnimation);
        }

        if (itemRelatedAnimation.Length > 0)
        {
            animator.SetBool(itemRelatedAnimation, true);
        }
    }
  
}

public enum ToolType
{
    Axe,
    Hoe,
    Water,
    Seed,
    Hand
}
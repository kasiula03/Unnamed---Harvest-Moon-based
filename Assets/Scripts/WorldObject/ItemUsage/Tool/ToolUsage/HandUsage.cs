using System.Collections;
using UnityEngine;

public class HandUsage : ToolUsage
{
    public PickableObject currentPicking;

    public HandUsage() : base()
    {
        itemActionKey = "HandAction";
    }

    protected override IEnumerator Interact()
    {
        base.Interact();
        Debug.Log("Use hand");
        if(currentPicking)
        {
            Debug.Log("Try to put item");
            currentPicking.InteractMyself();
            currentPicking = null;
        }
        else
        {
            yield return StartCoroutine(base.Interact());
        }
        yield return null;
    }

    protected override IEnumerator ToolAcion(Collider collider)
    {
        Debug.Log("Try to grab");
        UsableObject interable = collider.gameObject.GetComponent<UsableObject>();
        Debug.Log(interable.gameObject.name);
        foreach (ToolType toolType in interable.usableByTools)
        {
            //todo Refactor
            if (toolType == type)
            {
                if (collider.gameObject.GetComponent<PickableObject>())
                {
                    currentPicking = collider.gameObject.GetComponent<PickableObject>();
                }
                interable.InteractMyself();

            }
        }
        yield return null;
    }
}

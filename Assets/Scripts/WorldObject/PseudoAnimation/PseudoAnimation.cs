using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoAnimation : MonoBehaviour
{
    protected GameObject objectToAnimate;

    public void Awake()
    {
        objectToAnimate = gameObject;
    }

    public virtual IEnumerator ExecuteAnimation(bool right)
    {
        yield return null;
    }

    protected virtual bool ConditionForAdditionalAction()
    {
        return false;
    }

    protected virtual void AdditionalAction(bool right)
    {
        return;
    }
}

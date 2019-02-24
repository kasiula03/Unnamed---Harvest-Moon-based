using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallAnimation : PseudoAnimation
{
    private GameObject objectToRotate;
    public float requaredRotation = 360;
    public float rotationPercentToStop = 5;

    private float absoluteRotation = 0f;
    private bool additionalActionExecute = false;

    public void Awake()
    {
        base.Awake();
        objectToRotate = gameObject.GetComponentInChildren<MeshRenderer>().gameObject;
    }

    public override IEnumerator ExecuteAnimation(bool right)
    {
        absoluteRotation = requaredRotation - objectToRotate.transform.localEulerAngles.x;
        while (RotationPercent() > rotationPercentToStop)
        {
            objectToRotate.transform.Rotate(DeltaRotationBaseOnRotationPercent(right));
            if (ConditionForAdditionalAction() && !additionalActionExecute)
            {
                AdditionalAction(right);
            }
            yield return null;
        }
    }

    protected override bool ConditionForAdditionalAction()
    {
        return RotationPercent() < 30;
    }

    protected override void AdditionalAction(bool right)
    {
        ParticleSystem particle = objectToAnimate.GetComponentInChildren<ParticleSystem>();
        if (right)
        {
            Collider collider = objectToAnimate.GetComponent<Collider>();
            Vector3 position = particle.transform.position;
            position.x = position.x + 2 * collider.bounds.extents.x;
            particle.transform.Rotate(new Vector3(0, 180, 0));
            particle.transform.position = position;
        }
        var emision = particle.emission;
        emision.enabled = true;
        particle.Play();
        additionalActionExecute = true;
    }

    private Vector3 DeltaRotationBaseOnRotationPercent(bool right)
    {
        Vector3 direction = right ? Vector3.right : -Vector3.right;
        float percent = (100 - RotationPercent()) + 1;
        return direction * Time.deltaTime * 50 * (percent * 3 / 100);
    }

    private float RotationPercent()
    {
        return (requaredRotation - objectToRotate.transform.localEulerAngles.x) / absoluteRotation * 100;
    }
}

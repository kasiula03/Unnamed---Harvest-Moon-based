using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Quaternion objectRotation = animator.gameObject.transform.rotation;
        InterableObject interable = animator.gameObject.GetComponent<InterableObject>() ? animator.gameObject.GetComponent<InterableObject>() : animator.gameObject.GetComponentInParent<InterableObject>();
        objectRotation.y = interable.right ? 180 : 0;
        animator.gameObject.transform.rotation = objectRotation;
    }
}

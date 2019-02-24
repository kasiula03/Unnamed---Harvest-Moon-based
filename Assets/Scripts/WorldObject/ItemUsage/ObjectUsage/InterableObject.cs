using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterableObject : MonoBehaviour
{
    public bool right = false;

    protected Animator animator;
    protected GameObject player;

    public AutoMoveController owner;
    public String ownerName;
    public string itemRelatedAnimation;
    public string playerRelatedAnimation;

    protected abstract IEnumerator Interact();

    public void Awake()
    {
        OnStart();
    }

    protected void OnStart()
    {
        animator = GetComponent<Animator>();
        if (!animator)
        {
            animator = GetComponentInChildren<Animator>();
        }
        player = GameObject.FindGameObjectWithTag("Player");
        if (!owner && ownerName.Length > 0)
        {
            owner = GameObject.Find(ownerName).GetComponent<AutoMoveController>();
        }
    }

    public void InteractMyself()
    {
        SetInteractionSide();
        StartCoroutine(Interact());
    }

    private void SetInteractionSide()
    {
        if (player)
        {
            if (player.transform.eulerAngles.y < 180)
            {
                right = true;
            }
            else
            {
                right = false;
            }
        }
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

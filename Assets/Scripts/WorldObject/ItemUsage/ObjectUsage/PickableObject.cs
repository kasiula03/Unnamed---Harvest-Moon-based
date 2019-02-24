using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;

public class PickableObject : UsableObject {

    public bool isPicking = false;
    public bool isTool = false;
    public Item item;

    private PlayerController playerController;
    private bool objectWithParent = false;
    private Transform oldTransform;

    public void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    protected override IEnumerator Interact()
    {
        if(isPicking)
        {
            Put();
        }
        else
        {
            Pick();
        }
        yield return null;
    }

    private void Pick()
    {
        isPicking = true;
        if(gameObject.transform.parent)
        {
            oldTransform = gameObject.transform.parent;
            objectWithParent = true;
        }
        else
        {
            oldTransform = gameObject.transform;
            objectWithParent = false;
        }
        Rigidbody objRigidBody = gameObject.GetComponent<Rigidbody>();
        if (objRigidBody)
        {
            objRigidBody.useGravity = false;
            objRigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        }
        gameObject.transform.parent = playerController.transform;
        gameObject.transform.position = playerController.transform.position + new Vector3(0, playerController.GetComponent<NavMeshAgent>().height + 0.5f, 0);
        playerController.SetCurrentPickingObject(this);
    }

    private void Put()
    {
        if (oldTransform != null)
        {
            gameObject.transform.position = gameObject.transform.position - new Vector3(0, playerController.GetComponent<NavMeshAgent>().height, 0) + Vector3.forward;
            if (objectWithParent)
            {
                gameObject.transform.parent = oldTransform;
            }
            else
            {
                gameObject.transform.SetParent(null);
            }  
        }
        Rigidbody objRigidBody = gameObject.GetComponent<Rigidbody>();
        if (objRigidBody)
        {
            objRigidBody.useGravity = true;
            objRigidBody.constraints = RigidbodyConstraints.None;
        }
        playerController.SetCurrentPickingObject(null);
        isPicking = false;
    }

    public void HideInInventory()
    {
        Destroy(gameObject);
    }

}

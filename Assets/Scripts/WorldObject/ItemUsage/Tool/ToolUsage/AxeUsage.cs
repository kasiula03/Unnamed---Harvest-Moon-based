using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AxeUsage : ToolUsage
{
    protected override IEnumerator ToolAcion(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Tree"))
        {
            MovePlayerToRequiredPosition(collider);
        }
        yield return null;
    }

    private void MovePlayerToRequiredPosition(Collider collider)
    {
        NavMeshAgent agent = owner.GetComponent<NavMeshAgent>();
        Ray ray = new Ray(owner.transform.position, collider.transform.position - owner.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10))
        {
            Vector3 playerPosition = Vector3.zero;
            Vector3 normal = hit.normal;
            normal = hit.transform.TransformDirection(normal);
            Debug.Log(hit.normal);
            if (normal == hit.transform.forward)
            {
                playerPosition = -Vector3.forward;
            }
            else if (normal == -hit.transform.forward)
            {
                playerPosition = Vector3.forward;
            }
            else if (normal == hit.transform.right)
            {
                playerPosition = -Vector3.right;
            }
            else if (normal == -hit.transform.right)
            {
                playerPosition = Vector3.right;
            }

            Vector3 destinationPosition = collider.bounds.center - 3.3f * playerPosition;
            owner.currentDestination = collider.gameObject.GetComponent<DestructableObject>();
            owner.pointToFace = collider.bounds.center;
            owner.pointToFace.y = owner.transform.position.y;
            owner.GetComponent<Animator>().SetFloat("Speed", 0.2f);
            owner.currentUsingItem = this;

            agent.SetDestination(destinationPosition);
            agent.isStopped = false;

        }
    }
}

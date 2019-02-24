using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : UsableObject
{
    public GameObject dropItem;
    public int amountItem = 2;
    public int durability = 1;

    private string animationName = "Destroy";

    protected override IEnumerator Interact()
    {
        durability--;

        if (durability == 0)
        {
            animator.SetBool(animationName, true);

            // to make sure that animation starts. Has exit time should be uncheck
            
            yield return new WaitForSeconds(1f);

            float length = animator.GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(length * 0.6f);

            ParticleSystem particle = gameObject.GetComponentInChildren<ParticleSystem>();
            var emision = particle.emission;
            emision.enabled = true;
            particle.Play();

            yield return new WaitForSeconds(4.5f);

            if (dropItem != null)
            {
                for (int i = 0; i < amountItem; i++)
                {
                    Instantiate(dropItem, transform.position + Vector3.up * 2, Random.rotation);
                }
            }
            Debug.Log("Object was destroyed");
            Destroy(gameObject);
        }
    }

}

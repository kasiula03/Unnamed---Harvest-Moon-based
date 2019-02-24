using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObjectWithPseudoAnimation : DestructableObject
{
    private PseudoAnimation pseudoAnimation;

    public void Awake()
    {
        base.OnStart();
        pseudoAnimation = gameObject.GetComponent<PseudoAnimation>();
    }

    protected override IEnumerator Interact()
    {
        durability--;

        if (durability == 0)
        {
            yield return new WaitForSeconds(1f);
            
            yield return StartCoroutine(pseudoAnimation.ExecuteAnimation(right));       

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

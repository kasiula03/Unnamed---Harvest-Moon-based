using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableObject : UsableObject
{
    public Canvas harvestReadyCanvas;

    private Crop crop;
    
    public HarvestableObject()
    {
        //usageAvailableTags.Add("Plant");
    }

    public new void Awake()
    {
        base.OnStart();
        crop = gameObject.GetComponent<Crop>();
    }

    public void Update()
    {
        if(crop.harvestable && !harvestReadyCanvas.gameObject.activeInHierarchy)
        {
            harvestReadyCanvas.gameObject.SetActive(true);
        }
    }

    protected override IEnumerator Interact()
    {
        Debug.Log("Try tu harvest");
        yield return null;
    }

 
}

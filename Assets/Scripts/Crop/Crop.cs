using System;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour {

    public PickableObject crop;
    public List<Stage> stages = new List<Stage>();
    public int dayToRoot = 10;

    public int currentCropDay = 0;
    public int currentStage = 0;

    public List<Season> availableSeasonToGrow;

    [SerializeField]
    private bool isWatered = false;
    public bool rotted = false;
    public bool harvestable = false;

    [Serializable]
    public class Stage
    {
        public GameObject stageModel;
        public int dayOfStage;
    }

	void Start () {
        stages.ForEach(stage => stage.stageModel.SetActive(false));
        stages[0].stageModel.SetActive(true);
	}
	
    public void AddDay()
    {
        if(isWatered)
        {
            currentCropDay++;
        }
        if (IsNotFinalStage() && StageDayReach())
        {
            currentStage++;
            stages[currentStage - 1].stageModel.SetActive(false);
            stages[currentStage].stageModel.SetActive(true);
            currentCropDay = 0;
            Debug.Log("Next stage");
        }
        if(currentStage == stages.Count - 1)
        {
            harvestable = true;
        }
        if(currentCropDay >= dayToRoot)
        {
            rotted = true;
            // Add rotten stage
        }
        isWatered = false;
    }

    public void Water()
    {
        isWatered = true;
    }

    public void Harvest()
    {
        if(harvestable && crop != null)
        {
            Instantiate(crop, gameObject.transform).InteractMyself();
            // maybe some pick and hide method?
        }
        Destroy(gameObject);
    }

    private bool IsNotFinalStage()
    {
        return stages[currentStage].dayOfStage > 0 && currentStage + 1 < stages.Count;
    }

    private bool StageDayReach()
    {
        return stages[currentStage].dayOfStage <= currentCropDay;
    }
}

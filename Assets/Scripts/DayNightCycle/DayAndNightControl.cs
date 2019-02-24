using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class DayColors
{
	public Color skyColor;
	public Color equatorColor;
	public Color horizonColor;
}

public class DayAndNightControl : MonoBehaviour
{
    public TimeData worldTimeData;
    public CalendarData calendarData;

	public GameObject StarDome;
	public GameObject moonState;
	public GameObject moon;

	public DayColors dawnColors;
	public DayColors dayColors;
	public DayColors nightColors;

	public Light sunlight;

	public bool showUI;
	float lightIntensity; 
	Material starMat;

	Camera targetCam;

    public Text dayText;
    public Text timeText;
    public Text timeOfDayText;
    public Text seasonText;

    public Image calendarRound;

    void Start () {

		foreach (Camera camera in GameObject.FindObjectsOfType<Camera>())
		{
			if (camera.isActiveAndEnabled) {
				targetCam = camera;
			}
		}
		lightIntensity = sunlight.intensity;
		starMat = StarDome.GetComponentInChildren<MeshRenderer> ().material;
		if (worldTimeData.startDayTime.Equals(DayTime.Morning)) {
            worldTimeData.currentTime = 0.3f;
			starMat.color = new Color(1f,1f,1f,0f);
		}
        calendarRound.rectTransform.anchoredPosition = calendarData.CalendarRoundPosition();
    }

    void Update () {
        UpdateLight();
        worldTimeData.currentTime += (Time.deltaTime / worldTimeData.getDayLong()) * worldTimeData.timeMultiplier;
		if (worldTimeData.currentTime >= 1) {
            worldTimeData.currentTime = 0;
           // calendarData.AddDay();
          //  seasonText.text = calendarData.CurrentSeason().ToString();
           //// calendarRound.rectTransform.anchoredPosition = calendarData.CalendarRoundPosition();
           // Crop[] crops = FindObjectsOfType<Crop>();
           // Array.ForEach(crops, crop => crop.AddDay());

		}
      //  SetTimeUI();
    }

	void UpdateLight()
	{
		//StarDome.transform.Rotate (new Vector3 (2f * Time.deltaTime, 0, 0));
		//moon.transform.LookAt (targetCam.transform);

        float result = ((1 - worldTimeData.currentTime) * 360f);


        sunlight.transform.Rotate(1, 0, 0, Space.World);
        // sunlight.transform.localRotation = Quaternion.Euler (result, 0, 0);
        /*moonState.transform.localRotation = Quaternion.Euler ((worldTimeData.currentTime * 360f) - 100, 0, 0);
		//^^ we rotate the sun 360 degrees around the x axis, or one full rotation times the current time variable. we subtract 90 from this to make it go up
		//in increments of 0.25.

		//the 170 is where the sun will sit on the horizon line. if it were at 180, or completely flat, it would be hard to see. Tweak this value to what you find comfortable.

		float intensityMultiplier = 1;

		if (worldTimeData.currentTime <= 0.23f || worldTimeData.currentTime >= 0.75f) 
		{
			intensityMultiplier = 0.2f; //when the sun is below the horizon, or setting, the intensity needs to be 0 or else it'll look weird
			starMat.color = new Color(1,1,1,Mathf.Lerp(1,0,Time.deltaTime));
		}
		else if (worldTimeData.currentTime <= 0.25f) 
		{
			intensityMultiplier = Mathf.Clamp01((worldTimeData.currentTime - 0.23f) * (1 / 0.02f));
			starMat.color = new Color(1,1,1,Mathf.Lerp(0,1,Time.deltaTime));
		}
		else if (worldTimeData.currentTime <= 0.73f) 
		{
			intensityMultiplier = Mathf.Clamp01(1 - ((worldTimeData.currentTime - 0.73f) * (1 / 0.02f)));
		}

		if (worldTimeData.currentTime <= 0.2f) {
			RenderSettings.ambientSkyColor = nightColors.skyColor;
			RenderSettings.ambientEquatorColor = nightColors.equatorColor;
			RenderSettings.ambientGroundColor = nightColors.horizonColor;
		}
		if (worldTimeData.currentTime > 0.2f && worldTimeData.currentTime < 0.4f) {
			RenderSettings.ambientSkyColor = dawnColors.skyColor;
			RenderSettings.ambientEquatorColor = dawnColors.equatorColor;
			RenderSettings.ambientGroundColor = dawnColors.horizonColor;
		}
		if (worldTimeData.currentTime > 0.4f && worldTimeData.currentTime < 0.75f) {
			RenderSettings.ambientSkyColor = dayColors.skyColor;
			RenderSettings.ambientEquatorColor = dayColors.equatorColor;
			RenderSettings.ambientGroundColor = dayColors.horizonColor;
		}
		if (worldTimeData.currentTime > 0.75f) {
			RenderSettings.ambientSkyColor = dayColors.skyColor;
			RenderSettings.ambientEquatorColor = dayColors.equatorColor;
			RenderSettings.ambientGroundColor = dayColors.horizonColor;
		}

        //sunlight.intensity = lightIntensity * intensityMultiplier;
        //sunlight.intensity = 0.2f;*/

    }

    public void SetTimeUI()
    {
        dayText.text = "Day: " + calendarData.currentDay;
        timeText.text = "Hour: " + worldTimeData.FormatTime();
        timeOfDayText.text = worldTimeData.TimeOfDay().ToString();
    }

}

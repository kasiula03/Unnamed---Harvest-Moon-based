using System;
using UnityEngine;
using UnityEngine.UI;

public class DayNightController : MonoBehaviour
{
    public Light sunlight;
    public Light moonLight;
    public TimeData worldTimeData;
    public CalendarData calendarData;

    public DayColors dayColors;
    public DayColors nightColors;
    public DayColors dawnColors;

    public Text dayText;
    public Text timeText;
    public Text timeOfDayText;
    public Text seasonText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        worldTimeData.currentTime += (Time.deltaTime / worldTimeData.getDayLong()) * worldTimeData.timeMultiplier;
        if (worldTimeData.currentTime >= 1)
        {
            worldTimeData.currentTime = 0;
            calendarData.AddDay();
            seasonText.text = calendarData.CurrentSeason().ToString();
            // calendarRound.rectTransform.anchoredPosition = calendarData.CalendarRoundPosition();
            Crop[] crops = FindObjectsOfType<Crop>();
            Array.ForEach(crops, crop => crop.AddDay());

        }

        float rot = Mathf.Lerp(0, 360, worldTimeData.currentTime);
        float moonlightRot = Mathf.Lerp(0, 360, 1 - worldTimeData.currentTime);
        sunlight.transform.rotation = Quaternion.Euler(rot, 0, 0);
        moonLight.transform.rotation = Quaternion.Euler(moonlightRot, 0, 0);

        SetTimeUI();
        if (worldTimeData.currentTime >= 0.52f)
        {
            RenderSettings.ambientSkyColor = nightColors.skyColor;
            RenderSettings.ambientEquatorColor = nightColors.equatorColor;
            RenderSettings.ambientGroundColor = nightColors.horizonColor;
        }
        else if (worldTimeData.currentTime >= 0f && worldTimeData.currentTime <= 0.45f)
        {
            RenderSettings.ambientSkyColor = dayColors.skyColor;
            RenderSettings.ambientEquatorColor = dayColors.equatorColor;
            RenderSettings.ambientGroundColor = dayColors.horizonColor;
        }
        else if (worldTimeData.currentTime >= 0.45f && worldTimeData.currentTime < 0.52f)
        {
            RenderSettings.ambientSkyColor = dawnColors.skyColor;
            RenderSettings.ambientEquatorColor = dawnColors.equatorColor;
            RenderSettings.ambientGroundColor = dawnColors.horizonColor;
        }


        /*if (worldTimeData.currentTime <= 0.2f)
        {
            RenderSettings.ambientSkyColor = nightColors.skyColor;
            RenderSettings.ambientEquatorColor = nightColors.equatorColor;
            RenderSettings.ambientGroundColor = nightColors.horizonColor;
        }
        if (worldTimeData.currentTime > 0.2f && worldTimeData.currentTime < 0.4f)
        {
            RenderSettings.ambientSkyColor = dawnColors.skyColor;
            RenderSettings.ambientEquatorColor = dawnColors.equatorColor;
            RenderSettings.ambientGroundColor = dawnColors.horizonColor;
        }
        if (worldTimeData.currentTime > 0.4f && worldTimeData.currentTime < 0.75f)
        {
            RenderSettings.ambientSkyColor = dayColors.skyColor;
            RenderSettings.ambientEquatorColor = dayColors.equatorColor;
            RenderSettings.ambientGroundColor = dayColors.horizonColor;
        }
        if (worldTimeData.currentTime > 0.75f)
        {
            RenderSettings.ambientSkyColor = dayColors.skyColor;
            RenderSettings.ambientEquatorColor = dayColors.equatorColor;
            RenderSettings.ambientGroundColor = dayColors.horizonColor;
        }*/
    }

    public void SetTimeUI()
    {
        dayText.text = "Day: " + calendarData.currentDay;
        timeText.text = "Hour: " + worldTimeData.FormatTime();
        timeOfDayText.text = worldTimeData.TimeOfDay().ToString();
    }
}

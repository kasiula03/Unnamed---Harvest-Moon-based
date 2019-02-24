using UnityEngine;

[System.Serializable]
public enum DayTime
{
    Midnight,
    Morning,
    MidNoon,
    Evening,
    Night
}

[System.Serializable]
[CreateAssetMenu(fileName = "WorldTimeData", menuName = "Time/WorldTime", order = 1)]
public class TimeData : ScriptableObject
{
    public float timeMultiplier = 1f;
    [Range(0, 1)]
    public float currentTime = 0;
    public DayTime startDayTime = DayTime.Morning;
    [SerializeField]
    private float dayLongInSeconds = 120f;

    public float getDayLong()
    {
        return dayLongInSeconds;
    }

    public DayTime TimeOfDay()
    {
        if (currentTime >= 0f && currentTime < 0.1f)
        {
            return DayTime.Midnight;
        }
        else if (currentTime >= 0.1f && currentTime < 0.5f)
        {
            return DayTime.Morning;

        }
        else if (currentTime >= 0.5f && currentTime < 0.6f)
        {
            return DayTime.MidNoon;
        }
        else if (currentTime >= 0.6f && currentTime < 0.8f)
        {
            return DayTime.Evening;
        }
        else
        {
            return DayTime.Night;
        }
    }

    public string FormatTime()
    {
        float realTime = currentTime * 24;
        int hour = (int) realTime;
        int minutes = (int) ((realTime - hour) * 60);
        return hour.ToString() + " : " + minutes.ToString();
    }
}
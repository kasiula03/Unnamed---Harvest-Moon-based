using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Season
{
    Winter = 0,
    Spring = 1,
    Summer = 2,
    Fall = 3,
    NumberOfTypes
}

[System.Serializable]
[CreateAssetMenu(fileName = "CalendarData", menuName = "Time/Calendar", order = 1)]
public class CalendarData : ScriptableObject {

    public static int monthLength = 30;
    public TimeData timeData;
    public int currentDay = 1;
    public int year = 1;
    public int yearLenth = monthLength * (int) Season.NumberOfTypes;
    public Season currentSeason = Season.Spring;

    private Season startSeason = Season.Spring;

    public Season CurrentSeason()
    {
        return currentSeason;
    }

    public Vector2 CalendarRoundPosition()
    {
        int month = Mathf.FloorToInt(currentDay - 1) / 31;
        int tempDay = (currentDay - 1) - month * 30;
        int row = Mathf.FloorToInt(tempDay / 7);
        int column = tempDay - 7 * row;
        return new Vector2(6 + 63 * column, -53 - 57 * row);
    }

    public void AddDay()
    {
        currentDay++;
        ChangeDateIfNeeded();
    }

    private void ChangeDateIfNeeded()
    {
        int pastYear = (int)(currentDay / yearLenth);
        if(pastYear > 0)
        {
            year += pastYear;
            currentDay = 1;
        }
   
        int months = (currentDay / 31) + 1;
        currentSeason = (Season)(months % 4);
    }
}

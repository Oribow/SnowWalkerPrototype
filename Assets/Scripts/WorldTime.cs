using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTime : MonoBehaviour {

    public Material skyMat;
    public Gradient gradient;
    public UnityEngine.UI.Text timeOfDayText;

    public float timeUnitPerMinute;
    public static float worldTime = 0;
    public static int worldTimeInHours = 0;
    private static bool isPaused = false;

    void Awake()
    {
        WrapTimeTo(8);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
            return;
        WrapTime(Time.deltaTime * timeUnitPerMinute);
        skyMat.color = gradient.Evaluate((worldTime % 1440) / 1440);
        timeOfDayText.text = ConvertToDateTimeString(worldTimeInHours);
    }

    public static void WrapTime(float amount)
    {
        worldTime += amount;
        worldTimeInHours = (int)worldTime / 60;

        DebugPanel.Log("WorldTime", ConvertToTimeStringWithMins(worldTime));
    }

    public static void WrapTimeTo(int hourOfDay)
    {
        int dayTime = worldTimeInHours % 24;
        int amount = 0;
        if (dayTime >= hourOfDay)
        {
            amount = (24 - dayTime) + hourOfDay;
        }
        else
        {
            amount = hourOfDay - dayTime;
        }
        worldTimeInHours += amount;
        worldTime = worldTimeInHours * 60;

        DebugPanel.Log("WorldTime", ConvertToTimeStringWithMins(worldTime));
    }

    

    public static void Pause()
    {
        isPaused = true;
    }

    public static void Resume()
    {
        isPaused = false;
    }

    public static string ConvertToTimeString(int time)
    {
        float t = time;
        float hours = t % 24;
        t -= hours;
        t /= 24;
        float days = t;
        return (int)days + "d" + (int)hours + "h";
    }

    public static string ConvertToDateTimeString(int time)
    {
        float t = time;
        float hours = t % 24;
        t -= hours;
        t /= 24;
        float days = t;
        return (int)hours + ":00";
    }

    public static string ConvertToTimeStringWithMins(float time)
    {
        float t = time;
        float minutes = t % 60;
        t -= minutes;
        t /= 60;
        float hours = t % 24;
        t -= hours;
        t /= 24;
        float days = t;
        return (int)days + "d" + (int)hours + "h"+(int)minutes+"m";
    }
}

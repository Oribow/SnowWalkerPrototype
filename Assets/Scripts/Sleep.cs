using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : MonoBehaviour
{
    public UnityEngine.UI.Button sleepButton;
    public UnityEngine.UI.Text sleepButtonText;
    public int canSleepFrom;
    public int sleepsTo;

    // Update is called once per frame
    void Update()
    {
        int dayTime = WorldTime.worldTimeInHours % 24;

        if (dayTime >= canSleepFrom && dayTime <= sleepsTo)
        {
            sleepButtonText.text = "";
        }
        else
        {
            sleepButtonText.text = "Can't Sleep";
        }
    }


}

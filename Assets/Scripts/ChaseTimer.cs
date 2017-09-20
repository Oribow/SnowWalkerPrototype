using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTimer : MonoBehaviour {
    public int chaseEndTime;
    public UnityEngine.UI.Text textUI;
    public string chaserName;
	
	// Update is called once per frame
	void Update () {
        int timeLeft = Mathf.Max(0, chaseEndTime - WorldTime.worldTimeInHours);
        string timeString = WorldTime.ConvertToTimeString(timeLeft);
        textUI.text = chaserName + ": " + timeString;
    }
}

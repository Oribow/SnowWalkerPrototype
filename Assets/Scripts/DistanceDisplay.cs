using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDisplay : MonoBehaviour {

    public UnityEngine.UI.Text textUI;
    public float unitsPerMeter = 1;
    public Transform start;
    public Transform end;
    public Transform player;

    [HideInInspector]
    public float distanceTraveled;

	// Update is called once per frame
	void Update () {
        distanceTraveled = ((int)Mathf.Clamp(player.position.x - start.position.x, start.position.x, end.position.x - start.position.x) * unitsPerMeter);
        textUI.text = distanceTraveled +"m/"+ (end.position.x - start.position.x)+"m";
	}
}

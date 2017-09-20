using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FirePlace : MonoBehaviour {

    public GameObject sleepButtonRoot;
    public UnityEngine.UI.Text sleepButtonText;
    public int canSleepFrom;
    public int sleepsTo;

    private bool playerIsIn;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            sleepButtonRoot.gameObject.SetActive(true);
            playerIsIn = true;
        }
    }

    void Update()
    {
        if (playerIsIn)
        {
                int dayTime = WorldTime.worldTimeInHours % 24;

                if (dayTime >= canSleepFrom || (dayTime >= 0 && dayTime <= sleepsTo))
                {
                    if (dayTime >= canSleepFrom)
                        dayTime = 24 - dayTime + sleepsTo;
                    else
                    {
                        dayTime = sleepsTo - dayTime;
                    }

                    sleepButtonText.text = "Sleep for " + dayTime + "h";
                }
                else
                {
                    sleepButtonText.text = "Can't Sleep";
                }
            }
        
    }

    public void Sleep()
    {
        WorldTime.WrapTimeTo(sleepsTo);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            sleepButtonRoot.gameObject.SetActive(false);
            playerIsIn = false;
        }
    }
}

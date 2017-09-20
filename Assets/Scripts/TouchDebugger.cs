using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDebugger : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Ended)
                continue;
            Vector2 pos = Camera.main.ScreenToWorldPoint(t.position);
            DebugExtension.DebugCircle(pos, Vector3.forward, Color.red, 0.4f, 2);
        }
    }
}

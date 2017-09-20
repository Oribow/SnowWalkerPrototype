using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrthoCameraManager : MonoBehaviour
{
    public float horizontalResolution = 1920;
    Camera camera;

    void OnGUI()
    {
        if(camera == null)
                camera = GetComponent<Camera>();
        float currentAspect = (float)Screen.width / (float)Screen.height;
        camera.orthographicSize = horizontalResolution / currentAspect / 200;
    }
}



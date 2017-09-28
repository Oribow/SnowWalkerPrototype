using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {
    public Transform target;
    public float speed;

    private void LateUpdate()
    {
        Vector3 newPos = Vector2.Lerp(transform.position, target.position, Time.deltaTime * speed);
        newPos.z = -4;
        transform.position = newPos;
    }
}

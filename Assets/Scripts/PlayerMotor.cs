using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public CC2D.CharacterController2D motor;
    public float speedX = 5;
    public float gravity = -9;
    public float jump = 5;
    public float dashSpeed;
    public float dashDistance;
    [HideInInspector]
    public List<float> speedMultipliers;

    public IMotorInput motorInput;

    private float dashTimeStamp;
    private int dashDir;

    void Start()
    {
        speedMultipliers = new List<float>(4);
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 deltaMove = Vector3.zero;

        if (dashTimeStamp - Time.time > 0)
        {
            //Dash
            deltaMove.x = dashDir * dashSpeed;
        }
        else
        {
            //Normal walking
            deltaMove.x = motorInput.GetHorizontalAxis() * speedX;
            foreach (float i in speedMultipliers)
            {
                deltaMove.x *= i;
            }

            deltaMove.y = gravity * Time.deltaTime + motor.velocity.y;

            if (motorInput.ShouldJump() && motor.isGrounded)
                deltaMove.y += jump;
        }
        motor.move(deltaMove * Time.deltaTime, false);

        DebugPanel.Log("Player Speed", motor.velocity);
    }

    public void Dash(int dir)
    {
       
        if (dashTimeStamp - Time.time > 0)
            return; //Already perfoming a dash

        dashTimeStamp = Time.time + dashDistance / dashSpeed;
        dashDir = dir;
    }
}

public interface IMotorInput
{
    float GetHorizontalAxis();
    bool ShouldJump();
}

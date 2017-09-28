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
    public int facedDir;

    public IMotorInput motorInput;
    public Transform spriteRoot;

    private float dashTimeStamp;
    private int dashDir;

    void Start()
    {
        speedMultipliers = new List<float>(4);
        facedDir = (int)Mathf.Sign(transform.localScale.x);
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
        AdjustFacingDir(deltaMove.x);
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

    private void AdjustFacingDir(float velocity)
    {
        if (Mathf.Sign(spriteRoot.localScale.x) != Mathf.Sign(velocity))
        {
            Vector3 scale = spriteRoot.localScale;
            scale.x *= -1;
            spriteRoot.localScale = scale;
            facedDir = (int)Mathf.Sign(velocity);
        }
    }
}

public interface IMotorInput
{
    float GetHorizontalAxis();
    bool ShouldJump();
}

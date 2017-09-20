using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour, IMotorInput
{
    public PlayerMotor playerMotor;
    public float xBorderLeftRight;
    public float xBorderRightGesture;

    private float horizontalAxis;
    private bool shouldJump;

    private bool pressLeft;
    private bool pressRight;

    private void Start()
    {
        TouchKit.instance.designTimeResolution = new Vector2(800, 480);
        Debug.Log(Camera.main);

        TKAnyTouchRecognizer tapLeft = new TKAnyTouchRecognizer(new TKRect(0, 0, xBorderLeftRight, 480));
        tapLeft.onEnteredEvent += TapLeft_onEnteredEvent;
        tapLeft.onExitedEvent += TapLeft_onExitedEvent;

        TKAnyTouchRecognizer tapRight = new TKAnyTouchRecognizer(new TKRect(xBorderLeftRight, 0, xBorderRightGesture - xBorderLeftRight, 480));
        tapRight.onEnteredEvent += TapRight_onEnteredEvent;
        tapRight.onExitedEvent += TapRight_onExitedEvent;

        TKSwipeRecognizer swipeDash = new TKSwipeRecognizer(1);
        swipeDash.boundaryFrame = new TKRect(xBorderRightGesture, 0, 800 - xBorderRightGesture, 480);
        swipeDash.gestureRecognizedEvent += SwipeDash_gestureRecognizedEvent;
        swipeDash.timeToSwipe = 0;
        swipeDash.TrackTouchesStartedOutOfBounds = true;
        

        TouchKit.addGestureRecognizer(tapRight);
        TouchKit.addGestureRecognizer(tapLeft);
        TouchKit.addGestureRecognizer(swipeDash);        

        playerMotor.motorInput = this;
    }

    private void SwipeDash_gestureRecognizedEvent(TKSwipeRecognizer obj)
    {
        if (obj.completedSwipeDirection == TKSwipeDirection.Left)
        {
            //Dash left
            playerMotor.Dash(-1);
        }
        else if (obj.completedSwipeDirection == TKSwipeDirection.Right)
        {
            //Dash right
            playerMotor.Dash(1);
        }
    }

    private void TapRight_onExitedEvent(TKAnyTouchRecognizer obj)
    {
        pressRight = false;
        if (pressLeft)
            horizontalAxis = -1;
        else
            horizontalAxis = 0;
    }

    private void TapRight_onEnteredEvent(TKAnyTouchRecognizer obj)
    {
        pressRight = true;
        horizontalAxis = 1;
    }

    private void TapLeft_onExitedEvent(TKAnyTouchRecognizer obj)
    {
        pressLeft = false;
        if (pressRight)
            horizontalAxis = 1;
        else
            horizontalAxis = 0;
    }

    private void TapLeft_onEnteredEvent(TKAnyTouchRecognizer obj)
    {
        pressLeft = true;
        horizontalAxis = -1;
    }

    public float GetHorizontalAxis()
    {
        return horizontalAxis;
    }

    public bool ShouldJump()
    {
        return false;
    }
}

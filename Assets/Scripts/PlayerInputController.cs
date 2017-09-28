using Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour, IMotorInput
{
    public PlayerMotor playerMotor;
    public float xBorderLeftRight;
    public float xBorderRightGesture;
    public float yBorderJumpGesture;
    public Transform nearestEnemyRayStart;
    public Animator animator;
    public IHealth health;

    //Auto Attack
    public float autoAttackRange = 3;
    public LayerMask attackLayer;
    public float autoAttackCooldown;
    public float autoAttackDamage;

    private float horizontalAxis;
    private bool shouldJump;

    private bool pressLeft;
    private bool pressRight;

    private bool shouldBlock;

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

        TKLongPressRecognizer block = new TKLongPressRecognizer(1, 4, 1);
        block.boundaryFrame = new TKRect(xBorderRightGesture, 0, 800 - xBorderRightGesture, 480);
        block.cancelAfterRecognitionWhenOutOfBounds = true;
        block.ignoreMovementAfterRecognition = true;
        block.gestureRecognizedEvent += Block_gestureRecognizedEvent;
        block.gestureCompleteEvent += Block_gestureCompleteEvent;

        TKTapRecognizer jump = new TKTapRecognizer();
        jump.boundaryFrame = new TKRect(xBorderRightGesture, yBorderJumpGesture, 800 - xBorderRightGesture, 480 - yBorderJumpGesture);
        jump.gestureRecognizedEvent += Jump_gestureRecognizedEvent;

        TouchKit.addGestureRecognizer(tapRight);
        TouchKit.addGestureRecognizer(tapLeft);
        TouchKit.addGestureRecognizer(swipeDash);
        TouchKit.addGestureRecognizer(block);
        TouchKit.addGestureRecognizer(jump);

        playerMotor.motorInput = this;
        health.OnDeath += Health_OnDeath;
    }

    private void Health_OnDeath(object sender, IDamageInfo e)
    {
        this.enabled = false;
    }

    private void Jump_gestureRecognizedEvent(TKTapRecognizer obj)
    {
        Debug.Log("Player jumps");
        shouldJump = true;
    }

    private void Block_gestureCompleteEvent(TKLongPressRecognizer obj)
    {
        shouldBlock = false;
        Debug.Log("Player ends blocking");
    }

    private void Block_gestureRecognizedEvent(TKLongPressRecognizer obj)
    {
        shouldBlock = true;
        Debug.Log("Player starts blocking");
    }

    private void SwipeDash_gestureRecognizedEvent(TKSwipeRecognizer obj)
    {
        if (shouldBlock)
            return;

        if (obj.completedSwipeDirection == TKSwipeDirection.Left)
        {
            //Dash left
            Debug.Log("Player dashes left");
            playerMotor.Dash(-1);
        }
        else if (obj.completedSwipeDirection == TKSwipeDirection.Right)
        {
            //Dash right
            Debug.Log("Player dashes right");
            playerMotor.Dash(1);
        }
        else if ((obj.completedSwipeDirection & TKSwipeDirection.TopSide) > 0)
        {
            Debug.Log("Player performs Attack 1");
        }
        else if ((obj.completedSwipeDirection & TKSwipeDirection.BottomSide) > 0)
        {
            Debug.Log("Player performs Attack 2");
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

    private float attackTimeStamp;

    private void Update()
    {
        GameObject autoAttackTarget = GetFirstEnemyInRange(autoAttackRange);
        if (autoAttackTarget != null)
        {
            if (Time.time - attackTimeStamp >= autoAttackCooldown)
            {
                attackTimeStamp = Time.time;
                animator.SetTrigger("Knife_Attack");
                autoAttackTarget.SendMessage("TakeDamage", new BasicDamageInfo(IDamageInfo.DamageTyp.Melee, autoAttackDamage), SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private GameObject GetFirstEnemyInRange(float range)
    {
        RaycastHit2D hit = Physics2D.Raycast(nearestEnemyRayStart.position, Vector2.right * playerMotor.facedDir, range, attackLayer);
        if (hit.collider == null)
            return null;
        return hit.collider.gameObject;
    }

    public float GetHorizontalAxis()
    {
        return horizontalAxis;
    }

    public bool ShouldJump()
    {
        var tmp = shouldJump;
        shouldJump = false;
        return tmp;
    }

}

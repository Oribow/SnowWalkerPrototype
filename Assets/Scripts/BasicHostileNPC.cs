using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicHostileNPC : MonoBehaviour
{

    protected enum BrainState
    {
        Idle,
        Pursue,
        Attack,
        Dead,
        Comatose,
    }

    public float sightRad;
    public float attackRad;
    public float speed;
    public float maxDistFromSpawn;
    public Animator animator;

    protected GameObject player;
    float spawnX;

    protected float currentSpeed;
    protected BrainState brainState;


    // Use this for initialization
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {

        currentSpeed = 0;
        if (brainState == BrainState.Comatose)
            return;
        if (brainState == BrainState.Dead)
        {
            Dead();
            return;
        }

        LookOutForEnemy();

        switch (brainState)
        {
            case BrainState.Idle:
                Idle();
                break;
            case BrainState.Attack:
                Attack();
                break;
            case BrainState.Pursue:
                Pursue();
                break;
        }
    }

    private void LateUpdate()
    {
        animator.SetFloat("Speed", currentSpeed);
    }

    protected void LookOutForEnemy()
    {
        float dist = (player.transform.position - transform.position).magnitude;
        int dir = dist > 0 ? 1 : -1;
        dist = Mathf.Abs(dist);
        if (dist <= attackRad)
        {
            brainState = BrainState.Attack;
        }
        else if (dist <= sightRad)
        {
            brainState = BrainState.Pursue;
        }
        else
        {
            brainState = BrainState.Idle;
        }
    }

    private float timeStamp;
    private bool wasMoving;
    private bool moveToSpawn;
    private int moveDir;
    protected virtual void Idle()
    {
        float spawnDist = spawnX - transform.position.x;
        int dir = spawnDist > 0 ? 1 : -1;
        spawnDist *= dir;
        if (spawnDist > maxDistFromSpawn || moveToSpawn)
        {
            moveToSpawn = true;
            transform.Translate(Vector3.right * dir * speed * Time.deltaTime);
            currentSpeed = 1;
            if (Mathf.Sign(animator.transform.localScale.x) == dir)
            {
                Vector3 scale = animator.transform.localScale;
                scale.x *= -1;
                animator.transform.localScale = scale;
            }
            if (spawnDist < 1)
            {
                moveToSpawn = false;
            }
        }
        else
        {
            if (timeStamp - Time.time <= 0)
            {
                if (wasMoving)
                {
                    timeStamp = Time.time + Random.Range(2, 4);
                    wasMoving = false;
                    currentSpeed = 0;
                }
                else
                {
                    moveDir = Random.value > 0.5f ? 1 : -1;
                    timeStamp = Time.time + Random.Range(0.6f, 2);
                    wasMoving = true;
                }
            }
            if (wasMoving)
            {
                transform.Translate(Vector3.right * moveDir * speed * 0.5f * Time.deltaTime);
                currentSpeed = 1;
                if (Mathf.Sign(animator.transform.localScale.x) == moveDir)
                {
                    Vector3 scale = animator.transform.localScale;
                    scale.x *= -1;
                    animator.transform.localScale = scale;
                }
            }
        }
    }

    protected virtual void Attack()
    {

    }

    protected virtual void Pursue()
    {
        int dir = player.transform.position.x - transform.position.x > 0 ? 1 : -1;
        transform.Translate(Vector3.right * dir * speed * Time.deltaTime);
        currentSpeed = 1;
        if (Mathf.Sign(animator.transform.localScale.x) == dir)
        {
            Vector3 scale = animator.transform.localScale;
            scale.x *= -1;
            animator.transform.localScale = scale;
        }
    }

    protected virtual void Dead()
    {

    }

    protected virtual void KillMe()
    {
        animator.SetTrigger("Die");
        Destroy(gameObject, 2);
        brainState = BrainState.Dead;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRad);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRad);
        Gizmos.color = Color.blue;
        if (spawnX == 0)
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z), maxDistFromSpawn);
        else
            Gizmos.DrawWireSphere(new Vector3(spawnX, transform.position.y, transform.position.z), maxDistFromSpawn);
    }
}

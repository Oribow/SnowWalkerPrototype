using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour {

    protected enum ControllerStatus
    {
        Idle,
        Attack,
        Parry,
        Dodge,
    }

    //Stamina
    public float maxStamina;
    public float staminaRegRate;
    public float staminaRegWarmUp;

    //Attack
    public float attackStaminaCost;
    public AnimationClip attackAnim;
    public float damage;
    public float hitZoneLength;

    //Parry
    public AnimationClip parryAnim;
    public float staminaPerDamage;

    //Dodge
    public AnimationClip dodgeAnim;
    public float dodgeStaminaCost;

    protected float stamina;
    float timeStamp;
    protected ControllerStatus cStatus;
    Animation animator;

    void Awake()
    {
        this.animator = GetComponent<Animation>();
        stamina = maxStamina;
        Idle();
    }

    protected void Think()
    {
        switch (cStatus)
        {
            case ControllerStatus.Idle:
                if (Time.time - timeStamp >= staminaRegWarmUp)
                {
                    stamina = Mathf.Min(maxStamina, stamina + staminaRegRate * Time.deltaTime);
                }
                break;
            case ControllerStatus.Attack:
                if (!animator.IsPlaying(attackAnim.name))
                    Idle();
                break;
            case ControllerStatus.Parry:
                break;
            case ControllerStatus.Dodge:
                if (!animator.IsPlaying(dodgeAnim.name))
                {
                    transform.parent.position = transform.position;
                    transform.localPosition = Vector3.zero;
                    Idle();
                }
                break;
        }
    }

    public float HandleDamage(float amount)
    {
        if (cStatus == ControllerStatus.Dodge)
            return 0;
        else if (cStatus == ControllerStatus.Parry)
        {
            float st = staminaPerDamage * amount;
            if (st <= stamina)
            {
                stamina -= st;
                return 0;
            }
            else
            {
                st = stamina / staminaPerDamage;
                stamina = 0;
                return amount - st;
            }
        }
        return amount;
    }

    protected void Idle()
    {
        cStatus = ControllerStatus.Idle;
        timeStamp = Time.time;
    }

    protected bool CanAttack()
    {
        return cStatus == ControllerStatus.Idle && stamina - attackStaminaCost >= 0;
    }

    protected void Attack()
    {
        cStatus = ControllerStatus.Attack;
        stamina -= attackStaminaCost;
        animator.Play(attackAnim.name);

        //TODO: Do some Ray magic to damage enemy
    }

    protected bool CanParry()
    {
        return cStatus == ControllerStatus.Idle;
    }

    protected void Parry()
    {
        cStatus = ControllerStatus.Parry;
        animator[parryAnim.name].speed = 1;
        animator.Play(parryAnim.name);
    }

    protected void EndParry()
    {
        animator[parryAnim.name].speed = -1;
        animator.Play(parryAnim.name);
        Idle();
    }

    protected bool CanDodge()
    {
        return cStatus == ControllerStatus.Idle && stamina - dodgeStaminaCost >= 0;
    }

    protected void Dodge()
    {
        cStatus = ControllerStatus.Dodge;
        stamina -= dodgeStaminaCost;
        animator.Play(dodgeAnim.name);
    }

    protected GameObject FirstEnemyInAttackRange(int dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right * dir, hitZoneLength);
        return hit.collider.gameObject;
    }
}

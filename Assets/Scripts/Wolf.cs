using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : BasicHostileNPC {

    public float attackReloadTime = 3;
    public float attackDamage;
    float timeStamps;

    protected override void Attack()
    {
        if (timeStamps - Time.time <= 0)
        {
            animator.SetTrigger("Attack");
            timeStamps = Time.time + attackReloadTime;
            player.SendMessage("TakeDamage", new BasicDamageInfo(IDamageInfo.DamageTyp.Melee, attackDamage), SendMessageOptions.DontRequireReceiver);
        }
    }
}

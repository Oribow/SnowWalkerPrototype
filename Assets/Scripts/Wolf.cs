using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : BasicHostileNPC {

    public float attackReloadTime = 3;
    float timeStamps;

    protected override void Attack()
    {
        if (timeStamps - Time.time <= 0)
        {
            animator.SetTrigger("Attack");
            timeStamps = Time.time + attackReloadTime;
        }
    }
}

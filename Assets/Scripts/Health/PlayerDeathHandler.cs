using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour {

	public IHealth healthComponent;
    public Animator animator;
    public ReloadLevel reloadLevel;

    void Start()
    {
        healthComponent.OnDeath += HealthComponent_OnDeath;
    }

    private void HealthComponent_OnDeath(object sender, System.EventArgs e)
    {
        if (animator != null)
            animator.SetTrigger("Die");
        FocusHandler.PushFocus(reloadLevel);
    }
}

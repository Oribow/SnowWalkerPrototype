using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGUI : MonoBehaviour {

    public IHealth health;
    public UnityEngine.UI.Text text;

	// Use this for initialization
	void Start () {
        health.OnHealthChanged += Health_OnHealthChanged;

    }

    private void Health_OnHealthChanged(object sender, IDamageInfo e)
    {
        text.text = health.CurrentHealth + "+";
    }
}

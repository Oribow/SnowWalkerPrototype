using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFocus : IFocusable {

    public UnityEngine.Events.UnityEvent onGainFocus;
    public UnityEngine.Events.UnityEvent onLoseFocus;

    public override void OnGainFocus()
    {
        onGainFocus.Invoke();
    }

    public override void OnLoseFocus()
    {
        onLoseFocus.Invoke();
    }

    void Start () {
        FocusMe();
	}
}

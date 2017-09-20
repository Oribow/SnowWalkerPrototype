using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadLevel : IFocusable {

    public UnityEngine.Events.UnityEvent onGainFocus;
    public UnityEngine.Events.UnityEvent onLoseFocus;

    public void ReloadThisLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void OnGainFocus()
    {
        onGainFocus.Invoke();
    }

    public override void OnLoseFocus()
    {
        onLoseFocus.Invoke();
    }
}

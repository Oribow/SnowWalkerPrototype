using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusHandler {

    static Stack<IFocusable> focusStack;

    public static void PushFocus(IFocusable newFocus)
    {
        if (focusStack == null)
        {
            focusStack = new Stack<IFocusable>(2);
            focusStack.Push(newFocus);
            newFocus.OnGainFocus();
        }
        else if (newFocus == focusStack.Peek())
        {
            
        }
        else {
            focusStack.Peek().OnLoseFocus();
            focusStack.Push(newFocus);
            newFocus.OnGainFocus();
        }
        DebugPanel.Log("Focus", focusStack.Peek());
    }

    public static void PopFocus()
    {
        if (focusStack == null)
        {
            return;
        }
        focusStack.Pop().OnLoseFocus();
        focusStack.Peek().OnGainFocus();
    }
}

public abstract class IFocusable : MonoBehaviour
{
    public abstract void OnGainFocus();
    public abstract void OnLoseFocus();
    public void FocusMe()
    {
        FocusHandler.PushFocus(this);
    }
    public void DeFocusMe()
    {
        FocusHandler.PopFocus();
    }
}

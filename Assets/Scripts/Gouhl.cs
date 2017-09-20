using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gouhl : BasicHostileNPC
{
    public int difficulty;

    bool startedGame = false;

    void OnGameEnds(bool success)
    {
        if (success)
        {
            KillMe();
        }
        else
        {
            brainState = BrainState.Comatose;
            FocusHandler.PushFocus(FindObjectOfType<ReloadLevel>());
        }
    }

    protected override void Attack ()
    {
        RythmGame.rythmGame.StartGame(difficulty, OnGameEnds);
        brainState = BrainState.Comatose;
    }
}

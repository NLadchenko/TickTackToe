using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class MenuController : TTTElement
{
  
    public void SetDifficulty(Difficulty difficulty)
    {
        if (app.model.GetNext(Command.Begin) == ProcessState.Active)
        {
            app.model.MoveNext(Command.Begin);
            print("Difficulty=" + (int)difficulty);
            PlayerPrefs.SetInt("Difficulty", (int)difficulty);
            app.menuView.startButton.interactable = true;
            app.model.MoveNext(Command.End);
        }
    }

    public void StartGame()
    {

        if (app.model.GetNext(Command.Begin) == ProcessState.Active)
        {
            app.model.MoveNext(Command.Begin);
            Application.LoadLevel("Game");
            app.model.MoveNext(Command.End);
        }
    }

    public void ExitGame()
    {
        if (app.model.GetNext(Command.Exit) == ProcessState.Terminated)
        {
            app.model.MoveNext(Command.Exit);
            Application.Quit();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;


// Use this for initialization
public class TTTElement : MonoBehaviour
{
    // Gives access to the application and all instances.
    public TTTApplication app { get { return GameObject.FindObjectOfType<TTTApplication>(); } }

    public enum Difficulty
    {
        Easy = 1,
        Normal = 2,
        Undefeatable = 99
    }

    public enum ProcessState
    {
        Inactive,
        Active,
        Paused,
        Terminated
    }

    public enum Command
    {
        Begin,
        End,
        Pause,
        Resume,
        Exit
    }

    public enum Cell
    {
        Empty = 0,
        X = 1,
        O = 2
    }

    public enum RoundState
    {
        Win,
        Loss,
        Draw,
        Undefined
    }

}


public class TTTApplication : MonoBehaviour
{
    // Reference to the root instances of the MVC.
    public TTTModel model;
    public GameView gameView;
    public MenuView menuView;
    public GameController gameController;
    public MenuController menuController;


    // Init things here
    void Start() { Application.runInBackground = true; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButtonView : TTTElement
{
    public Difficulty difficulty = Difficulty.Easy;

    // Use this for initialization
    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        //print("view diff=" + (int)difficulty);
        app.menuController.SetDifficulty(difficulty);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : TTTElement
{

    public Button startButton;
    void Start()
    {
        //find elements in scene if not attached
        if (startButton == null)
            startButton = GameObject.Find("StartButton").GetComponent<Button>();
       startButton.interactable = false;
    }
}

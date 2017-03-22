using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellButtonView : TTTElement
{
    public int index = 0;
    // Use this for initialization
    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {

        app.gameController.MakeMove(index);
        //app.gameController.MakeMove(0);

    }
}

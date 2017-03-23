using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : TTTElement
{


    public GameObject[] gridButtons;
    public Text winCountText;
    public Text drawCountText;
    public Text lossCountText;
    public GameObject resultPanel;
    public GameObject winText;
    public GameObject drawText;
    public GameObject lossText;

    void Start()
    {


        //find elements in scene if not attached
        if (winText == null)
            winText = GameObject.Find("WinText");
        if (drawText == null)
            drawText = GameObject.Find("DrawText");
        if (lossText == null)
            lossText = GameObject.Find("LossText");

        winText.SetActive(false);
        drawText.SetActive(false);
        lossText.SetActive(false);

        //find elements in scene if not attached
        if (resultPanel == null)
            resultPanel = GameObject.Find("ResultPanel");
        resultPanel.SetActive(false);

        //find elements in scene if not attached
        if (winCountText == null)
            winCountText = GameObject.Find("WinsBody").GetComponent<Text>();
        if (drawCountText == null)
            drawCountText = GameObject.Find("DrawsBody").GetComponent<Text>();
        if (lossCountText == null)
            lossCountText = GameObject.Find("LossesBody").GetComponent<Text>();

        //find elements in scene if not attached
        if (gridButtons.Length != 9
            || gridButtons[0] == null
            || gridButtons[1] == null
            || gridButtons[2] == null
            || gridButtons[3] == null
            || gridButtons[4] == null
            || gridButtons[5] == null
            || gridButtons[6] == null
            || gridButtons[7] == null
            || gridButtons[8] == null)
        {
            gridButtons = new GameObject[9];
            for (int i = 0; i < 9; i++)
            {
                gridButtons[i] = GameObject.Find("CellButton_" + i.ToString());
            }
        }

    }
}

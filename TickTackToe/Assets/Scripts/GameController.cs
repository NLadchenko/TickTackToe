using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GameController : TTTElement
{
    //Initialization
    void Start()
    {
        //check if difficulty is set
        int difficulty = PlayerPrefs.GetInt("Difficulty");

        if (difficulty != 1 && difficulty != 2 && difficulty != 99)
        {
            PlayerPrefs.SetInt("Difficulty", (int)Difficulty.Easy);
            maxDepth = 1;
        }
        else
            maxDepth = difficulty;

        app.model.roundState = RoundState.Undefined;               

        currentTurn = Cell.X;
        player = Cell.X;
        computer = Cell.O;
        grid = new Cell[9];
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = Cell.Empty;
        }
    }

    void Update()//ai moves when it's turn comes
    {
        if(currentTurn == computer && app.model.roundState == RoundState.Undefined)
        {
            print("AI moves");
            MakeMove(0);
        }
    }
    
    public void ExitMenu()//on ExitMenu button press
    {
        if (app.model.GetNext(Command.Exit) == ProcessState.Terminated)
        {

            app.model.MoveNext(Command.Exit);
            Application.LoadLevel("Menu");
        }
    }

    public void Continue()//on continue button press
    {
        if (app.model.GetNext(Command.Begin) == ProcessState.Active)
        {
            app.model.MoveNext(Command.Begin);
            
            EnableResultPanel(false);//hide result panel
            Increment(app.model.roundState);//update counters
            RefreshGrid(app.model.roundState);//refresh grid

            //change players if draw or loss accordingly
            if (app.model.roundState == RoundState.Draw)
                SetPlayer(SwitchCell(player));
            else if (app.model.roundState == RoundState.Loss)
                SetPlayer(Cell.O);
            else
                SetPlayer(Cell.X);
            currentTurn = Cell.X;//X always starts first
            app.model.roundState = RoundState.Undefined;

            app.model.MoveNext(Command.End);
        }
        else
            print("unable to Continue()");
    }

    private void EnableResultPanel(bool enable)//hides/shows pop-up result panel
    {        
        if (enable)
            app.gameView.resultPanel.SetActive(true);
        else
            app.gameView.resultPanel.SetActive(false);
    }

    private void Increment(RoundState result)//increment win-draw-loss counters by acessing and converting their stored values
    {
        if (result == RoundState.Win)
        {
            int i = Convert.ToInt32(app.gameView.winCountText.text) + 1;
            app.gameView.winCountText.text = i.ToString();
        }
        else if (result == RoundState.Draw)
        {
            int i = Convert.ToInt32(app.gameView.drawCountText.text) + 1;
            app.gameView.drawCountText.text = i.ToString();
        }
        else
        {
            int i = Convert.ToInt32(app.gameView.lossCountText.text) + 1;
            app.gameView.lossCountText.text = i.ToString();
        }
    }

    private void Round(RoundState result)//game has ended - time to show score and continue to nex round
    {
        //RoundState.Undefined
        app.model.roundState = result;
        EnableResultPanel(true);
        if (result == RoundState.Win)
        {
            app.gameView.winText.SetActive(true);
            app.gameView.drawText.SetActive(false);
            app.gameView.lossText.SetActive(false);
        }
        else if (result == RoundState.Draw)
        {
            app.gameView.winText.SetActive(false);
            app.gameView.drawText.SetActive(true);
            app.gameView.lossText.SetActive(false);
        }
        else
        {
            app.gameView.winText.SetActive(false);
            app.gameView.drawText.SetActive(false);
            app.gameView.lossText.SetActive(true);
        }
    }

    private void RefreshGrid(RoundState result)//in order to countinue RoundPAnel must be hidden and, grid restored
    {
        //restore grid
        foreach (GameObject gridButton in app.gameView.gridButtons)
        {
            gridButton.GetComponentInChildren<Text>().text = "";
            gridButton.GetComponent<Button>().interactable = true;
        }
        grid = new Cell[9];

        
        
    }

    //------------------------------- minimax -------------------------------

    private int[,] winConditions = new int[8, 3]
    {
        { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 },
        { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 },
        { 0, 4, 8 }, { 2, 4, 6 }
    };

    public Cell[] grid = new Cell[9];
    public Cell currentTurn = Cell.X;
    public Cell computer = Cell.O;
    public Cell player = Cell.X;
    public int choice = 0;
    public int maxDepth = 1;

    private void SetPlayer(Cell player)
    {
        this.player = player;
        this.computer = SwitchCell(player);
    }

    private Cell SwitchCell(Cell cell)
    {
        if (cell == Cell.X) return Cell.O;
        else return Cell.X;
    }

    public void MakeMove(int move)
    {
        if (app.model.GetNext(Command.Begin) == ProcessState.Active)
        {
            app.model.MoveNext(Command.Begin);
            if (app.model.roundState == RoundState.Undefined)
            {
                if (currentTurn == player)//if player's move - grid simply get;s updated by makeGridMove, if ai - movement is chosen by minimax
                {
                    print("ai " + computer + " | player " + player + " | " + currentTurn + " moves to " + move);
                    grid = makeGridMove(grid, currentTurn, move);
                    //update grid button
                    app.gameView.gridButtons[move].GetComponentInChildren<Text>().text = player.ToString();
                    app.gameView.gridButtons[move].GetComponent<Button>().interactable = false;
                    currentTurn = SwitchCell(currentTurn);
                }
                else if (currentTurn == computer)
                {
                    minimax(cloneGrid(grid), currentTurn, 0);
                    print("ai " + computer + " | player " + player + " | " + currentTurn + " moves to " + choice);
                    grid = makeGridMove(grid, currentTurn, choice);
                    //update grid button
                    app.gameView.gridButtons[choice].GetComponentInChildren<Text>().text = computer.ToString();
                    app.gameView.gridButtons[choice].GetComponent<Button>().interactable = false;
                    currentTurn = SwitchCell(currentTurn);
                }
                checkRound(grid, player);
            }
            app.model.MoveNext(Command.End);
        }
        else
            print("Unable to MakeMove");
    }

    private Cell[] makeGridMove(Cell[] grid, Cell move, int position)
    {
      
        Cell[] newGrid = cloneGrid(grid);
        if (newGrid[position] == move)
            print("Moving to filled cell! move=" + move + " position=" + position + " cell=" + newGrid[position]);
        newGrid[position] = move;
        return newGrid;
    }

    private Cell[] cloneGrid(Cell[] grid)
    {
        Cell[] clone = new Cell[9];
        for (int i = 0; i < 9; i++)
        {
            clone[i] = grid[i];
        }

        return clone;
    }

    private int minimax(Cell[] inputGrid, Cell player, int depth)//minimax
    {
        Cell[] grid = cloneGrid(inputGrid);

        if (checkScore(grid, computer, depth) != 0)
            return checkScore(grid, computer, depth);
        else if (checkGameEnd(grid)) return 0;

        depth += 1;

        if (depth <= maxDepth)//difficulty limits recursion depth
        {
            List<int> scores = new List<int>();
            List<int> moves = new List<int>();

            for (int i = 0; i < 9; i++)
            {
                if (grid[i] == Cell.Empty)
                {
                    scores.Add(minimax(makeGridMove(grid, player, i), SwitchCell(player), depth));
                    moves.Add(i);
                }
            }

            if (player == computer)
            {
                int maxScoreIndex = scores.IndexOf(scores.Max());
                if (scores.Max() == scores.Min())//randomize choices if they are all the same
                    maxScoreIndex = UnityEngine.Random.Range(0, scores.Count - 1);
                choice = moves[maxScoreIndex];
                return scores[maxScoreIndex];
            }
            else
            {
                int minScoreIndex = scores.IndexOf(scores.Min());
                if (scores.Max() == scores.Min())//randomize choices if they are all the same
                    minScoreIndex = UnityEngine.Random.Range(0, scores.Count - 1);
                choice = moves[minScoreIndex];
                return scores[minScoreIndex];
            }            
        }
        else
            return 0;

    }

    private int checkScore(Cell[] grid, Cell player, int depth)//return score for minimax
    {
        if (checkGameWin(grid, player)) return 10 - depth;

        else if (checkGameWin(grid, SwitchCell(player))) return depth - 10;

        else return 0;
    }

    private bool checkGameWin(Cell[] grid, Cell player)//check game win condition for minimax
    {
        for (int i = 0; i < 8; i++)
        {
            if (grid[winConditions[i, 0]] == player && grid[winConditions[i, 1]] == player && grid[winConditions[i, 2]] == player)
            {
                return true;
            }
        }
        return false;
    }

    private void checkRound(Cell[] grid, Cell player)//check round end after each move
    {
        bool fin = false;

        for (int i = 0; i < 8; i++)
        {
            if (grid[winConditions[i, 0]] == player && grid[winConditions[i, 1]] == player && grid[winConditions[i, 2]] == player)
            {
                fin = true;
                print(player + " won!");
                Round(RoundState.Win);
            }
        }
        if (!fin)//if player has not won - check ai
        {
            for (int i = 0; i < 8; i++)
            {
                if (grid[winConditions[i, 0]] == computer && grid[winConditions[i, 1]] == computer && grid[winConditions[i, 2]] == computer)
                {
                    fin = true;
                    print(player + " lost!");
                    Round(RoundState.Loss);
                }
            }
            if (!fin)//if noone won and if one cell left - check if player can win, if no - draw
            {
                List<int> emptyCells = new List<int>();

                for (int j = 0; j < 9; j++)
                    if (grid[j] == Cell.Empty)
                        emptyCells.Add(j);

                if (emptyCells.Count == 1)
                {
                    grid[emptyCells[0]] = player;
                    for (int i = 0; i < 8; i++)
                    {
                        if (grid[winConditions[i, 0]] == player && grid[winConditions[i, 1]] == player && grid[winConditions[i, 2]] == player)
                        {
                            fin = true;
                        }
                    }
                    if (!fin)
                    {
                        print("Draw!");
                        Round(RoundState.Draw);
                    }
                }
            }
        }
    }    

    private static bool checkGameEnd(Cell[] grid)//check game end condition for minimax
    {
        foreach (Cell p in grid) if (p == Cell.Empty) return false;
        return true;
    }
}

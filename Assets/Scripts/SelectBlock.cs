using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBlock : MonoBehaviour
{
    public List<Transform> horizontals;
    public Transform[,] gameSquares;
    public List<Transform> matches = new List<Transform>();
    FloodFill flood;

    int rows, cols;
    public void Initiate()
    {
        flood = GetComponent<FloodFill>();

        rows = horizontals.Count;
        cols = horizontals[0].childCount;
        flood.rows = rows;
        flood.cols = cols;
        gameSquares = new Transform[rows,cols];
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                gameSquares[i, j] = horizontals[i].GetChild(j);
                SetGameSquare(i, j);
            }
        }
        flood.gameSquares = gameSquares;
        flood.IsLeft();
    }

    public Items[] items;
    public void SetGameSquare(int i, int j)
    {
        Blocks blocks = gameSquares[i, j].GetChild(0).GetComponent<Blocks>();
        Items item = items[Random.Range(0, items.Length)];
        blocks.SetItem(item);
        blocks.SetPosition(i, j);
    }

    public void SelectItem(int i, int j, string name)
    {
        flood.PrepareFill(i,j,name,gameSquares);
    }
}

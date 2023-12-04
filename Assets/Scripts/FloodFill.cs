using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

public class FloodFill : MonoBehaviour
{
    int start_i, start_j;
    public int rows, cols;

    public Transform[,] gameSquares;
    List<Transform> matches;


    //Creation
    [SerializeField] GameObject item;
    public SelectBlock sBlock;

    GameManager gm;

    public TextMeshProUGUI movesTxt;
    int moves;
    
    public ParticleSystem[] exp;

    AudioSource expSound;
    private void Start()
    {
        gm = GetComponent<GameManager>();
        expSound = GetComponent<AudioSource>();
        moves = 0;
    }
    public void PrepareFill(int i, int j, string name, Transform[,] newSquares)
    {
        matches = new List<Transform>();
        start_i = i;
        start_j = j;
        gameSquares = newSquares;
        Fill(start_i, start_j,"Click", name);
    }

    void Fill(int i, int j, string op, string name)
    {
        //Conditions of return
        if (gameSquares[i, j].transform.parent.name == "TOP") return;
        if (gameSquares[i, j].childCount == 0) return;
        if (gameSquares[i, j].GetChild(0).name != name) return;
        if (op == "Click" && matches.Contains(gameSquares[i, j].GetChild(0))) return;
        if (op == "Click" && gameSquares[i, j].GetChild(0).GetComponent<Blocks>().cantClick) return;
        if (op != "Click" && isLeft.Contains(gameSquares[i, j].GetChild(0))) return;

        if (op == "Click")
            matches.Add(gameSquares[i, j].GetChild(0));
        else
            isLeft.Add(gameSquares[i, j].GetChild(0));

        CheckDown(i, j, op, name);
        CheckLeft(i, j, op, name);
        CheckUp(i, j, op, name);
        CheckRight(i, j, op, name);

        if (op == "Check") return;

        if (i == start_i && j == start_j && matches.Count > 2 && op == "Click")
        {
            gm.canClick = false;

            if (PlayerPrefs.GetInt("Sound") == 1)
                expSound.Play();

            gm.ResetTime();
            moves++;
            movesTxt.text = moves.ToString();
            DestroyMatches();
        }
    }


    void CheckDown(int i, int j, string op, string name)
    {
        if (i == 0) return;
        Fill(i - 1, j, op, name);
    }
    void CheckUp(int i, int j, string op, string name)
    {
        if (i == rows - 1) return;
        Fill(i + 1, j, op, name);
    }
    void CheckLeft(int i, int j, string op, string name)
    {
        if (j == 0) return;
        Fill(i, j - 1, op, name);
    }
    void CheckRight(int i, int j, string op, string name)
    {
        if (j == cols - 1) return;
        Fill(i, j + 1, op, name);
    }

    void DestroyMatches()
    {
        int counter = 0;
        for (int i = 0; i < matches.Count; i++)
        {
            exp[counter].transform.position = matches[i].transform.position;
            exp[counter].Play();
            counter++;
            Destroy(matches[i].gameObject);
        }
        StartCoroutine(CheckEmpties());
    }
    IEnumerator CheckEmpties()
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (gameSquares[i, j].childCount == 0)
                {
                    GetDown(i, j);
                    if (gameSquares[i, j].childCount > 0)
                    {
                        StartCoroutine(gameSquares[i, j].GetChild(0).GetComponent<Blocks>().DropDown());
                    }
                }
            }
        }
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (gameSquares[i, j].childCount == 0)
                {
                    GameObject newItem = Instantiate(item, gameSquares[i, j]);
                    newItem.transform.localPosition = Vector3.zero;
                    sBlock.SetGameSquare(i, j);
                }
            }

        }

        IsLeft();

    }
    void GetDown(int i, int j)
    {
        if (i == rows - 1) return;

        if (gameSquares[i + 1, j].childCount > 0)
        {
            gameSquares[i + 1, j].GetChild(0).parent = gameSquares[i, j];
            Blocks blck = gameSquares[i, j].GetChild(0).GetComponent<Blocks>();
            blck.i = i;
            blck.j = j;
        }
        else
        {
            GetDown(i + 1, j);
            if (gameSquares[i, j].childCount == 0 && gameSquares[i + 1, j].childCount > 0)
            {
                gameSquares[i + 1, j].GetChild(0).parent = gameSquares[i, j];
                Blocks blck = gameSquares[i, j].GetChild(0).GetComponent<Blocks>();
                blck.i = i;
                blck.j = j;
            }
        }
    }
    List<Transform> isLeft = new List<Transform>();
    public Animator noneLeft;

    public void IsLeft()
    {
        bool left = false;
        for (int i = 0; i < (rows / 2) + 1; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Fill(i, j, "Check", gameSquares[i, j].GetChild(0).name);
                if (isLeft.Count > 2)
                {
                    left = true;
                    break;
                }
                isLeft = new List<Transform>();
            }
            if (left) break;
        }
        if (left)
        {
            Debug.Log("THERE IS ONE");
        }
        else
        {
            noneLeft.SetTrigger("Anim");
            sBlock.Initiate();
        }

        isLeft = new List<Transform>();
    }

}

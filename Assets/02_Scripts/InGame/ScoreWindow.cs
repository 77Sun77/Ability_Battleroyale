using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreWindow : MonoBehaviour
{
    public static ScoreWindow instance;
    public List<ScoreBoard> boards = new List<ScoreBoard>();
    public List<int> scoreTemp = new List<int>();
    public Transform canvas;
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        List<int> temp = new List<int>();
        foreach(ScoreBoard board in boards)
        {
            temp.Add(board.score);
        }

        for(int i=0; i<boards.Count; i++)
        {
            if(temp[i] != scoreTemp[i])
            {
                foreach (ScoreBoard board in boards)
                {
                    board.transform.parent = canvas;
                }
                Sort();

            }
        }
    }

    void Sort()
    {
        
        for(int i = 0; i < boards.Count; i++)
        {
            int maxTemp = 0;
            int index = i;
            for(int j=i+1; j<boards.Count; j++)
            {
                if(maxTemp < boards[j].score)
                {
                    maxTemp = boards[j].score;
                    index = j;
                }
            }
            ScoreBoard temp = boards[i];
            boards[i] = boards[index];
            boards[index] = temp;
            scoreTemp[i] = boards[i].score;
        }

        foreach(ScoreBoard board in boards)
        {
            board.transform.parent = transform;
        }
    }
}

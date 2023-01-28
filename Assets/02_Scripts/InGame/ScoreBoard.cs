using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreBoard : MonoBehaviour
{
    public GameObject[] players;
    public int score;

    public Text NameText, ScoreText;

    private void Start()
    {
        ScoreWindow.instance.boards.Add(this);
        ScoreWindow.instance.scoreTemp.Add(score);
    }

    public void Setting(PlayerController.Weapons weapons)
    {
        if (weapons == PlayerController.Weapons.Sword) players[0].SetActive(true);
        else if(weapons == PlayerController.Weapons.Magic) players[1].SetActive(true);
        else if (weapons == PlayerController.Weapons.Army) players[2].SetActive(true);
        else if (weapons == PlayerController.Weapons.Ninja) players[3].SetActive(true);
        else players[4].SetActive(true);
    }

    public void UpdateInfo(int score, string name)
    {
        this.score = score;
        ScoreText.text = score.ToString();
        NameText.text = name;
    }
}

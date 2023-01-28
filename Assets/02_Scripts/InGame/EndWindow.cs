using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class EndWindow : MonoBehaviour
{
    public PlayerController.Weapons winnerWeapon;

    public GameObject[] players;
    public Text nameText;

    public Transform pieces;
    public void Setting(PlayerController.Weapons weapon, string name)
    {
        if (weapon == PlayerController.Weapons.Sword) players[0].SetActive(true);
        else if (weapon == PlayerController.Weapons.Magic) players[1].SetActive(true);
        else if (weapon == PlayerController.Weapons.Army) players[2].SetActive(true);
        else if (weapon == PlayerController.Weapons.Ninja) players[3].SetActive(true);
        else players[4].SetActive(true);
        nameText.text = name;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        pieces.Rotate(Vector3.back * 180 * Time.deltaTime);
    }

    public void Quit()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }
}

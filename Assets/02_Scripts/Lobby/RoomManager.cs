using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    public GameObject Lobby, CharSelect;
    public LobbyPlayer player;
    public PlayerList list;


    public Color ReadyColor, CancelColor, MasterColor;

    Vector2 spawnPoint;

    public GameObject MapParent;
    public GameObject[] maps;
    void OnEnable()
    {
        instance = this;
        spawnPoint = new Vector2(0, 2.6f);
    }
    public void LobbyCharSpawn(PlayerController.Weapons weapon)
    {
        if(weapon == PlayerController.Weapons.Sword)
        {
            player = PhotonNetwork.Instantiate("Lobby/SwordChar", spawnPoint, Quaternion.identity).GetComponent<LobbyPlayer>();
        }
        else if(weapon == PlayerController.Weapons.Magic)
        {
            player = PhotonNetwork.Instantiate("Lobby/MagicChar", spawnPoint, Quaternion.identity).GetComponent<LobbyPlayer>();
        }
        else if(weapon == PlayerController.Weapons.Army)
        {
            player = PhotonNetwork.Instantiate("Lobby/ArmyChar", spawnPoint, Quaternion.identity).GetComponent<LobbyPlayer>();
        }
        else if(weapon == PlayerController.Weapons.Ninja)
        {
            player = PhotonNetwork.Instantiate("Lobby/NinjaChar", spawnPoint, Quaternion.identity).GetComponent<LobbyPlayer>();
        }
        else
        {
            player = PhotonNetwork.Instantiate("Lobby/FighterChar", spawnPoint, Quaternion.identity).GetComponent<LobbyPlayer>();
        }
        player.Weapon = weapon;
    }
    public void LobbyCharDestroy()
    {
        if(player != null)
        {
            spawnPoint = player.transform.position;
            PhotonNetwork.Destroy(player.photonView);
        }

    }
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startBtn.SetActive(true);
            readyBtn.SetActive(false);
            cancelBtn.SetActive(false);
        }
        else
        {
            startBtn.SetActive(false);
        }
    }

    public void DisableLobby()
    {
        Lobby.SetActive(false);
        LobbyManager.instance.Disable();
    }

    public GameObject readyBtn, cancelBtn, startBtn;
    public GameObject warningPrefab;
    public Transform warningParent;
    public void OnClick_Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        bool isAllReady = false;
        foreach(GameObject player in players)
        {
            if (player.GetComponent<LobbyPlayer>().isReady)
            {
                isAllReady = true;
            }
            else
            {
                isAllReady = false;
                break;
            }
        }
        if (isAllReady)
        {
            int random = Random.Range(0, 3); // 0==Grassland

            player.photonView.RPC("SceneMove", RpcTarget.AllBuffered, random);
        }
        else
        {
            Instantiate(warningPrefab, warningParent);
        }
    }
    

    public void OnClick_Ready()
    {
        if (player.photonView.IsMine)
        {
            player.isReady = true;
            readyBtn.SetActive(false);
            cancelBtn.SetActive(true);
        }
    }
    public void OnClick_Cancel()
    {
        if (player.photonView.IsMine)
        {
            player.isReady = false;
            readyBtn.SetActive(true);
            cancelBtn.SetActive(false);
        }
    }
    public void DisableAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
    public GameObject QuitWindow;
    public void OnClick_Quit()
    {
        QuitWindow.SetActive(true);
    }
    public void DisableQuitWindow()
    {
        LobbyManager.instance.isMaster = false;
        StartCoroutine(LobbyManager.instance.Disable_UI(QuitWindow));
    }
    public void QuitRoom()
    {
        PhotonNetwork.LeaveRoom();
        instance = null;
        LobbyManager.instance.isStart = true;
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetTrigger("Quit");
        
    }

    public void EnableLobby()
    {
        Lobby.SetActive(true);
    }

    public void DisableGO()
    {
        gameObject.SetActive(false);
        readyBtn.SetActive(true);
        cancelBtn.SetActive(false);
        GetComponent<Animator>().enabled = false;
    }

    public GameObject warningText;
    public void OnClick_CharSelect()
    {
        if(player.isReady && !PhotonNetwork.IsMasterClient)
        {
            Instantiate(warningText, warningParent);
            return;
        }
        CharSelect.SetActive(true);
    }

    public void OnMap(int mapNum)
    {
        MapParent.SetActive(true);
        maps[mapNum].SetActive(true);
        Invoke("LoadScene", 3);
    }

    void LoadScene()
    {
        SceneManager.LoadScene("InGame");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    public static int index;
    public static PlayerController.Weapons weapon;

    public Transform[] spawnPoint;
    List<Transform> spawnList = new List<Transform>();
    public static int MapNum;
    public GameObject MapParent;
    public GameObject[] maps;
    public GameObject[] mapObjects;
    public Collider2D[] Ranges;
    public GameObject[] BGs;

    public GameObject dieWindow;

    public Text countText;

    public bool isAllReady, isStart, isDisable, isEnd;

    public PlayerController player;
    public List<PlayerController> Players = new List<PlayerController>();
    public GameObject ScoreBoardPrefab;
    public Transform ScoreBoardParent;

    public CinemachineConfiner2D camera;

    public EndWindow endWindow;
    public static void SettingManager(int index, PlayerController.Weapons weapon, int MapNum)
    {
        GameManager.index = index;
        GameManager.weapon = weapon;
        GameManager.MapNum = MapNum;
    }
    void Start()
    {
        instance = this;
        MapParent.SetActive(true);
        maps[MapNum].SetActive(true);
        mapObjects[MapNum].SetActive(true);
        camera.m_BoundingShape2D = Ranges[MapNum];
        BGs[MapNum].SetActive(true);
        foreach (Transform child in spawnPoint[MapNum])
        {
            spawnList.Add(child);
        }
        PhotonNetwork.Instantiate("InGame/"+weapon.ToString()+"/"+ weapon.ToString() + "Char", spawnList[index].position, Quaternion.identity);
    }

    void Update()
    {
        if(!isAllReady && PhotonNetwork.IsMasterClient)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                if (!player.GetComponent<PlayerController>().isReady) return;
            }
            isAllReady = true;
            
        }
        if (isStart)
        {
            Invoke("DisableText", 1);
        }

        
    }


    public void DisableMapWindow()
    {
        MapParent.SetActive(false);
        maps[MapNum].SetActive(false);
        countText.gameObject.SetActive(true);
        isDisable = true;
    }

    void DisableText()
    {
        countText.gameObject.SetActive(false);
    }

    public void Respawn()
    {
        player.transform.position = spawnList[Random.Range(0,spawnList.Count)].position;
        player.gameObject.SetActive(true);
        player.isDisable = false;
        dieWindow.SetActive(false);
    }

    public ScoreBoard SpawnScoreBoard(PlayerController.Weapons weapon)
    {
        ScoreBoard scoreBoard = Instantiate(ScoreBoardPrefab, ScoreBoardParent).GetComponent<ScoreBoard>();
        scoreBoard.Setting(weapon);
        return scoreBoard;
    }

    public void End(PlayerController.Weapons weapon, string name)
    {
        endWindow.Setting(weapon, name);
        isEnd = true;
    }
}

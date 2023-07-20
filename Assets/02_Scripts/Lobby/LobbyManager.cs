using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager instance;

    public Transform MainUI, WindowsUI;
    public GameObject LoadingWindow, NameSetWindow;
    public Text NameText;

    public InputField c_RoomName; // Create
    public GameObject c_Warning;
    public GameObject c_Window;
    public InputField j_RoomName; // Join
    public GameObject j_Warning;
    public GameObject j_Window;


    string name;

    public GameObject Room;

    public bool isConnect, isStart, isMaster;

    public static bool isFirst;

    public AudioClip startMusic, lobbyMusic;


    void Awake()
    {
        
        instance = this;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }
    public override void OnConnectedToMaster()
    {
        LoadingWindow.SetActive(false);
        SoundManager.instance.AS.clip=startMusic;
        SoundManager.instance.AS.Play();
        if (!isConnect && !isFirst)
        {
            isFirst = true;
            NameSetWindow.SetActive(true);
        }
        else
        {
            Set_Name(PhotonNetwork.LocalPlayer.NickName);
        }
        isConnect = true;
        foreach (Transform tr in MainUI)
        {
            tr.gameObject.SetActive(true);
        }
    }

    public void Disable()
    {
        foreach(Transform tr in WindowsUI)
        {
            tr.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        
    }

    public void Set_Name(string name)
    {
        this.name = name;
        NameText.text = name;
        PhotonNetwork.LocalPlayer.NickName = name;
    }

    public IEnumerator Disable_UI(GameObject go)
    {
        go.GetComponent<Animator>().SetTrigger("UI_Out");
        yield return new WaitForSeconds(0.6f);
        go.SetActive(false);
    }


    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(c_RoomName.text, new RoomOptions { MaxPlayers = 8 }, null);
    }
    public override void OnCreatedRoom()
    {
        c_RoomName.text = "";
        Room.SetActive(true);
        isMaster = true;
    }
    public override void OnCreateRoomFailed(short _returnCode, string _message)
    {
        c_Warning.SetActive(true);
        c_RoomName.text = "";
    }
    public void DisableCreateWindow()
    {
        c_Warning.SetActive(false);
        StartCoroutine(Disable_UI(c_Window));
        c_RoomName.text = "";
    }


    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(j_RoomName.text, null);
    }
    public override void OnJoinedRoom()
    {
        if (isMaster)
        {
            join();
        }
        else
        {
            LoadingWindow.SetActive(true);
            Invoke("join", 1);
        }
        SoundManager.instance.AS.clip = lobbyMusic;
        SoundManager.instance.AS.Play();
    }

    void join()
    {
        LoadingWindow.SetActive(false);
        j_RoomName.text = "";
        if (RoomManager.instance == null && isStart)
        {

            isStart = false;
            j_Warning.SetActive(true);
            PhotonNetwork.LeaveRoom();
            return;
        }
        
        Room.SetActive(true);
        Room.GetComponent<Animator>().enabled = true;
        Room.GetComponent<Animator>().SetTrigger("Join");
    }
    public override void OnJoinRoomFailed(short _returnCode, string _message)
    {
        j_Warning.SetActive(true);
        j_RoomName.text = "";
    }
    public void DisableJoinWindow()
    {
        j_Warning.SetActive(false);
        StartCoroutine(Disable_UI(j_Window));
        j_RoomName.text = "";
    }



}

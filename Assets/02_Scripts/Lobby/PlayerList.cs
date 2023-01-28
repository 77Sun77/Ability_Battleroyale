using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerList : MonoBehaviourPunCallbacks
{
    int playerCount;
    public GameObject namePrefab;
    public List<Text> texts = new List<Text>();

    void Update()
    {
        if(PhotonNetwork.PlayerList.Length != playerCount)
        {
            foreach (Transform child in transform) Destroy(child.gameObject);
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                Text t = Instantiate(namePrefab, transform).GetComponent<Text>();
                t.text = p.NickName;
            }
            playerCount = PhotonNetwork.PlayerList.Length;
        }
        if(playerCount != texts.Count)
        {
            texts.Clear();
            foreach (Transform child in transform)
            {
                texts.Add(child.GetComponent<Text>());
            }
        }
        if(texts.Count != 0)
        {
            RoomManager.instance.list.texts[0].color = RoomManager.instance.MasterColor;
        }
        
    }
}

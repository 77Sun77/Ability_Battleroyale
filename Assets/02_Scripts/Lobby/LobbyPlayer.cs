using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LobbyPlayer : PlayerController, IPunObservable
{
    public bool isMasterClient;
    public int nameCount;

    
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        myAnim = mySprite.GetComponent<Animator>();
        isMove = true;
        if (Weapon == Weapons.Sword) Set_Player(14, 5, 16);
        else if (Weapon == Weapons.Magic) Set_Player(12, 5, 16);
        else if (Weapon == Weapons.Army) Set_Player(16, 5, 16);
        else if (Weapon == Weapons.Ninja) Set_Player(4, 7, 14);
        else Set_Player(20, 4, 16);
    }

    void Update()
    {
        Move();
        Jump();
        NameText.text = photonView.IsMine ? PhotonNetwork.NickName : photonView.Owner.NickName;
        if (photonView.IsMine)
        {
            nameCount = IndexOut();
            
            name = PhotonNetwork.LocalPlayer.NickName;

            if (Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.5f), 0.07f, (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Scaffolding"))))
            {
                isGround = true;
                isDoubleJump = false;
                isJump = false;

            }
            else
            {
                isGround = false;
                isJump = true;
            }
        }

        if (PhotonNetwork.IsMasterClient && photonView.IsMine)
        {
            isMasterClient = true;
            isReady = true;
        }

        
        if (nameCount == 0) return;
        if (isReady)
        {
            RoomManager.instance.list.texts[nameCount].color = RoomManager.instance.ReadyColor;
        }
        else
        {
            RoomManager.instance.list.texts[nameCount].color = RoomManager.instance.CancelColor;
        }
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isReady);
            stream.SendNext(isMasterClient);
            stream.SendNext(nameCount);
            stream.SendNext(mySprite.flipX);
        }
        else
        {
            this.isReady = (bool)stream.ReceiveNext();
            this.isMasterClient = (bool)stream.ReceiveNext();
            this.nameCount = (int)stream.ReceiveNext();
            this.flip_X = (bool)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void SceneMove(int sceneNum)
    {
        LobbyManager.instance.isStart = true;
        if (RoomManager.instance == null) return;
        GameManager.SettingManager(RoomManager.instance.player.nameCount, RoomManager.instance.player.Weapon, sceneNum);
        RoomManager.instance.OnMap(sceneNum);
        
    }

    int IndexOut()
    {
        for(int i=0; i< PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber) return i;
        }
        return 0;
    }
}

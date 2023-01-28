using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class Army : MonoBehaviourPunCallbacks
{
    PlayerController pc;
    public Animator arm;
    private void Start()
    {
        pc = GetComponent<PlayerController>();
    }
    void Update()
    {
        if (GameManager.instance.isEnd) return;
        if (photonView.IsMine && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(0)) && GameManager.instance.isStart)
        {
            Shot();
        }
    }

    void Shot() 
    {
        Vector3 direction = Vector2.zero;
        if (pc.mySprite.flipX) direction = Vector2.left;
        else direction = Vector2.right;
        PhotonNetwork.Instantiate("InGame/Army/Bullet", transform.position + direction + (Vector3.up * 0.1f), Quaternion.identity).GetComponent<Bullet>().Set_Bullet(direction, photonView);
        arm.SetTrigger("Attack"); 
    }
}

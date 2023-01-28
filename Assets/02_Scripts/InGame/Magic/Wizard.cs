using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Wizard : MonoBehaviourPunCallbacks
{
    PlayerController pc;
    float timer;

    public Animator arm;
    private void Start()
    {
        pc = GetComponent<PlayerController>();
        timer = 0;
    }
    void Update()
    {
        if (GameManager.instance.isEnd) return;
        if ((timer<= 0 && photonView.IsMine) && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(0)) && GameManager.instance.isStart)
        {
            Shot();
        }
        timer -= Time.deltaTime;
    }

    void Shot()
    {
        Vector3 direction = Vector2.zero;
        if (pc.mySprite.flipX) direction = Vector2.left;
        else direction = Vector2.right;
        PhotonNetwork.Instantiate("InGame/Magic/Magic", transform.position + (direction *1.5f), Quaternion.identity).GetComponent<Magic>().Set_Magic(direction, photonView);
        timer = 0.5f;
        arm.SetTrigger("Attack");
    }
}

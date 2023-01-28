using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Sword : MonoBehaviourPunCallbacks
{
    PlayerController pc;
    float timer;

    public Animator arm;
    public GameObject sprite, spriteArm, Arm;

    public bool isAttack;
    private void Start()
    {
        pc = GetComponent<PlayerController>();
        timer = 0;
    }
    void Update()
    {
        if (GameManager.instance.isEnd) return;
        if (photonView.IsMine && !isAttack && GameManager.instance.isStart)
        {
            Vector3 vec = Arm.transform.rotation.eulerAngles;
            spriteArm.transform.rotation = Quaternion.Euler(vec);
        }
        if ((timer <= 0 && photonView.IsMine) && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(0)))
        {
            Swing();
        }
        timer -= Time.deltaTime;
    }

    void Swing()
    {
        arm.SetTrigger("Attack");
        photonView.RPC("Active", RpcTarget.AllBuffered);
        timer = 0.5f;
        isAttack = true;
    }

    [PunRPC]
    public void Active()
    {
        sprite.SetActive(true);
    }
}

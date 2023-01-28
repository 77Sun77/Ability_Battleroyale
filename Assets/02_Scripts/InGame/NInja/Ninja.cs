using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Ninja : MonoBehaviourPunCallbacks
{
    PlayerController pc;
    float timer;
    int attackCount;

    public Animator arm;
    private void Start()
    {
        pc = GetComponent<PlayerController>();
        timer = 0;
        attackCount = 0;
    }
    void Update()
    {
        if (GameManager.instance.isEnd) return;
        if ((timer <= 0 && photonView.IsMine) && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(0)) && GameManager.instance.isStart)
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
        if(attackCount % 2 == 0)
        {
            PhotonNetwork.Instantiate("InGame/Ninja/Shuriken", transform.position + direction, Quaternion.identity).GetComponent<Shuriken>().Set_Shuriken(direction, photonView);
            arm.SetTrigger("Attack1");
        }
        else
        {
            PhotonNetwork.Instantiate("InGame/Ninja/Kunai", transform.position + direction, Quaternion.identity).GetComponent<Kunai>().Set_Kunai(direction, photonView);
            arm.SetTrigger("Attack2");
        }
        
        timer = 0.25f;
        attackCount++;
    }
}

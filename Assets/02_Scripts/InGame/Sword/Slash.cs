using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Slash : MonoBehaviourPunCallbacks
{
    bool isHit;
    public Sword s;
    private void OnEnable()
    {
        isHit = false;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (isHit) return;
        if (coll.GetComponent<PhotonView>() != null)
        {
            if (!photonView.IsMine && coll.GetComponent<PhotonView>().IsMine)
            {
                coll.GetComponent<PlayerController>().Hit(5f, photonView);
                isHit = true;
            }
        }

    }
    public void DisableGO()
    {
        gameObject.SetActive(false);
        s.isAttack = false;
    }
}

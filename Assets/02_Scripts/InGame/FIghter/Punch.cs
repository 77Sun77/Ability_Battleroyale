using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Punch : MonoBehaviourPunCallbacks
{
    bool isHit;
    public Fighter f;
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
                coll.GetComponent<PlayerController>().Hit(4f, photonView);
                Vector2 dir = Vector2.zero;
                if(f.pc.flip_X)
                {
                    dir = Vector2.left;
                }
                else
                {
                    dir = Vector2.right;
                }
                coll.GetComponent<PlayerController>().KnockBack(dir);
                isHit = true;
            }
        }

    }
    public void DisableGO()
    {
        gameObject.SetActive(false);
        f.isAttack = false;
    }
}

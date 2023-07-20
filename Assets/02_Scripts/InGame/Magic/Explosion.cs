using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Explosion : MonoBehaviourPunCallbacks, IPunObservable
{
    bool isHit;
    PhotonView pv;
    public Player id;
    public float Damage;

    public bool isItem;
    public void Set_Explosion(PhotonView pv)
    {
        this.pv = pv;
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.GetComponent<PhotonView>() != null)
        {
            if ((isItem && photonView.IsMine && !isHit) || (!photonView.IsMine && coll.GetComponent<PhotonView>().IsMine && !isHit))
            {
                PhotonView pv = null;
                foreach (PlayerController player in GameManager.instance.Players)
                {
                    if (player.photonView.Owner == id)
                    {
                        pv = player.photonView;
                    }
                }
                isHit = true;
                if(isItem) coll.GetComponent<PlayerController>().Hit(Damage, this.pv);
                else coll.GetComponent<PlayerController>().Hit(Damage, pv);

            }
        }
        
    }

    public void GO_Destroy()
    {
        Destroy(gameObject);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(pv.Owner);
        }
        else
        {
            id = (Player)stream.ReceiveNext();
        }
    }
}

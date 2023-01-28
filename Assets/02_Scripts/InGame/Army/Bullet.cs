using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Bullet : MonoBehaviourPunCallbacks, IPunObservable
{
    Vector2 dir;
    Rigidbody2D myRigid;
    public PhotonView pv;
    public Player id;
    
    public void Set_Bullet(Vector2 dir, PhotonView pv)
    {
        this.dir = dir;
        this.pv = pv;
    }

    private void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            myRigid.velocity = dir * 20;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (photonView.IsMine && coll.CompareTag("Ground"))
        {
            photonView.RPC("AllDestroy", RpcTarget.AllBuffered);
        }
        else if(coll.GetComponent<PhotonView>() != null)
        {
            if (!photonView.IsMine && coll.GetComponent<PhotonView>().IsMine)
            {
                PhotonView pv = null;
                foreach(PlayerController player in GameManager.instance.Players)
                {
                    if(player.photonView.Owner == id)
                    {
                        pv = player.photonView;
                    }
                }
                coll.GetComponent<PlayerController>().Hit(1.5f, pv);
                photonView.RPC("AllDestroy", RpcTarget.AllBuffered);

            }
        }
        
    }

    [PunRPC]
    public void AllDestroy()
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

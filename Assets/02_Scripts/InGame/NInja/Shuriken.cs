using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Shuriken : MonoBehaviourPunCallbacks, IPunObservable
{
    Vector2 dir;
    Rigidbody2D myRigid;
    Transform child;
    public PhotonView pv;
    public Player id;
    public void Set_Shuriken(Vector2 dir, PhotonView pv)
    {
        this.dir = dir;
        this.pv = pv;
    }

    private void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        child = transform.GetChild(0);
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            myRigid.velocity = dir * 10;
        }

        if (myRigid.velocity.x < 0)
        {
            child.Rotate(Vector3.forward * 360 * Time.deltaTime);
        }
        else
        {
            child.Rotate(Vector3.back * 360 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (photonView.IsMine && coll.CompareTag("Ground"))
        {
            photonView.RPC("AllDestroy", RpcTarget.AllBuffered);
        }
        else if (coll.GetComponent<PhotonView>() != null)
        {
            if (!photonView.IsMine && coll.GetComponent<PhotonView>().IsMine)
            {
                PhotonView pv = null;
                foreach (PlayerController player in GameManager.instance.Players)
                {
                    if (player.photonView.Owner == id)
                    {
                        pv = player.photonView;
                    }
                }
                coll.GetComponent<PlayerController>().Hit(3f, pv);
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

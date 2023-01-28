using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Magic : MonoBehaviourPunCallbacks
{
    Vector2 dir;
    Rigidbody2D myRigid;
    public PhotonView pv;
    public void Set_Magic(Vector2 dir, PhotonView pv)
    {
        this.dir = dir;
        this.pv = pv;
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            myRigid = GetComponent<Rigidbody2D>();
            Vector3 vec = new Vector3(0, 0, dir.x) * 90;
            transform.GetChild(0).rotation = Quaternion.Euler(vec);
        }
        
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            myRigid.velocity = dir * 6;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (photonView.IsMine && coll.CompareTag("Ground"))
        {
            Explosion();

        }
        else if (coll.GetComponent<PhotonView>() != null)
        {
            if (photonView.IsMine && !coll.GetComponent<PhotonView>().IsMine)
            {
                Explosion();
            }
        }
        
    }

    void Explosion()
    {
        Vector3 vec = dir*0.5f;
        PhotonNetwork.Instantiate("InGame/Magic/Explosion", transform.position + vec, Quaternion.identity).GetComponent<Explosion>().Set_Explosion(pv);
        photonView.RPC("AllDestroy", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void AllDestroy()
    {
        Destroy(gameObject);
    }

}

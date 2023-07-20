using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GetItem : MonoBehaviour
{
    public AudioClip getSound;
    public virtual void Attack(PlayerController player)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && collision.GetComponent<PhotonView>().IsMine)
        {
            Attack(collision.GetComponent<PlayerController>());
            GetComponent<PhotonView>().RPC("AllDestroy", RpcTarget.AllBuffered);
            SoundManager.instance.AS.PlayOneShot(getSound);
        }
    }

    [PunRPC]
    public void AllDestroy()
    {
        Destroy(gameObject);
    }
}

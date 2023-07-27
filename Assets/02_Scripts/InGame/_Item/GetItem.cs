using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GetItem : MonoBehaviour
{
    public AudioClip getSound;
    public GameObject itemPrefab;
    public virtual void Attack(PlayerController player)
    {

    }

    private void Start()
    {
        Instantiate(itemPrefab, GameObject.Find("Canvas").transform);
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

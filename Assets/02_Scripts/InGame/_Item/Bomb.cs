using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Bomb : GetItem
{
    public override void Attack(PlayerController player)
    {

        GetComponent<PhotonView>().RPC("SpawnBomb", RpcTarget.AllBuffered, player.photonView.Owner.ActorNumber);

        
    }

    [PunRPC]
    public void SpawnBomb(int player)
    {
        if(player != GameManager.instance.player.photonView.Owner.ActorNumber)
        {
            PlayerController pc = null;
            foreach(GameObject p in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (p.GetComponent<PlayerController>().photonView.Owner.ActorNumber == player) pc = p.GetComponent<PlayerController>();
            }
            PhotonNetwork.Instantiate("Item/BombExplosion", GameManager.instance.player.transform.position, Quaternion.identity).GetComponent<BombExplosion>().Setting(pc);
        }
            
    }
}

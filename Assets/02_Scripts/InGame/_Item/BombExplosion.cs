using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    public Explosion ep;

    public void Setting(PlayerController player)
    {
        ep.Set_Explosion(player.photonView);
        ep.isItem = true;
    }
}

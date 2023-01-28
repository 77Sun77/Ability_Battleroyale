using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class NameText : MonoBehaviourPunCallbacks
{
    public Outline text1, text2;
    public Color[] Colors;

    void Update()
    {
        if (photonView.IsMine)
        {
            text1.effectColor = Colors[0];
            text2.effectColor = Colors[0];
        }
        else
        {
            text1.effectColor = Colors[1];
            text2.effectColor = Colors[1];
        }
    }
}

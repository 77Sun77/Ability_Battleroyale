using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSound : MonoBehaviour
{
    public AudioClip attackSound;


    private void OnEnable()
    {
        SoundManager.instance.AS.PlayOneShot(attackSound);

    }
}

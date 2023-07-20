using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource AS;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        AS = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        
    }
}

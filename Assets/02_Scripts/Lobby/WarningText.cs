using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningText : MonoBehaviour
{
    public float timer = 2;
    void Start()
    {
        Destroy(gameObject, timer);
    }

}

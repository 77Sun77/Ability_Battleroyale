using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnim : MonoBehaviour
{
    public GameObject obj;
    public void DestroyThis()
    {
        if (obj) Destroy(obj);
        else Destroy(gameObject);
    }
}

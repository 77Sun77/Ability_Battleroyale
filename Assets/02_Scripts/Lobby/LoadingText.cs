using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    public enum Kind { UI, Map }
    public Kind kind;
    public Text text;
    void Start()
    {
        if(kind == Kind.UI) Invoke("One", 0.5f);
        else Invoke("One_2", 0.5f);
    }


    void One()
    {
        text.text = "Loading.";
        Invoke("Two", 0.5f);
    }

    void Two()
    {
        text.text = "Loading..";
        Invoke("Three", 0.5f);
    }

    void Three()
    {
        text.text = "Loading...";
        Invoke("One", 0.5f);
    }

    void One_2()
    {
        text.text = ".";
        Invoke("Two_2", 0.5f);
    }
    void Two_2()
    {
        text.text = "..";
        Invoke("Three_2", 0.5f);
    }
    void Three_2()
    {
        text.text = "...";
        Invoke("One_2", 0.5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetNickname : MonoBehaviour
{
    public InputField name;
    public void Set_Name()
    {
        if(name.text.Length < 2 || name.text.Length > 8)
        {
            name.text = "";
            name.placeholder.GetComponent<Text>().text = "다시 입력해주세요";
            return;
        }
        name.placeholder.GetComponent<Text>().text = "";
        LobbyManager.instance.Set_Name(name.text);
        StartCoroutine(LobbyManager.instance.Disable_UI(gameObject));
        name.text = "";

    }
}

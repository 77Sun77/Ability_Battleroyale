using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] circles;
    public GameObject[] characterPages;
    public int pageCount;

    void OnEnable()
    {
        pageCount = 0;
        Set_UI();
    }

    private void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void OnClick_Left()
    {
        if(pageCount == 0)
        {
            pageCount = 4;
        }
        else
        {
            pageCount--;
        }
        Set_UI();
    }
    public void OnClick_Right()
    {
        if (pageCount == 4)
        {
            pageCount = 0;
        }
        else
        {
            pageCount++;
        }
        Set_UI();
    }

    void Set_UI()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i == pageCount)
            {
                circles[i].SetActive(true);
                characterPages[i].SetActive(true);
            }
            else
            {
                circles[i].SetActive(false);
                characterPages[i].SetActive(false);
            }
        }
    }

    public void Select_Char()
    {
        PlayerController.Weapons weapon;
        if (pageCount == 0)
        {
            weapon = PlayerController.Weapons.Sword;
        }
        else if (pageCount == 1)
        {
            weapon = PlayerController.Weapons.Magic;
        }
        else if (pageCount == 2)
        {
            weapon = PlayerController.Weapons.Army;
        }
        else if(pageCount == 3)
        {
            weapon = PlayerController.Weapons.Ninja;
        }
        else
        {
            weapon = PlayerController.Weapons.Fighter;
        }
        RoomManager.instance.LobbyCharDestroy();
        RoomManager.instance.LobbyCharSpawn(weapon);
        gameObject.SetActive(false);
    }
}

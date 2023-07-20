using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : GetItem
{
    public GameObject prefab;
    public override void Attack(PlayerController player)
    {
        player.hp += 5;
        Instantiate(prefab, transform.position, Quaternion.identity);

    }
}

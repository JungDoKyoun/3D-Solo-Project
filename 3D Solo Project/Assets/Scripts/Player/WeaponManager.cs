using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    PlayerController player;
    DropItem item;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        item = GetComponent<DropItem>();
    }

    //데미지 입힐때
    private void OnTriggerEnter(Collider other)
    {
        MonsterController monsterController = other.GetComponent<MonsterController>();

        if (other.CompareTag("Enemy") && !item.IsDroop)
        {
            monsterController.TakeDamage(player.CurrentAtk);
        }
    }
}

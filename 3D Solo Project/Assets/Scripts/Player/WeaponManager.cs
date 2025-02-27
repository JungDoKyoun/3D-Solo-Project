using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    PlayerController player;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
    }

    //데미지 입힐때
    private void OnTriggerEnter(Collider other)
    {
        MonsterController monsterController = other.GetComponent<MonsterController>();

        if (other.CompareTag("Enemy"))
        {
            monsterController.TakeDamage(player.CurrentAtk);
        }
    }
}

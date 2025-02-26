using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeaponManager : MonoBehaviour
{
    MonsterController monster;

    private void Awake()
    {
        monster = GetComponent<MonsterController>();
    }

    //몬스터가 플레이어 데미지 입힐때
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = GetComponent<PlayerController>();

        if (other.CompareTag("Player"))
        {
            player.TakeDamae(monster.Damage);
        }
    }
}

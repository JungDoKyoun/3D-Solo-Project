using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeaponManager : MonoBehaviour
{
    MonsterController monster;

    private void Awake()
    {
        monster = GetComponentInParent<MonsterController>();
    }

    //몬스터가 플레이어 데미지 입힐때
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (other.CompareTag("Player"))
        {
            player.TakeDamae(monster.Damage);
            Debug.Log("플레이어 체력" + player.CurrentHP);
        }
    }
}

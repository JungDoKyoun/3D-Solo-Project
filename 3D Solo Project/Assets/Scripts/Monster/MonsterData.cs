using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterData
{
    public GameObject prefab; //몬스터 프리팹
    public int id; //몬스터 종류 나타내는 ID
    public string name; //몬스터 이름
    public int maxHP; //최대체력
    public int damage; //데미지
    public int def; //방어력
    public int detectingRange; //감지 범위
    public int maxCount; //최대 생성갯수
    public float patrolSpeed; //순찰속도
    public float chaseSpeed; //추격속도
}


[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Monster/MonsterData")]
public class MonsterDataSO : ScriptableObject
{
    public MonsterData[] monsters;
}

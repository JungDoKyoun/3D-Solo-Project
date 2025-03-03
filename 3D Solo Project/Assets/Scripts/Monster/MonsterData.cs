using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterData
{
    public GameObject prefab; //몬스터 프리팹

    [Header("몬스터 정보")]
    public int id; //몬스터 종류 나타내는 ID
    public string name; //몬스터 이름
    public float maxHP; //최대체력
    public int detectingRange; //감지 범위
    public int maxCount; //최대 생성갯수
    public int spawnRange;

    [Header("이동 관련")]
    public float patrolSpeed; //순찰속도
    public float chaseSpeed; //추격속도
    public int randomRange;

    [Header("땅 감지 관련")]
    public float groundSphereRadius;
    public float groundSphereOffSet;

    [Header("공격 관련")]
    public int damage; //데미지
    public int def; //방어력
    public float attackDel; //공격 딜레이
    public float st;
    public float ed;
    public float animeTime;

    [Header("드롭 아이템")]
    public List<ItemDataSO> dropItems;
    public int maxDropCount = 3;
    public int Score;
}


[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Monster/MonsterData")]
public class MonsterDataSO : ScriptableObject
{
    public MonsterData[] monsters;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterData
{
    public GameObject prefab;
    public int id;
    public string name;
    public int hp;
    public int damage;
    public int maxCount;
}


[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Monster/MonsterData")]
public class MonsterDataSO : ScriptableObject
{
    public MonsterData[] monsters;
}

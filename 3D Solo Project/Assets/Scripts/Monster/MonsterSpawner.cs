using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]private MonsterDataSO monsterDataSO;
    private Dictionary<int, ObjectPool<GameObject>> monsterPool;
    private Dictionary<int, int> monsterMaxCount;

    private void Start()
    {
        monsterPool = new Dictionary<int, ObjectPool<GameObject>>();
        monsterMaxCount = new Dictionary<int, int>();
        foreach (var monster in monsterDataSO.monsters)
        {
            monsterMaxCount[monster.id] = monster.maxCount;

            monsterPool[monster.id] = new ObjectPool<GameObject>(
                createFunc: () => CreateMonster(monster.id),
                actionOnGet : obj => obj.SetActive(true),
                actionOnRelease : obj => obj.SetActive(false),
                actionOnDestroy : obj => Destroy(obj),
                collectionCheck : false,
                defaultCapacity : 10,
                maxSize : monster.maxCount
                );
        }
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Alpha1)))
        {
            SpawnMonster(0);
        }
    }

    private GameObject CreateMonster(int id)
    {
        GameObject monster = monsterDataSO.monsters[id].prefab;
        return Instantiate(monster);
    }

    public void SpawnMonster(int monsterID)
    {
        if(monsterPool.ContainsKey(monsterID))
        {
            int activeCount = monsterPool[monsterID].CountActive;
            int maxCount = monsterMaxCount[monsterID];

            if(activeCount <= maxCount)
            {
                GameObject monster = monsterPool[monsterID].Get();
                monster.transform.position = transform.position;
            }
        }
    }

    public void ReturnMonster(GameObject monster, int monsterID)
    {
        if(monsterPool.ContainsKey(monsterID))
        {
            monsterPool[monsterID].Release(monster);
        }
        else
        {
            Destroy(monster);
        }
    }
}

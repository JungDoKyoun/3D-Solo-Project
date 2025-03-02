using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]private MonsterDataSO monsterDataSO;
    private Dictionary<int, ObjectPool<GameObject>> monsterPool;
    private Dictionary<int, int> monsterMaxCount;
    [SerializeField] int spawnRange;

    private void Awake()
    {
        
    }

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
        GameObject monsterPrefab = monsterDataSO.monsters[id].prefab;
        GameObject monster = Instantiate(monsterPrefab);

        MonsterController monsterController = monster.GetComponent<MonsterController>();
        if(monsterController != null)
        {
            monsterController.Initialize(monsterDataSO.monsters[id]);
        }
        return monster;
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
                Vector3 spawnPos = GetRandomNavMeshPos(monsterID);
                monster.transform.position = spawnPos;
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

    public Vector3 GetRandomNavMeshPos(int monsterID)
    {
        int maxAttempts = 10;
        Vector3 spawnPos = transform.position;
        float spawnRange = monsterDataSO.monsters[monsterID].spawnRange;

        for(int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomDir = Random.insideUnitSphere * spawnRange;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDir, out hit, spawnRange, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        spawnPos += new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        return spawnPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}

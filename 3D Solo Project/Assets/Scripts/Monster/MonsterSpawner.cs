using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

        for(int i = 0; i < 50; i++)
        {
            SpawnMonster(0);
            SpawnMonster(1);
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
        StartCoroutine(RespawnAfterDelay(monster));
    }

    public Vector3 GetRandomNavMeshPos(int monsterID)
    {
        int maxAttempts = 10;
        Vector3 spawnPos = transform.position;
        float spawnRange = monsterDataSO.monsters[monsterID].spawnRange;

        for(int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomDir = transform.position + Random.insideUnitSphere * spawnRange;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDir, out hit, spawnRange, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        spawnPos += new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
        return spawnPos;
    }

    public IEnumerator RespawnAfterDelay(GameObject monster)
    {
        yield return new WaitForSeconds(10f);
        MonsterController controller = monster.GetComponent<MonsterController>();

        if(controller != null)
        {
            controller.ResetMonster();
        }

        monster.gameObject.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 30);
    }
}

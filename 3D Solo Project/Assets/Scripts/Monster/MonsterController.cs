using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private GameObject hpBarPrefab;
    private Vector3 hpBarOffSet;
    private Canvas uiCanvas;
    private Image hpBarImage;
    private MonsterHPBar hpBarInstance;
    private MonsterSpawner spawner;
    private MonsterData data;
    private BoxCollider weaponCollider;
    private Rigidbody monsterRb;
    private NavMeshAgent agent;
    private MonsterAnimeController anime;
    private LayerMask player;
    private LayerMask ground;
    private Vector3 originTR;
    private float _currentMonsterHP;
    private float timeSinceLastUpdate;
    private int _damage;
    [SerializeField] private int _random;
    private bool _isDie;
    private bool _isChase;
    private bool _isAttack;

    public MonsterData Data { get => data; set => data = value; }
    public Rigidbody MonsterRb { get => monsterRb; set => monsterRb = value; }
    public MonsterAnimeController Anime { get => anime; set => anime = value; }
    public NavMeshAgent Agent { get => agent; set => agent = value; }
    public int Damage { get => _damage; set => _damage = value; }
    public bool IsChase { get => _isChase; set => _isChase = value; }
    public bool IsAttack { get => _isAttack; set => _isAttack = value; }
    public bool IsDie { get => _isDie; set => _isDie = value; }

    private void Awake()
    {
        weaponCollider = GetComponentInChildren<BoxCollider>();
        player = LayerMask.GetMask("Player");
        ground = LayerMask.GetMask("Ground");
        monsterRb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        anime = GetComponent<MonsterAnimeController>();
        spawner = FindAnyObjectByType<MonsterSpawner>();
        hpBarInstance = FindAnyObjectByType<MonsterHPBar>();
        _random = 20;
        _isDie = false;
        _isChase = false;
        _isAttack = false;
        hpBarOffSet = new Vector3(0, 1.7f, 0);
    }

    private void Start()
    {
        SetHpBar();
        Debug.Log(hpBarInstance);
    }

    //몬스터 데이터 초기화
    public void Initialize(MonsterData monsterData)
    {
        data = monsterData;
        _currentMonsterHP = data.maxHP;
        _damage = data.damage;
        _isDie = false;
        _isChase = false;
        _isAttack = false;
    }

    //몬스터가 데미지 받았을 때
    public void TakeDamage(int damage)
    {
        _currentMonsterHP -= damage - data.def;
        Debug.Log("현재 몬스터 체력" + _currentMonsterHP);
        hpBarImage.fillAmount = _currentMonsterHP / data.maxHP;

        if (_currentMonsterHP <= 0)
        {
            _currentMonsterHP = 0;
            MonsterDie();
        }
    }

    //몬스터 죽음
    public void MonsterDie()
    {
        GameManager.Instance.AddScore(data.Score);
        Destroy(hpBarInstance.gameObject);
        hpBarInstance = null;
        DropItem();
        _isDie = true;
    }

    //몬스터 무기 콜라이더 켜기
    public void OnMonsterWeaponCollider()
    {
        weaponCollider.enabled = true;
    }

    //몬스터 무기 콜라이더 끄기
    public void OffMonsterWeaponCollider()
    {
        weaponCollider.enabled = false;
    }

    //주변에 플레이가 있나 탐색
    public Collider DetectPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Data.detectingRange, player);
        if (hitColliders.Length > 0)
        {
            Collider target = hitColliders[0];
            return target;
        }
        return null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _random);
    }

    //플레이어 쫒기
    public void ChasePlayer(Collider target)
    {
        agent.speed = data.chaseSpeed;
        if (target == null)
        {
            return;
        }
        agent.SetDestination(target.transform.position);
        _isChase = true;
    }

    //플레이어와 적의 거리 측정
    public float CheckDistance(Collider target)
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        return distance;
    }

    //네비메쉬 멈춤
    public void StopChase()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    //공격
    public IEnumerator Attack(Transform target)
    {
        float tempTime = 0;
        _isAttack = true;
        LookAtTarget(target.transform);
        anime.PlayAttackAnime(true);
        yield return null;
        while (tempTime < data.st)
        {
            tempTime += Time.deltaTime;
            yield return null;
        }
        OnMonsterWeaponCollider();
        while (tempTime < data.ed)
        {
            tempTime += Time.deltaTime;
            yield return null;
        }
        OffMonsterWeaponCollider();
        while (tempTime < data.animeTime)
        {
            tempTime += Time.deltaTime;
            yield return null;
        }
        anime.PlayAttackAnime(false);
        while (tempTime < data.attackDel)
        {
            tempTime += Time.deltaTime;
            yield return null;
        }
        _isAttack = false;
    }

    public void LookAtTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; 

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;
    }

    //네비메쉬 랜덤 포지션 받기
    Vector3 GetRandomPositionOnNavMesh()
    {
        Vector3 randomDir = Random.insideUnitSphere * data.randomRange;
        randomDir += transform.position;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomDir, out hit, data.randomRange, NavMesh.AllAreas))
        {
            if (!agent.isOnNavMesh)
            {
                agent.Warp(hit.position);
                Debug.Log("sd");
            }
            return hit.position;
        }
        else
        {
            return transform.position;
        }
    }

    //랜덤하게 정찰
    public IEnumerator Patrol()
    {
        agent.speed = data.patrolSpeed;
        Vector3 randomPos = GetRandomPositionOnNavMesh();
        agent.SetDestination(randomPos);
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
        
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void DropItem()
    {
        if (data.dropItems.Count == 0)
        {
            return;
        }
        int droppedCount = 0;

        foreach (var dropInfo in data.dropItems)
        {
            if (droppedCount >= data.maxDropCount)
            {
                break;
            }
            float roll = Random.Range(0f, 100f);
            if (roll <= dropInfo.dropChance)
            {
                GameObject drop = Instantiate(dropInfo.ItemPrefab, transform.position + RandomDropOffset(), Quaternion.identity);
                DropItem dropItem = drop.GetComponent<DropItem>();
                SphereCollider dropColl = drop.GetComponent<SphereCollider>();
                if (dropItem != null)
                {
                    dropItem.SetItem(dropInfo);
                    droppedCount++;
                    dropColl.enabled = true;    
                }
            }
        }

        //ItemDataSO randomItem = data.dropItems[Random.Range(0, data.dropItems.Count)];
        //GameObject drop = Instantiate(dropItemPrefab, dropPoint.position, Quaternion.identity);
        //DropItem dropItem = drop.GetComponent<DropItem>();

        //if (dropItem != null)
        //{
        //    dropItem.SetItem(randomItem);
        //    dropItem.IsDroop = true;
        //    dropItem.Collider.enabled = true;
        //}
    }

    public Vector3 RandomDropOffset()
    {
        return new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
    }

    public IEnumerator RespawnMonster()
    {
        if (spawner != null)
        {
            yield return new WaitForSeconds(5f);
            spawner.ReturnMonster(gameObject, data.id);
        }
    }

    public void ResetMonster()
    {
        transform.position = spawner.GetRandomNavMeshPos(data.id);
        agent.enabled = true;
        MonsterRb.isKinematic = false;
        _currentMonsterHP = data.maxHP;
        IsDie = false;

        if (GetComponent<Collider>())
        {
            GetComponent<Collider>().enabled = true;
        }

        if (hpBarPrefab != null)
        {
            GameObject hpBarObj = Instantiate(hpBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity, FindObjectOfType<Canvas>().transform);
            hpBarInstance = hpBarObj.GetComponent<MonsterHPBar>();
        }
    }

    public void SetHpBar()
    {
        uiCanvas = GameObject.Find("MonsterHp").GetComponent<Canvas>();
        GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
        hpBarInstance = hpBar.GetComponent<MonsterHPBar>();

        var hp = hpBar.GetComponent<MonsterHPBar>();
        hp.TargetTr = this.gameObject.transform;
        hp.Offset = hpBarOffSet;
    }
}

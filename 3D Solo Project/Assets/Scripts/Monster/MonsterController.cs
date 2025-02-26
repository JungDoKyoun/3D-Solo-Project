using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class MonsterController : MonoBehaviour
{
    private MonsterData data;
    private BoxCollider weaponCollider;
    private Rigidbody monsterRb;
    private NavMeshAgent agent;
    private MonsterAnimeController anime;
    private LayerMask player;
    private LayerMask ground;
    private Vector3 originTR;
    private int _currentMonsterHP;
    [SerializeField] private float updateInterval;
    private float timeSinceLastUpdate;
    private int _damage;
    [SerializeField] private float _groundSphereRadius;
    [SerializeField] private float _groundSphereOffSet;
    [SerializeField] private int _detectingRange;
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

    private void Awake()
    {
        weaponCollider = GetComponentInChildren<BoxCollider>();
        player = LayerMask.GetMask("Player");
        ground = LayerMask.GetMask("Ground");
        monsterRb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        anime = GetComponent<MonsterAnimeController>();
        updateInterval = 5f;
        _groundSphereRadius = 0.5f;
        _groundSphereOffSet = 0.5f;
        _detectingRange = 10;
        _random = 20;
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

        if (_currentMonsterHP <= 0)
        {
            _currentMonsterHP = 0;
            MonsterDie();
        }
    }

    //몬스터 죽음
    public void MonsterDie()
    {
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


    //바닥 감지
    //public bool CheckIsGround()
    //{
    //    originTR = transform.position;
    //    originTR.y += data.groundSphereOffSet;

    //    var coll = Physics.OverlapSphere(originTR, data.groundSphereRadius, ground);
    //    _isGround = coll.Length > 0;
    //    Debug.Log(_isGround);
    //    return _isGround;
    //}

    //private void OnDrawGizmos()
    //{
    //    originTR = transform.position;
    //    originTR.y += _groundSphereOffSet;
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(originTR, _groundSphereRadius);
    //}

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
    public IEnumerator Attack()
    {
        _isAttack = true;
        anime.PlayAttackAnime(true);
        yield return new WaitForSeconds(data.attackDel);
        anime.PlayAttackAnime(false);
        _isAttack = false;
    }

    //네비메쉬 랜덤 포지션 받기
    Vector3 GetRandomPositionOnNavMesh()
    {
        Vector3 randomDir = Random.insideUnitSphere * data.randomRange;
        randomDir += transform.position;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomDir, out hit, data.randomRange, NavMesh.AllAreas))
        {
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
        //timeSinceLastUpdate += Time.deltaTime;

        //if(timeSinceLastUpdate >= updateInterval)
        //{
        //    Vector3 randomPos = GetRandomPositionOnNavMesh();
        //    agent.SetDestination(randomPos);
        //    timeSinceLastUpdate = 0;
        //}
    }
}

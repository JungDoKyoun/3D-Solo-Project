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
    private Collider weaponCollider;
    private Rigidbody monsterRb;
    private NavMeshAgent agent;
    private LayerMask player;
    private LayerMask ground;
    private Vector3 originTR;
    private int _currentMonsterHP;
    private int _damage;
    [SerializeField] private float _groundSphereRadius;
    [SerializeField] private float _groundSphereOffSet;
    [SerializeField] private int _detectingRange;
    private bool _isDie;
    private bool _isGround;

    public MonsterData Data { get => data; set => data = value; }
    public Rigidbody MonsterRb { get => monsterRb; set => monsterRb = value; }
    public int Damage { get => _damage; set => _damage = value; }
    public bool IsGround { get => _isGround; set => _isGround = value; }

    private void Awake()
    {
        weaponCollider = GetComponentInChildren<Collider>();
        player = LayerMask.GetMask("Player");
        ground = LayerMask.GetMask("Ground");
        monsterRb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        _groundSphereRadius = 0.5f;
        _groundSphereOffSet = 0.5f;
        _detectingRange = 10;
    }

    //몬스터 데이터 초기화
    public void Initialize(MonsterData monsterData)
    {
        data = monsterData;
        _currentMonsterHP = data.maxHP;
        _damage = data.damage;
        _isDie = false;
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
        Gizmos.DrawWireSphere(transform.position, _detectingRange);
    }


    //바닥 감지
    public bool CheckIsGround()
    {
        originTR = transform.position;
        originTR.y += data.groundSphereOffSet;

        var coll = Physics.OverlapSphere(originTR, data.groundSphereRadius, ground);
        _isGround = coll.Length > 0;

        return _isGround;
    }

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
        if(target == null)
        {
            return;
        }
        agent.SetDestination(target.transform.position);
    }
}

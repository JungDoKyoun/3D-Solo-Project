using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("플레이어 움직이는 동작 관련")]
    [SerializeField] float _playerMoveSpeed;
    [SerializeField] float _playerRotationSpeed;
    [SerializeField] float _playerSprintSpeed;
    private float _magnitude;//애니메이션 컨트롤에 줄 속력
    [SerializeField] bool _isSprint;
    [SerializeField] bool _wasGround;

    [Header("플레이어 추락 및 착지")]
    [SerializeField] LayerMask _groundLayerMask;
    [SerializeField] float _playerFallenSpeed;
    [SerializeField] float _playerDirFallenSpeed;
    [SerializeField] float _inAirTime;
    [SerializeField] float _rayCastHightOffset;
    [SerializeField] float _fallenSphereRadius;
    [SerializeField] bool _isGround;

    [Header("플레이어 점프")]
    [SerializeField] float _jumpForce;
    [SerializeField] float _gravityForce;
    [SerializeField] bool _isJump;

    [Header("플레이어 계단 오르내리기")]
    [SerializeField] Transform _rawerRay;
    [SerializeField] Transform _upperRay;
    [SerializeField] float _stepSmooth;
    [SerializeField] float _stepHight;

    [Header("플레이어 경사로 오르내리기")]
    [SerializeField] float _maxAngle;
    [SerializeField] float _maxDistance;
    [SerializeField] float _angleResult;
    [SerializeField] bool _isSloop;

    [Header("플레이어 공격")]
    [SerializeField] float _attackPower;
    [SerializeField] bool _isAttack;

    private void Awake()
    {
        PlayerMoveSpeed = 3f;
        PlayerRotationSpeed = 10f;
        PlayerSprintSpeed = 5f;
        PlayerFallenSpeed = 30;
        InAirTime = 0f;
        RayCastHightOffset = 0.15f;
        FallenSphereRadius = 0.2f;
        JumpPower = 5;
        GravityForce = -9.8f;
        StepSmooth = 0.1f;
        StepHight = 0.3f;
        MaxAngle = 35;
        MaxDistance = 2;
        AttackPower = 10f;
        IsSprint = false;
        IsGround = true;
        IsJump = false;
        IsSloop = false;
        IsAttack = false;
    }

    public float PlayerMoveSpeed { get => _playerMoveSpeed;  set => _playerMoveSpeed = value; }
    public float PlayerRotationSpeed { get => _playerRotationSpeed; set =>_playerRotationSpeed = value; }
    public float PlayerSprintSpeed { get => _playerSprintSpeed; set => _playerSprintSpeed = value; }
    public float PlayerFallenSpeed { get => _playerFallenSpeed; set => _playerFallenSpeed = value; }
    public float PlayerDirFallenSpeed { get => _playerDirFallenSpeed; set => _playerDirFallenSpeed = value; }
    public float InAirTime { get => _inAirTime; set => _inAirTime = value; }
    public float RayCastHightOffset { get => _rayCastHightOffset; set => _rayCastHightOffset = value; }
    public float FallenSphereRadius { get => _fallenSphereRadius; set => _fallenSphereRadius = value; }
    public float JumpPower { get => _jumpForce; set => _jumpForce = value; }
    public float GravityForce { get => _gravityForce; set => _gravityForce = value; }
    public float StepSmooth { get => _stepSmooth; set => _stepSmooth = value; }
    public float StepHight { get => _stepHight; set => _stepHight = value; }
    public float MaxAngle { get => _maxAngle; set => _maxAngle = value; }
    public float MaxDistance { get => _maxDistance; set => _maxDistance = value; }
    public float Magnitude { get => _magnitude; set => _magnitude = value; }
    public float AngleResult { get => _angleResult; set => _angleResult = value; }
    public float AttackPower { get => _attackPower; set => _attackPower = value; }
    public bool IsSprint { get => _isSprint; set => _isSprint = value; }
    public bool IsGround { get => _isGround; set => _isGround = value; }
    public bool IsJump { get => _isJump; set => _isJump = value; }
    public bool IsSloop { get => _isSloop; set => _isSloop = value; }
    public bool IsAttack { get => _isAttack; set => _isAttack = value; }
    public bool WasGround { get => _wasGround; set => _wasGround = value; }
    public LayerMask GroundLayerMask { get => _groundLayerMask; set => _groundLayerMask = value; }
    public Transform RawerRay { get => _rawerRay; set => _rawerRay = value; }

    public Transform UpperRay { get => _upperRay; set => _upperRay = value; }
}

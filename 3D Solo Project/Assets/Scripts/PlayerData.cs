using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("플레이어 움직이는 동작 관련")]
    [SerializeField] float _playerMoveSpeed;
    [SerializeField] float _playerRotationSpeed;
    [SerializeField] float _playerSprintSpeed;
    [SerializeField] bool _isSprint;

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
    [SerializeField] bool _isJumping;

    private void Awake()
    {
        PlayerMoveSpeed = 3f;
        PlayerRotationSpeed = 10f;
        PlayerSprintSpeed = 5f;
        PlayerFallenSpeed = 30;
        InAirTime = 0f;
        RayCastHightOffset = 0.5f;
        FallenSphereRadius = 0.2f;
        JumpPower = 5;
        GravityForce = -9.8f;
        IsSprint = false;
        IsGround = true;
        IsJump = false;
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
    public bool IsSprint { get => _isSprint; set => _isSprint = value; }
    public bool IsGround { get => _isGround; set => _isGround = value; }
    public bool IsJump { get => _isJump; set => _isJump = value; }
    public bool IsJumping { get => _isJumping; set => _isJumping = value; }
    public LayerMask GroundLayerMask { get => _groundLayerMask; set => _groundLayerMask = value; }
}

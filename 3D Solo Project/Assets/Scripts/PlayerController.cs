using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PlayerController : MonoBehaviour
{
    private PlayerData playerData;//플레이어 데이터
    private Rigidbody playerRb;//리지드 바디
    private Camera cam;//메인 카메라
    private AnimationController anime;
    private BoxCollider weaPon; //무기
    private RaycastHit sloopHit;
    private Vector3 moveDir;//움직이는 방향
    private Vector2 inputMoveDir;//인풋 변수 받아옴
    private bool wasGround = true;

    private void Awake()
    {
        playerData = GetComponent<PlayerData>();
        playerRb = GetComponent<Rigidbody>();
        cam = Camera.main;
        anime = GetComponent<AnimationController>();
        weaPon = GetComponentInChildren<BoxCollider>();
        moveDir = Vector3.zero;
        playerData.UpperRay.position = new Vector3(playerData.UpperRay.position.x, playerData.StepHight, playerData.UpperRay.position.y);
    }

    public PlayerData PlayerData { get => playerData; set => playerData = value; }
    public Rigidbody PlayerRb { get => playerRb; set => playerRb = value; }
    public Camera Cam { get => cam; set => cam = value; }
    public AnimationController Anime { get => anime; set => anime = value; }
    public BoxCollider WeaPon { get => weaPon; set => weaPon = value; }
    public RaycastHit SloopHit { get => sloopHit; set => sloopHit = value; }
    public Vector3 MoveDir { get => moveDir; set => moveDir = value; }
    public Vector3 InputMoveDir { get => inputMoveDir; set => inputMoveDir = value; }

    //플레이어가 뛰나 안뛰나 여부 판단
    public void SetSprint(bool TorF)
    {
        playerData.IsSprint = TorF;
    }

    //뛰는지 여부 전달
    public bool GetSprint()
    {
        return playerData.IsSprint;
    }

    //플레이어가 땅에 있는지 여부 판단
    public bool CheckIsGround()
    {
        Vector3 originalPos = transform.position;
        originalPos.y += playerData.RayCastHightOffset;

        var coll = Physics.OverlapSphere(originalPos, playerData.FallenSphereRadius, playerData.GroundLayerMask);
        bool isGround = coll.Length > 0;
        playerData.IsGround = isGround;
        return isGround;
    }

    //플레이어 착지
    public void Landing()
    {
        playerData.IsGround = true;
        playerData.InAirTime = 0;
    }

    //점프 참거짓
    public void SetJump(bool TorF)
    {
        playerData.IsJump = TorF;
    }

    //점프 참거짓 반환
    public bool GetJump()
    {
        return playerData.IsJump;
    }

    //계단 인지 아닌지
    public bool CheckStair()
    {
        RaycastHit rawHit;
        if (Physics.Raycast(playerData.RawerRay.position, transform.TransformDirection(Vector3.forward), out rawHit, 0.1f) && IsToHigh())
        {
            RaycastHit upperHit;
            if (!Physics.Raycast(playerData.UpperRay.position, transform.TransformDirection(Vector3.forward), out upperHit, 0.2f))
            {
                return true;
            }
        }

        RaycastHit rawHit45;
        if (Physics.Raycast(playerData.RawerRay.position, transform.TransformDirection(1.5f, 0, 1), out rawHit45, 0.1f) && IsToHigh())
        {
            RaycastHit upperHit45;
            if (!Physics.Raycast(playerData.UpperRay.position, transform.TransformDirection(1.5f, 0, 1), out upperHit45, 0.2f))
            {
                return true;
            }
        }

        RaycastHit rawHitmin45;
        if (Physics.Raycast(playerData.RawerRay.position, transform.TransformDirection(-1.5f, 0, 1), out rawHitmin45, 0.1f) && IsToHigh())
        {
            RaycastHit upperHitmin45;
            if (!Physics.Raycast(playerData.UpperRay.position, transform.TransformDirection(-1.5f, 0, 1), out upperHitmin45, 0.2f))
            {
                return true;
            }
        }

        return false;
    }

    //계단 오르내리기
    public void UpStair()
    {
        if(CheckStair() && !IsOnSloop())
        {
            playerRb.position -= new Vector3(0, -playerData.StepSmooth, 0);
        }
    }

    //경사로인지 구분법
    public bool IsOnSloop()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out sloopHit, playerData.MaxDistance, playerData.GroundLayerMask))
        {
            playerData.AngleResult = Vector3.Angle(Vector3.up, sloopHit.normal);
            return playerData.IsSloop = playerData.AngleResult != 0 && IsToHigh();
        }
        return playerData.IsSloop = false;
    }

    //경사로가 너무 높은지 판별
    public bool IsToHigh()
    {
        if (playerData.AngleResult < playerData.MaxAngle)
        {
            return true;
        }
        return false;
    }

    //경사로 일때 행동
    public void OnSloop()
    {
        if (playerData.IsSloop)
        {
            moveDir = Vector3.ProjectOnPlane(moveDir, sloopHit.normal);
            playerRb.useGravity = false;
            Debug.Log(playerData.IsSloop + " 경사면");
        }
        else if (!NextFrameIsSloop())
        {
            moveDir = Vector3.zero;
            playerRb.velocity = Vector3.zero;
        }
        else
        {
            playerRb.useGravity = true;
        }
    }

    //다음 캐릭터의 각도 계산
    public bool NextFrameIsSloop()
    {
        RaycastHit hit;
        var nextPlayerPos = transform.position + (moveDir * playerData.PlayerMoveSpeed * Time.fixedDeltaTime);
        if (Physics.Raycast(nextPlayerPos, Vector3.down, out hit, playerData.MaxDistance, playerData.GroundLayerMask))
        {
            float nextAngle = Vector3.Angle(Vector3.up, hit.normal);
            return nextAngle < playerData.MaxAngle;
        }
        return false;
    }

    //공격 참 거짓
    public void SetAttack(bool TorF)
    {
        playerData.IsAttack = TorF;
    }

    public void OnWeaponCollider()
    {
        weaPon.enabled = true;
    }

    public void OffWeaponCollider()
    {
        weaPon.enabled = false;
    }
}

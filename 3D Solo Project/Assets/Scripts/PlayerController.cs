using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PlayerController : MonoBehaviour, IIdle, IMove, ICheckIsGround, IFallen, ILanding, IJump, IUpDownStair, IAttack
{
    private PlayerData playerData;//플레이어 데이터
    private Rigidbody playerRb;//리지드 바디
    private Camera cam;//메인 카메라
    private IPlayerState currentState;//플레이어 상태
    private RaycastHit sloopHit;
    private Vector3 moveDir;//움직이는 방향
    private Vector3 featPos;//발 위치
    private Vector2 inputMoveDir;//인풋 변수 받아옴

    private void Awake()
    {
        playerData = GetComponent<PlayerData>();
        playerRb = GetComponent<Rigidbody>();
        currentState = new PlayerIdleState();
        currentState.Enter(this);
        cam = Camera.main;
        playerData.UpperRay.position = new Vector3(playerData.UpperRay.position.x, playerData.StepHight, playerData.UpperRay.position.y);
    }

    public PlayerData PlayerData { get => playerData; set => playerData = value; }
    public Vector3 InputMoveDir { get => inputMoveDir; set => inputMoveDir = value; }

    public void Updated()
    {
        if(currentState != null)
        {
            currentState.Update();
        }
    }

    //플레이어 상태 변화 시켜주는 코드
    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter(this);
    }

    //명령 내리기
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
    }

    //가만히 있는 상태
    public void Idle()
    {
        OnSloop();
        playerRb.velocity = Vector3.zero;
        playerData.Magnitude = playerRb.velocity.magnitude;
    }

    //움직임
    public void Move()
    {
        moveDir = cam.transform.forward * inputMoveDir.y + cam.transform.right * inputMoveDir.x;
        moveDir.y = 0;
        moveDir.Normalize();
        OnSloop();
        if (playerData.IsSprint)
        {
            playerRb.velocity = moveDir * playerData.PlayerSprintSpeed;
        }
        else
        {
            playerRb.velocity = moveDir * playerData.PlayerMoveSpeed;
        }
        playerData.Magnitude = playerRb.velocity.magnitude;
    }

    //플레이어 회전
    public void Rotation()
    {
        Vector3 targetdir = (cam.transform.forward * inputMoveDir.y) + (cam.transform.right * inputMoveDir.x);
        targetdir.Normalize();
        targetdir.y = 0;
        if(targetdir == Vector3.zero)
        {
            targetdir = transform.forward;
        }
        Quaternion roDir = Quaternion.LookRotation(targetdir);
        Quaternion playerRo = Quaternion.Lerp(transform.rotation, roDir, playerData.PlayerRotationSpeed * Time.deltaTime);
        transform.rotation = playerRo;
    }

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
        //bool isGround = Physics.SphereCast(originalPos, playerData.FallenSphereRadius, -Vector3.up, out hit, playerData.GroundLayerMask);
        
        return isGround;
    }

    private void OnDrawGizmos()
    {
        Vector3 originalPos = transform.position;
        originalPos.y += playerData.RayCastHightOffset;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(originalPos, playerData.FallenSphereRadius);
    }

    //플레이어 추락 및 랜딩
    public void PlayerFallenAndLanding()
    {
        Vector3 originalPos = transform.position;
        originalPos.y += playerData.RayCastHightOffset;
        RaycastHit hit;
        
        if(!playerData.IsGround)
        {
            playerData.InAirTime += Time.deltaTime;
            playerRb.AddForce(-Vector3.up * playerData.PlayerFallenSpeed * playerData.InAirTime);
        }

        if (Physics.SphereCast(originalPos, playerData.FallenSphereRadius, -Vector3.up, out hit, playerData.GroundLayerMask))
        {
            playerData.IsGround = true;
            playerData.InAirTime = 0;
        }
        else
        {
            playerData.IsGround = false;
        }
    }

    //플레이어 추락
    public void Fallen()
    {
        playerData.InAirTime += Time.deltaTime;
        playerRb.AddForce(-Vector3.up * playerData.PlayerFallenSpeed * playerData.InAirTime);
    }

    //플레이어 착지
    public void Landing()
    {
        playerData.IsGround = true;
        playerData.InAirTime = 0;
    }

    //플레이어 점프
    public void Jump()
    {
        playerRb.useGravity = true;
        float playerHight = Mathf.Sqrt(-2 * playerData.GravityForce * playerData.JumpPower);
        Vector3 player = playerRb.velocity;
        player.y = playerHight;
        playerRb.velocity = player;
        Debug.Log("점프함");
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
    public bool CheckUpDownStair()
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
    public void UpDownStair()
    {
        playerRb.position -= new Vector3(0, -playerData.StepSmooth, 0);
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
        if(playerData.AngleResult < playerData.MaxAngle)
        {
            return true;
        }
        return false;
    }

    //경사로 일때 행동
    public void OnSloop()
    {
        IsOnSloop();

        if (playerData.IsSloop)
        {
            moveDir = Vector3.ProjectOnPlane(moveDir, sloopHit.normal);
            playerRb.useGravity = false;
            Debug.Log(playerData.IsSloop + " 경사면");
        }
        else if(!NextFrameIsSloop())
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
        if(Physics.Raycast(nextPlayerPos, Vector3.down, out hit, playerData.MaxDistance, playerData.GroundLayerMask))
        {
            float nextAngle = Vector3.Angle(Vector3.up, hit.normal);
            return nextAngle < playerData.MaxAngle;
        }
        return false;
    }

    //공격 메서드
    public void Attack()
    {
        
    }

    //공격 참 거짓
    public void SetAttack(bool TorF)
    {
        playerData.IsAttack = TorF;
    }
}

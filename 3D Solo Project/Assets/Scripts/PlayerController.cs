using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PlayerController : MonoBehaviour, IMove, ICheckIsGround, IFallen, ILanding, IJump
{
    private PlayerData playerData;//플레이어 데이터
    private Rigidbody playerRb;//리지드 바디
    private Camera cam;//메인 카메라
    private IPlayerState currentState;//플레이어 상태
    private Vector3 moveDir;//움직이는 방향
    private Vector3 featPos;//발 위치
    private Vector2 inputMoveDir;//인풋 변수 받아옴
    private float _magnitude;//애니메이션 컨트롤에 줄 속력

    private void Awake()
    {
        playerData = GetComponent<PlayerData>();
        playerRb = GetComponent<Rigidbody>();
        currentState = new PlayerIdleState();
        currentState.Enter(this);
        cam = Camera.main;
    }

    public PlayerData PlayerData { get => playerData; set => playerData = value; }
    public Vector3 InputMoveDir { get => inputMoveDir; set => inputMoveDir = value; }
    public float Magnitude { get => _magnitude; set => _magnitude = value; }

    public void Updated()
    {
        //SetFeetPos();
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
        Debug.Log(CheckIsGround());
    }

    //움직임
    public void Move()
    {
        moveDir = cam.transform.forward * inputMoveDir.y + cam.transform.right * inputMoveDir.x;
        moveDir.y = 0;
        moveDir.Normalize();
        if(playerData.IsSprint)
        {
            playerRb.velocity = moveDir * playerData.PlayerSprintSpeed;
        }
        else
        {
            playerRb.velocity = moveDir * playerData.PlayerMoveSpeed;
        }
        Magnitude = playerRb.velocity.magnitude;
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
        RaycastHit hit;

        bool isGround = Physics.SphereCast(originalPos, playerData.FallenSphereRadius, -Vector3.up, out hit, playerData.GroundLayerMask);

        return isGround;
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

    //플레이어의 발 위치 설정
    public void SetFeetPos()
    {
        Vector3 originalPos = transform.position;
        originalPos.y += playerData.RayCastHightOffset;
        RaycastHit hit;
        featPos = transform.position;

        if(Physics.SphereCast(originalPos, playerData.FallenSphereRadius, -Vector3.up, out hit, playerData.GroundLayerMask))
        {
            featPos.y = hit.point.y;
        }

        if (playerData.IsGround && !playerData.IsJump)
        {
            if(Magnitude > 0)
            {
                transform.position = Vector3.Lerp(transform.position, featPos , Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = featPos;
            }
        }
    }

    //플레이어 점프
    public void Jump()
    {
        float playerHight = Mathf.Sqrt(-2 * playerData.GravityForce * playerData.JumpPower);
        Vector3 player = playerRb.velocity;
        player.y = playerHight;
        playerRb.velocity = player;
        Debug.Log("점프함");
    }

    public void SetJump(bool TorF)
    {
        playerData.IsJump = TorF;
    }

    public bool GetJump()
    {
        return playerData.IsJump;
    }
}

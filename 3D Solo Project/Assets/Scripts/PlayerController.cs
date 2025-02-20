using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PlayerController : MonoBehaviour, IMove, ICheckIsGround, IFallen, ILanding, IJump, IUpDownStair
{
    private PlayerData playerData;//�÷��̾� ������
    private Rigidbody playerRb;//������ �ٵ�
    private Camera cam;//���� ī�޶�
    private IPlayerState currentState;//�÷��̾� ����
    private Vector3 moveDir;//�����̴� ����
    private Vector3 featPos;//�� ��ġ
    private Vector2 inputMoveDir;//��ǲ ���� �޾ƿ�
    private float _magnitude;//�ִϸ��̼� ��Ʈ�ѿ� �� �ӷ�

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
    public float Magnitude { get => _magnitude; set => _magnitude = value; }

    public void Updated()
    {
        //SetFeetPos();
        if(currentState != null)
        {
            currentState.Update();
        }
    }

    //�÷��̾� ���� ��ȭ �����ִ� �ڵ�
    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter(this);
    }

    //��� ������
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        Debug.Log(CheckIsGround());
    }

    //������
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

    //�÷��̾� ȸ��
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

    //�÷��̾ �ٳ� �ȶٳ� ���� �Ǵ�
    public void SetSprint(bool TorF)
    {
        playerData.IsSprint = TorF;
    }

    //�ٴ��� ���� ����
    public bool GetSprint()
    {
        return playerData.IsSprint;
    }

    //�÷��̾ ���� �ִ��� ���� �Ǵ�
    public bool CheckIsGround()
    {
        Vector3 originalPos = transform.position;
        originalPos.y += playerData.RayCastHightOffset;

        var coll = Physics.OverlapSphere(originalPos, playerData.FallenSphereRadius, playerData.GroundLayerMask);
        bool isGround = coll.Length > 0;
        //bool isGround = Physics.SphereCast(originalPos, playerData.FallenSphereRadius, -Vector3.up, out hit, playerData.GroundLayerMask);
        
        return isGround;
    }

    //�÷��̾� �߶� �� ����
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

    //�÷��̾� �߶�
    public void Fallen()
    {
        playerData.InAirTime += Time.deltaTime;
        playerRb.AddForce(-Vector3.up * playerData.PlayerFallenSpeed * playerData.InAirTime);
    }

    //�÷��̾� ����
    public void Landing()
    {
        playerData.IsGround = true;
        playerData.InAirTime = 0;
    }

    //�÷��̾� ����
    public void Jump()
    {
        float playerHight = Mathf.Sqrt(-2 * playerData.GravityForce * playerData.JumpPower);
        Vector3 player = playerRb.velocity;
        player.y = playerHight;
        playerRb.velocity = player;
        Debug.Log("������");
    }

    //���� ������
    public void SetJump(bool TorF)
    {
        playerData.IsJump = TorF;
    }

    //���� ������ ��ȯ
    public bool GetJump()
    {
        return playerData.IsJump;
    }

    //��� ����������
    public void UpDownStair()
    {
        RaycastHit rawHit;
        if(Physics.Raycast(playerData.RawerRay.position , transform.TransformDirection(Vector3.forward), out rawHit, 0.1f))
        {
            RaycastHit upperHit;
            if (!Physics.Raycast(playerData.UpperRay.position, transform.TransformDirection(Vector3.forward), out upperHit, 0.2f))
            {
                playerRb.position -= new Vector3(0, -playerData.StepSmooth, 0);
                Debug.Log("��� �ö󰡴���");
            }
        }

        RaycastHit rawHit45;
        if (Physics.Raycast(playerData.RawerRay.position, transform.TransformDirection(1.5f, 0, 1), out rawHit45, 0.1f))
        {
            RaycastHit upperHit45;
            if (!Physics.Raycast(playerData.UpperRay.position, transform.TransformDirection(1.5f, 0, 1), out upperHit45, 0.2f))
            {
                playerRb.position -= new Vector3(0, -playerData.StepSmooth, 0);
                Debug.Log("��� �ö󰡴���");
            }
        }

        RaycastHit rawHitmin45;
        if (Physics.Raycast(playerData.RawerRay.position, transform.TransformDirection(-1.5f, 0, 1), out rawHitmin45, 0.1f))
        {
            RaycastHit upperHitmin45;
            if (!Physics.Raycast(playerData.UpperRay.position, transform.TransformDirection(-1.5f, 0, 1), out upperHitmin45, 0.2f))
            {
                playerRb.position -= new Vector3(0, -playerData.StepSmooth, 0);
                Debug.Log("��� �ö󰡴���");
            }
        }
    }
}

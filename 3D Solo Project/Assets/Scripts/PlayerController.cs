using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PlayerController : MonoBehaviour
{
    private PlayerData playerData;//�÷��̾� ������
    private Rigidbody playerRb;//������ �ٵ�
    private Camera cam;//���� ī�޶�
    private IPlayerState currentState;//�÷��̾� ����
    private RaycastHit sloopHit;
    private Vector3 moveDir;//�����̴� ����
    private Vector2 inputMoveDir;//��ǲ ���� �޾ƿ�

    private void Awake()
    {
        playerData = GetComponent<PlayerData>();
        playerRb = GetComponent<Rigidbody>();
        currentState = new PlayerIdleState();
        currentState.Enter(this);
        cam = Camera.main;
        moveDir = Vector3.zero;
        playerData.UpperRay.position = new Vector3(playerData.UpperRay.position.x, playerData.StepHight, playerData.UpperRay.position.y);
    }

    public PlayerData PlayerData { get => playerData; set => playerData = value; }
    public Rigidbody PlayerRb { get => playerRb; set => playerRb = value; }
    public Camera Cam { get => cam; set => cam = value; }
    public IPlayerState CurrentState { get => currentState; set => currentState = value; }
    public RaycastHit SloopHit { get => sloopHit; set => sloopHit = value; }
    public Vector3 MoveDir { get => moveDir; set => moveDir = value; }
    public Vector3 InputMoveDir { get => inputMoveDir; set => inputMoveDir = value; }

    public void FixedUpdated()
    {
        if(currentState != null)
        {
            currentState.Move();
            currentState.Rotation();
        }
    }

    public void Updated()
    {
        if (currentState != null)
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
        
        return isGround;
    }

    //�÷��̾� ����
    public void Landing()
    {
        playerData.IsGround = true;
        playerData.InAirTime = 0;
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

    //��� ���� �ƴ���
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

    //��� ����������
    public void UpStair()
    {
        playerRb.position -= new Vector3(0, -playerData.StepSmooth, 0);
    }

    //�������� ���й�
    public bool IsOnSloop()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out sloopHit, playerData.MaxDistance, playerData.GroundLayerMask))
        {
            playerData.AngleResult = Vector3.Angle(Vector3.up, sloopHit.normal);
            return playerData.IsSloop = playerData.AngleResult != 0 && IsToHigh();
        }
        return playerData.IsSloop = false;
    }

    //���ΰ� �ʹ� ������ �Ǻ�
    public bool IsToHigh()
    {
        if(playerData.AngleResult < playerData.MaxAngle)
        {
            return true;
        }
        return false;
    }

    //���� �϶� �ൿ
    public void OnSloop()
    {
        IsOnSloop();

        if (playerData.IsSloop)
        {
            moveDir = Vector3.ProjectOnPlane(moveDir, sloopHit.normal);
            playerRb.useGravity = false;
            Debug.Log(playerData.IsSloop + " ����");
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

    //���� ĳ������ ���� ���
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

    //���� �޼���
    public void Attack()
    {
        
    }

    //���� �� ����
    public void SetAttack(bool TorF)
    {
        playerData.IsAttack = TorF;
    }
}

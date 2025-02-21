using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPlayerState
{
    public abstract void Enter(PlayerController playerController);
    public abstract void Update();
    public abstract void Exit();
    public abstract void Move();
    public abstract void Rotation();
    public abstract void Attack();
}

//��� ����
public class PlayerIdleState : IPlayerState
{
    private PlayerController player;

    public override void Enter(PlayerController playerController)
    {
        player = playerController;
    }
    
    public override void Exit()
    {
        
    }

    public override void Move()
    {
        //����
        player.IsOnSloop();

        if (player.PlayerData.IsSloop)
        {
            player.MoveDir = Vector3.ProjectOnPlane(player.MoveDir, player.SloopHit.normal);
            player.PlayerRb.useGravity = false;
            Debug.Log(player.PlayerData.IsSloop + " ����");
        }
        else if (!player.NextFrameIsSloop())
        {
            player.MoveDir = Vector3.zero;
            player.PlayerRb.velocity = Vector3.zero;
        }
        else
        {
            player.PlayerRb.useGravity = true;
        }

        //�÷��̾� ������
        player.PlayerRb.velocity = Vector3.zero;
        player.PlayerData.Magnitude = player.PlayerRb.velocity.magnitude;
    }

    public override void Rotation()
    {
        Vector3 targetdir = (player.Cam.transform.forward * player.InputMoveDir.y) + (player.Cam.transform.right * player.InputMoveDir.x);
        targetdir.Normalize();
        targetdir.y = 0;
        if (targetdir == Vector3.zero)
        {
            targetdir = player.transform.forward;
        }
        Quaternion roDir = Quaternion.LookRotation(targetdir);
        Quaternion playerRo = Quaternion.Lerp(player.transform.rotation, roDir, player.PlayerData.PlayerRotationSpeed * Time.deltaTime);
        player.transform.rotation = playerRo;
    }

    public override void Attack()
    {
        player.Anime.PlayAttackAnime();
    }

    public override void Update()
    {
        if(player.CheckIsGround())
        {
            if(player.PlayerData.IsJump)
            {
                player.ChangeState(new PlayerJumpState());
                return;
            }
            else if(player.InputMoveDir.magnitude > 0.1f)
            {
                if(player.GetSprint())
                {
                    player.ChangeState(new PlayerSprintState());
                    return;
                }
                else
                {
                    player.ChangeState(new PlayerMoveState());
                    return;
                }
            }
        }
        else
        {
            player.ChangeState(new PlayerFallenState());
            return;
        }
    }
}

//�̵�����
public class PlayerMoveState : IPlayerState
{
    private PlayerController player;

    public override void Enter(PlayerController playerController)
    {
        player = playerController;
    }

    public override void Exit()
    {

    }

    public override void Move()
    {
        player.MoveDir = player.Cam.transform.forward * player.InputMoveDir.y + player.Cam.transform.right * player.InputMoveDir.x;
        player.MoveDir = new Vector3(player.MoveDir.x, 0, player.MoveDir.z);
        player.MoveDir.Normalize();

        //����
        player.IsOnSloop();

        if (player.PlayerData.IsSloop)
        {
            player.MoveDir = Vector3.ProjectOnPlane(player.MoveDir, player.SloopHit.normal);
            player.PlayerRb.useGravity = false;
            Debug.Log(player.PlayerData.IsSloop + " ����");
        }
        else if (!player.NextFrameIsSloop())
        {
            player.MoveDir = Vector3.zero;
            player.PlayerRb.velocity = Vector3.zero;
        }
        else
        {
            player.PlayerRb.useGravity = true;
        }

        //�÷��̾� ������
        player.PlayerRb.velocity = player.MoveDir * player.PlayerData.PlayerMoveSpeed;
        player.PlayerData.Magnitude = player.PlayerRb.velocity.magnitude;

        //���
        if (player.CheckStair() && !player.IsOnSloop())
        {
            player.PlayerRb.position -= new Vector3(0, -player.PlayerData.StepSmooth, 0);
        }
    }

    public override void Rotation()
    {
        Vector3 targetdir = (player.Cam.transform.forward * player.InputMoveDir.y) + (player.Cam.transform.right * player.InputMoveDir.x);
        targetdir.Normalize();
        targetdir.y = 0;
        if (targetdir == Vector3.zero)
        {
            targetdir = player.transform.forward;
        }
        Quaternion roDir = Quaternion.LookRotation(targetdir);
        Quaternion playerRo = Quaternion.Lerp(player.transform.rotation, roDir, player.PlayerData.PlayerRotationSpeed * Time.deltaTime);
        player.transform.rotation = playerRo;
    }

    public override void Attack()
    {
        player.Anime.PlayAttackAnime();
    }

    public override void Update()
    {
        if (player.CheckIsGround())
        {
            if (player.PlayerData.IsJump)
            {
                player.ChangeState(new PlayerJumpState());
                return;
            }
            else if (player.InputMoveDir.magnitude > 0.1f)
            {
                if (player.GetSprint())
                {
                    player.ChangeState(new PlayerSprintState());
                    return;
                }
            }
            else if(player.InputMoveDir.magnitude < 0.1f)
            {
                player.ChangeState(new PlayerIdleState());
                return;
            }
        }
        else
        {
            player.ChangeState(new PlayerFallenState());
        }
    }

}

//�޸��� ����
public class PlayerSprintState : IPlayerState
{
    private PlayerController player;

    public override void Enter(PlayerController playerController)
    {
        player = playerController;
    }

    public override void Exit()
    {

    }

    public override void Move()
    {
        player.MoveDir = player.Cam.transform.forward * player.InputMoveDir.y + player.Cam.transform.right * player.InputMoveDir.x;
        player.MoveDir = new Vector3(player.MoveDir.x, 0, player.MoveDir.z);
        player.MoveDir.Normalize();

        //����
        player.IsOnSloop();

        if (player.PlayerData.IsSloop)
        {
            player.MoveDir = Vector3.ProjectOnPlane(player.MoveDir, player.SloopHit.normal);
            player.PlayerRb.useGravity = false;
            Debug.Log(player.PlayerData.IsSloop + " ����");
        }
        else if (!player.NextFrameIsSloop())
        {
            player.MoveDir = Vector3.zero;
            player.PlayerRb.velocity = Vector3.zero;
        }
        else
        {
            player.PlayerRb.useGravity = true;
        }

        //������
        player.PlayerRb.velocity = player.MoveDir * player.PlayerData.PlayerSprintSpeed;
        player.PlayerData.Magnitude = player.PlayerRb.velocity.magnitude;

        //���
        if (player.CheckStair() && !player.IsOnSloop())
        {
            player.PlayerRb.position -= new Vector3(0, -player.PlayerData.StepSmooth, 0);
        }
    }

    public override void Rotation()
    {
        Vector3 targetdir = (player.Cam.transform.forward * player.InputMoveDir.y) + (player.Cam.transform.right * player.InputMoveDir.x);
        targetdir.Normalize();
        targetdir.y = 0;
        if (targetdir == Vector3.zero)
        {
            targetdir = player.transform.forward;
        }
        Quaternion roDir = Quaternion.LookRotation(targetdir);
        Quaternion playerRo = Quaternion.Lerp(player.transform.rotation, roDir, player.PlayerData.PlayerRotationSpeed * Time.deltaTime);
        player.transform.rotation = playerRo;
    }

    public override void Attack()
    {
        player.Anime.PlayAttackAnime();
    }

    public override void Update()
    {
        if (player.CheckIsGround())
        {
            if (player.PlayerData.IsJump)
            {
                player.ChangeState(new PlayerJumpState());
                return;
            }
            else if (player.InputMoveDir.magnitude > 0.1f)
            {
                if (!player.GetSprint())
                {
                    player.ChangeState(new PlayerMoveState());
                    return;
                }
            }
            else if (player.InputMoveDir.magnitude < 0.1f)
            {
                player.ChangeState(new PlayerIdleState());
                return;
            }
        }
        else
        {
            player.ChangeState(new PlayerFallenState());
        }
    }
}

//�������� ����
public class PlayerFallenState : IPlayerState
{
    private PlayerController player;
    public override void Enter(PlayerController playerController)
    {
        player = playerController;
        player.PlayerData.IsGround = false;
    }

    public override void Exit()
    {

    }

    public override void Move()
    {
        player.PlayerData.InAirTime += Time.deltaTime;
        player.PlayerRb.AddForce(-Vector3.up * player.PlayerData.PlayerFallenSpeed * player.PlayerData.InAirTime);
        Debug.Log("�߶���");
    }

    public override void Rotation()
    {
        
    }

    public override void Attack()
    {

    }

    public override void Update()
    {
        if(player.CheckIsGround())
        {
            player.ChangeState(new PlayerLandingState());
            return;
        }
    }
}

//���� ����
public class PlayerLandingState : IPlayerState
{
    private PlayerController player;
    public override void Enter(PlayerController playerController)
    {
        player = playerController;
        player.Landing();
    }

    public override void Exit()
    {

    }

    public override void Move()
    {
        
    }

    public override void Rotation()
    {
        
    }

    public override void Attack()
    {

    }

    public override void Update()
    {
        if (player.PlayerData.IsGround)
        {
            Debug.Log("����");
            player.ChangeState(new PlayerIdleState());
            return;
        }
    }
}

//���� ����
public class PlayerJumpState : IPlayerState
{
    PlayerController player;
    public override void Enter(PlayerController playerController)
    {
        player = playerController;

        player.PlayerRb.useGravity = true;
        float playerHight = Mathf.Sqrt(-2 * player.PlayerData.GravityForce * player.PlayerData.JumpPower);
        Vector3 playerVel = player.PlayerRb.velocity;
        playerVel = new Vector3(playerVel.x, playerHight, playerVel.z);
        player.PlayerRb.velocity = playerVel;
    }

    public override void Exit()
    {
        
    }

    public override void Move()
    {
        
    }

    public override void Rotation()
    {
        
    }

    public override void Attack()
    {

    }

    public override void Update()
    {
        if (player.CheckIsGround())
        {
            player.ChangeState(new PlayerLandingState());
            return;
        }
    }
}

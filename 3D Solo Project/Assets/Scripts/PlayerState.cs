using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPlayerState
{
    public abstract void Enter(PlayerController playerController);
    public abstract void Update();
    public abstract void Exit();
}

//대기 상태
public class PlayerIdleState : IPlayerState
{
    private PlayerController player;
    public override void Enter(PlayerController playerController)
    {
        player = playerController;
        Debug.Log("아이들");
    }
    
    public override void Exit()
    {
        
    }

    public override void Update()
    {
        if(player.CheckIsGround())
        {
            if(player.PlayerData.IsJump)
            {
                player.ChangeState(new PlayerJumpState());
            }
            else if (player.InputMoveDir != Vector3.zero)
            {
                player.ChangeState(new PlayerMoveState());
            }
            else if (player.InputMoveDir != Vector3.zero && player.GetSprint())
            {
                player.ChangeState(new PlayerSprintState());
            }
        }
        else
        {
            player.ChangeState(new PlayerFallenState());
        }
    }
}

//이동상태
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

    public override void Update()
    {
        if (player.CheckIsGround())
        {
            if (player.PlayerData.IsJump)
            {
                player.ChangeState(new PlayerJumpState());
            }
            else if (player.InputMoveDir != Vector3.zero && !player.GetSprint())
            {
                player.ExecuteCommand(new MoveCommand(player));
                player.ExecuteCommand(new UpDownStair(player));
                Debug.Log("걷는중");
            }
            else if (player.InputMoveDir != Vector3.zero && player.GetSprint())
            {
                player.ChangeState(new PlayerSprintState());
            }
            else
            {
                player.ChangeState(new PlayerIdleState());
            }
        }
        else
        {
            player.ChangeState(new PlayerFallenState());
        }
    }

}

//뛰는 상태
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

    public override void Update()
    {
        if (player.CheckIsGround())
        {
            if (player.PlayerData.IsJump)
            {
                player.ChangeState(new PlayerJumpState());
            }
            else if (player.InputMoveDir != Vector3.zero && !player.GetSprint())
            {
                player.ChangeState(new PlayerMoveState());
            }
            else if (player.InputMoveDir != Vector3.zero && player.GetSprint())
            {
                player.ExecuteCommand(new MoveCommand(player));
                player.ExecuteCommand(new UpDownStair(player));
                Debug.Log("뛰는중");
            }
            else
            {
                player.ChangeState(new PlayerIdleState());
            }
        }
        else
        {
            player.ChangeState(new PlayerFallenState());
        }
    }
}

//떨어지는 상태
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
    public override void Update()
    {
        if(!player.PlayerData.IsGround)
        {
            player.ExecuteCommand(new FallenCommand(player));
            Debug.Log("추락중");
        }

        if(player.CheckIsGround())
        {
            player.ChangeState(new PlayerLandingState());
        }
    }
}

//착지 상태
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

    public override void Update()
    {
        if (player.PlayerData.IsGround)
        {
            Debug.Log("랜딩");
            player.ChangeState(new PlayerIdleState());
        }
    }
}

//점프 상태
public class PlayerJumpState : IPlayerState
{
    PlayerController player;
    public override void Enter(PlayerController playerController)
    {
        player = playerController;
        player.ExecuteCommand(new JumpCommand(player));
    }

    public override void Exit()
    {
        Debug.Log("점프끝");
    }

    public override void Update()
    {
        if (player.CheckIsGround())
        {
            player.ChangeState(new PlayerLandingState());
        }
    }
}

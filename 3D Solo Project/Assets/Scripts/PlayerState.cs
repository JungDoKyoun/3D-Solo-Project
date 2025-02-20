using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPlayerState
{
    public abstract void Enter(PlayerController playerController);
    public abstract void Update();
    public abstract void Exit();
}

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
            if (player.InputMoveDir != Vector3.zero)
            {
                player.ChangeState(new PlayerMoveState());
            }
            else if (player.InputMoveDir != Vector3.zero && player.GetSprint())
            {
                player.ChangeState(new PlayerSprintState());
            }
            else if(player.PlayerData.IsJump)
            {
                player.ChangeState(new PlayerJumpState());
            }
        }
        else
        {
            player.ChangeState(new PlayerFallenState());
        }
    }
}

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
            if (player.InputMoveDir != Vector3.zero && !player.GetSprint())
            {
                player.ExecuteCommand(new MoveCommand(player));
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
            if (player.InputMoveDir != Vector3.zero && !player.GetSprint())
            {
                player.ChangeState(new PlayerMoveState());
            }
            else if (player.InputMoveDir != Vector3.zero && player.GetSprint())
            {
                player.ExecuteCommand(new MoveCommand(player));
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

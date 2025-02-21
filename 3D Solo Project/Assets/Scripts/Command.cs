using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public interface ICommand
{
    public void Execute();
}

public class IdleCommand : ICommand
{
    IIdle entity;

    public IdleCommand(IIdle idle)
    {
        entity = idle;
    }

    public void Execute()
    {
        entity.Idle();
    }
}

//플레이어 움직임 명령
public class MoveCommand : ICommand
{
    IMove entity;

    public MoveCommand(IMove entityInterface)
    {
        entity = entityInterface;
    }

    public void Execute()
    {
        entity.Move();
        entity.Rotation();
    }
}

public class FallenCommand : ICommand
{
    IFallen entity;

    public FallenCommand(IFallen fallen)
    {
        entity = fallen;
    }

    public void Execute()
    {
        entity.Fallen();
    }
}

public class JumpCommand : ICommand
{
    IJump entity;

    public JumpCommand(IJump jump)
    {
        entity = jump;
    }

    public void Execute()
    {
        entity.Jump();
    }
}

public class UpDownStair : ICommand
{
    IUpDownStair entity;

    public UpDownStair(IUpDownStair upDownStair)
    {
        entity = upDownStair;
    }

    public void Execute()
    {
        entity.UpDownStair();
    }
}

public class Attack : ICommand
{
    IAttack entity;

    public Attack(IAttack attack)
    {
        entity = attack;
    }

    public void Execute()
    {
        entity.Attack();
    }
}


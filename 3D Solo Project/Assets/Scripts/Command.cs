using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public interface ICommand
{
    public void Execute();
}

//�÷��̾� ������ ���
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

//�������
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

//���� ���
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


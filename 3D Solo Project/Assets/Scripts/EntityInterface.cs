using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIdle
{
    void Idle();
}

public interface IMove
{
    void Move();
    void Rotation();
}

public interface ICheckIsGround
{
    bool CheckIsGround();
}

public interface IFallenAndLanding
{
    bool CheckIsGround();
    void Fallen();
    void Landing();
}

public interface IJump
{
    void Jump();
}

public interface IUpStair
{
    bool CheckStair();
    void UpStair();
}

public interface ISloop
{
    bool IsOnSloop();
    bool IsToHigh();
    bool NextFrameIsSloop();
    void OnSloop();
}

public interface IAttack
{
    void Attack();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    void Move();
    void Rotation();
}

public interface ICheckIsGround
{
    bool CheckIsGround();
}

public interface IFallen
{
    void Fallen();
}

public interface ILanding
{
    void Landing();
}

public interface IJump
{
    void Jump();
}

public interface IUpDownStair
{
    void UpDownStair();
}

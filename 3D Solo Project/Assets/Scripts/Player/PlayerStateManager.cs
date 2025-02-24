using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private IPlayerState currentState;//�÷��̾� ����
    PlayerController player;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        ChangeState(new PlayerIdleState());
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.Move();
            currentState.Rotation();
        }
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
            currentState.Attack();
        }
    }

    //�÷��̾� ���� ��ȭ �����ִ� �ڵ�
    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter(player, this);
    }
}

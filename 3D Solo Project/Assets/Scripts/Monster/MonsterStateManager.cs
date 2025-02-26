using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateManager : MonoBehaviour
{
    private IMonsterState currentState;
    private MonsterController monsterController;

    private void Awake()
    {
        monsterController = GetComponent<MonsterController>();
        ChangeMonsterState(new MonsterIdle());
    }

    private void FixedUpdate()
    {
        currentState.Move();
        currentState.Attack();
    }

    private void Update()
    {
        currentState.Update();
    }

    public void ChangeMonsterState(IMonsterState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(monsterController, this);
    }
}

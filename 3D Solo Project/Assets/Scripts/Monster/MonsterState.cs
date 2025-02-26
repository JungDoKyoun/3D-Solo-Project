using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMonsterState
{
    public abstract void Enter(MonsterController monsterController, MonsterStateManager monsterStateManager);
    public abstract void Update();
    public abstract void Exit();
    public abstract void Move();
    public abstract void Detect();
    public abstract void Attack();
}

public class Idle : IMonsterState
{
    MonsterController monster;
    MonsterStateManager stateManager;

    public override void Enter(MonsterController monsterController, MonsterStateManager monsterStateManager)
    {
        monster = monsterController;
        stateManager = monsterStateManager;
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }

    public override void Move()
    {
        
    }

    public override void Detect()
    {
        
    }

    public override void Attack()
    {
        
    }
}

public class Patrol : IMonsterState
{
    MonsterController monster;
    MonsterStateManager stateManager;

    public override void Enter(MonsterController monsterController, MonsterStateManager monsterStateManager)
    {
        monster = monsterController;
        stateManager = monsterStateManager;
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }

    public override void Move()
    {

    }

    public override void Detect()
    {

    }

    public override void Attack()
    {

    }
}

public class Chase : IMonsterState
{
    MonsterController monster;
    MonsterStateManager stateManager;

    public override void Enter(MonsterController monsterController, MonsterStateManager monsterStateManager)
    {
        monster = monsterController;
        stateManager = monsterStateManager;
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }

    public override void Move()
    {

    }

    public override void Detect()
    {

    }

    public override void Attack()
    {

    }
}

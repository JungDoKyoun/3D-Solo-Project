using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMonsterState
{
    public abstract void Enter(MonsterController monsterController, MonsterStateManager monsterStateManager);
    public abstract void Update();
    public abstract void Exit();
    public abstract void Move();
    public abstract void Attack();
}

public class MonsterIdle : IMonsterState
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
        if(monster.CheckIsGround())
        {
            if (monster.DetectPlayer() != null)
            {
                stateManager.ChangeMonsterState(new MonsterChase());
            }
        }
        else
        {
            stateManager.ChangeMonsterState(new MonsterFalling());
        }
    }

    public override void Move()
    {
        
    }

    public override void Attack()
    {
        
    }
}

public class MonsterPatrol : IMonsterState
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
        if (monster.CheckIsGround())
        {
            if (monster.DetectPlayer() != null)
            {
                stateManager.ChangeMonsterState(new MonsterChase());
            }
        }
        else
        {
            stateManager.ChangeMonsterState(new MonsterFalling());
        }
    }

    public override void Move()
    {

    }

    public override void Attack()
    {

    }
}

public class MonsterChase : IMonsterState
{
    MonsterController monster;
    MonsterStateManager stateManager;
    Collider target;

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
        target = monster.DetectPlayer();
        if (monster.CheckIsGround())
        {
            if (monster.DetectPlayer() == null)
            {
                stateManager.ChangeMonsterState(new MonsterIdle());
            }
        }
        else
        {
            stateManager.ChangeMonsterState(new MonsterFalling());
        }
    }

    public override void Move()
    {
        monster.ChasePlayer(target);
    }

    public override void Attack()
    {

    }
}

public class MonsterFalling : IMonsterState
{
    MonsterController monster;
    MonsterStateManager stateManager;
    Collider target;

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
        if (monster.CheckIsGround())
        {
            stateManager.ChangeMonsterState(new MonsterIdle());
        }
    }

    public override void Move()
    {

    }

    public override void Attack()
    {

    }
}

public class MonsterLanding : IMonsterState
{
    MonsterController monster;
    MonsterStateManager stateManager;
    Collider target;

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

    public override void Attack()
    {

    }
}

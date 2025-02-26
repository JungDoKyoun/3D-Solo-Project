using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    Collider target;
    float distance;
    float updateTime;
    float timeSinceLastUpdate;

    public override void Enter(MonsterController monsterController, MonsterStateManager monsterStateManager)
    {
        monster = monsterController;
        stateManager = monsterStateManager;
        distance = 100;
        updateTime = 5;
        timeSinceLastUpdate = 0;
        Debug.Log("대기중");
    }

    public override void Exit()
    {
        monster.StopAllCoroutines();
    }

    public override void Update()
    {
        target = monster.DetectPlayer();
        if (target != null)
        {
            distance = monster.CheckDistance(target);
            if (distance > 1.5 && !monster.IsAttack)
            {
                stateManager.ChangeMonsterState(new MonsterChase());
            }
        }
        else
        {
            timeSinceLastUpdate += Time.deltaTime;
            if(timeSinceLastUpdate >= updateTime)
            {
                stateManager.ChangeMonsterState(new MonsterPatrol());
            }
        }
    }

    public override void Move()
    {
        
    }

    public override void Attack()
    {
        if(distance <= 3 && !monster.IsAttack)
        {
            monster.StartCoroutine(monster.Attack());
        }
    }
}

public class MonsterPatrol : IMonsterState
{
    MonsterController monster;
    MonsterStateManager stateManager;
    Collider target;
    float distance;

    public override void Enter(MonsterController monsterController, MonsterStateManager monsterStateManager)
    {
        monster = monsterController;
        stateManager = monsterStateManager;
        monster.Anime.PlayPatrolAnime(true);
        monster.StartCoroutine(monster.Patrol());
        Debug.Log("asd");
    }

    public override void Exit()
    {
        monster.Anime.PlayPatrolAnime(false);
        monster.StopAllCoroutines();
        Debug.Log("순찰끝");
    }

    public override void Update()
    {
        target = monster.DetectPlayer();
        if (target != null)
        {
            distance = monster.CheckDistance(target);
            if (distance > 1.5)
            {
                stateManager.ChangeMonsterState(new MonsterChase());
            }
        }
        else if (monster.Agent.remainingDistance <= monster.Agent.stoppingDistance && !monster.Agent.pathPending)
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

public class MonsterChase : IMonsterState
{
    MonsterController monster;
    MonsterStateManager stateManager;
    Collider target;
    float distance;

    public override void Enter(MonsterController monsterController, MonsterStateManager monsterStateManager)
    {
        monster = monsterController;
        stateManager = monsterStateManager;
        monster.Anime.PlayChaseAnime(true);
        distance = 100;
    }

    public override void Exit()
    {
        monster.StopAllCoroutines();
    }

    public override void Update()
    {
        target = monster.DetectPlayer();
        if(target != null)
        {
            distance = monster.CheckDistance(target);
        }
        
        if (target == null || distance <= 1.5f)
        {
            monster.StopChase();
            monster.Anime.PlayChaseAnime(false);
            if(!monster.IsAttack)
            {
                stateManager.ChangeMonsterState(new MonsterIdle());
            }
        }
    }

    public override void Move()
    {
        monster.ChasePlayer(target);
    }

    public override void Attack()
    {
        if (distance <= 3 && !monster.IsAttack)
        {
            monster.StartCoroutine(monster.Attack());
        }
    }
}

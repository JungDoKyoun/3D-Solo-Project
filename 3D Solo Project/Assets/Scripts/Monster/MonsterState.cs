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
        updateTime = 5;
        distance = 100;
        timeSinceLastUpdate = 0;
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
        }
        if(monster.IsDie)
        {
            stateManager.ChangeMonsterState(new MonsterDieState());
        }
        else if(distance <= 1.5 && target != null)
        {
            stateManager.ChangeMonsterState(new MonsterAttack());
        }
        else if (distance > 1.5 && !monster.IsAttack && target != null)
        {
            stateManager.ChangeMonsterState(new MonsterChase());
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
    }

    public override void Exit()
    {
        monster.Anime.PlayPatrolAnime(false);
        monster.StopAllCoroutines();
    }

    public override void Update()
    {
        target = monster.DetectPlayer();
        if (target != null)
        {
            distance = monster.CheckDistance(target);
        }
        if (monster.IsDie)
        {
            stateManager.ChangeMonsterState(new MonsterDieState());
        }
        else if (distance <= 1.5 && target != null)
        {
            stateManager.ChangeMonsterState(new MonsterAttack());
        }
        else if (distance > 1.5 && target != null)
        {
            stateManager.ChangeMonsterState(new MonsterChase());
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
        Debug.Log("추적중");
    }

    public override void Exit()
    {
        monster.Anime.PlayChaseAnime(false);
    }

    public override void Update()
    {
        target = monster.DetectPlayer();
        if (target != null)
        {
            distance = monster.CheckDistance(target);
        }
        if (monster.IsDie)
        {
            stateManager.ChangeMonsterState(new MonsterDieState());
        }
        else if (distance <= 1.5 && target != null)
        {
            stateManager.ChangeMonsterState(new MonsterAttack());
        }
        else if (target == null || distance <= 1.5f)
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
        
    }
}

public class MonsterAttack : IMonsterState
{
    MonsterController monster;
    MonsterStateManager stateManager;
    Collider target;
    float distance;

    public override void Enter(MonsterController monsterController, MonsterStateManager monsterStateManager)
    {
        Debug.Log("공격함");
        monster = monsterController;
        stateManager = monsterStateManager;
    }

    public override void Exit()
    {
        monster.StopCoroutine(monster.Attack(target.transform));
    }

    public override void Update()
    {
        target = monster.DetectPlayer();
        if (target != null)
        {
            distance = monster.CheckDistance(target);
        }
        if (monster.IsDie)
        {
            stateManager.ChangeMonsterState(new MonsterDieState());
        }
        if (distance > 1.5 && target != null)
        {
            stateManager.ChangeMonsterState(new MonsterChase());
        }
        else if (target == null)
        {
            stateManager.ChangeMonsterState(new MonsterIdle());
        }
    }

    public override void Move()
    {

    }

    public override void Attack()
    {
        if (target != null && !monster.IsAttack)
        {
            monster.StartCoroutine(monster.Attack(target.transform));
        }
    }
}

public class MonsterDieState : IMonsterState
{
    private MonsterController monster;
    private MonsterStateManager stateManager;

    public override void Enter(MonsterController monsterController, MonsterStateManager monsterStateManager)
    {
        monster = monsterController;
        stateManager = monsterStateManager;

        monster.Anime.PlayDieAnime(true);

        monster.MonsterRb.isKinematic = true;
        monster.Agent.enabled = false;
        monster.IsAttack = false;
        monster.IsChase = false;

        if (monster.GetComponent<Collider>())
        {
            monster.GetComponent<Collider>().enabled = false;
        }

        monster.StartCoroutine(monster.RespawnMonster());
    }


    public override void Exit()
    {
        monster.Anime.PlayDieAnime(false);
    }

    public override void Move()
    {
        
    }
    public override void Attack()
    {
        
    }

    public override void Update()
    {
        if (!monster.IsDie)
        {
            stateManager.ChangeMonsterState(new MonsterIdle());
        }
    }
}

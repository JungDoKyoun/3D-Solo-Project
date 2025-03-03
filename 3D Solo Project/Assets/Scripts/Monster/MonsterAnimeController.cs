using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimeController : MonoBehaviour
{
    Animator anime;
    MonsterController monster;
    int chase;
    int patrol;
    int attack;
    int die;

    private void Awake()
    {
        anime = GetComponent<Animator>();
        monster = GetComponent<MonsterController>();
        chase = Animator.StringToHash("IsChase");
        patrol = Animator.StringToHash("IsPatrol");
        attack = Animator.StringToHash("IsAttack");
        die = Animator.StringToHash("IsDie");
    }

    public void PlayChaseAnime(bool TorF)
    {
        anime.SetBool(chase, TorF);
    }

    public void PlayPatrolAnime(bool TorF)
    {
        anime.SetBool(patrol, TorF);
    }

    public void PlayAttackAnime(bool TorF)
    {
        anime.SetBool(attack, TorF);
    }

    public void PlayDieAnime(bool TorF)
    {
        anime.SetBool(die, TorF);
    }
}

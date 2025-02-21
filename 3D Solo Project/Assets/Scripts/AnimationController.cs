using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator anime;
    PlayerController player;
    int _speed;
    int _attack;
    float playerSpeed;

    private void Awake()
    {
        anime = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
        _speed = Animator.StringToHash("Speed");
        _attack = Animator.StringToHash("SwordAttack");
    }
    
    public void PlayAllAnime()
    {
        PlayerMoveAnime();
    }

    private void PlayerMoveAnime()
    {
        playerSpeed = player.PlayerData.Magnitude;
        float speed;
        Debug.Log(player.PlayerData.Magnitude);
        if (playerSpeed > 0 && playerSpeed < 3.5f)
        {
            speed = 0.5f;
        }
        else if (playerSpeed >= 3.5)
        {
            speed = 1;
        }
        else
        {
            speed = 0;
        }
        anime.SetFloat(_speed, speed, 0.1f, Time.deltaTime);
    }

    public void PlayAttackAnime()
    {
        if(player.PlayerData.IsAttack)
        {
            anime.SetBool(_attack, true);
            player.PlayerData.IsAttack = true;
            StartCoroutine(player.ResetAttack());
        }
    }

    public void ResetAttackAnime()
    {
        anime.SetBool(_attack, false);
    }
}

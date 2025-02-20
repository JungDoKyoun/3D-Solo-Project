using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator anime;
    PlayerController player;
    int _speed;
    float playerSpeed;

    private void Awake()
    {
        anime = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
        _speed = Animator.StringToHash("Speed");
    }
    public void PlayIdleAnime()
    {
        playerSpeed = player.Magnitude;
        if(playerSpeed == 0)
        {
            anime.SetFloat(_speed, 0);
        }
    }

    public void PlayMoveAnime()
    {
        playerSpeed = player.Magnitude;
        if(playerSpeed > 0 && playerSpeed < 3.5f)
        {
            anime.SetFloat(_speed, 0.5f);
        }
    }

    public void PlayRunAnime()
    {
        playerSpeed = player.Magnitude;
        if(playerSpeed >= 3.5)
        {
            anime.SetFloat(_speed, 1);
        }
    }
}

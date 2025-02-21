using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerController playerController;
    private AnimationController anime;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        anime = GetComponent<AnimationController>();
    }

    private void Update()
    {
        playerController.Updated();
        anime.PlayAllAnime();
    }

    private void FixedUpdate()
    {
        playerController.FixedUpdated();
    }
}

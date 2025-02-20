using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();   
    }

    private void FixedUpdate()
    {
        playerController.Updated();
    }

    private void Update()
    {
        playerController.SetFeetPos();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    private ItemDataSO itemData;
    private PlayerInven player;
    private SphereCollider collider;
    private bool isPlayerNearby;
    private bool isDroop;

    private void Awake()
    {
        player = FindObjectOfType<PlayerInven>();
        isPlayerNearby = false;
        isDroop = false;
    }   


    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            PickUpItem();
        }
    }

    public bool IsDroop { get => isDroop; set => isDroop = value; }

    public void SetItem(ItemDataSO data)
    {
        itemData = data;
    }
    public void PickUpItem()
    {
        if (player != null)
        {
            collider = GetComponent<SphereCollider>();
            player.AddItemInven(itemData.CreateItem());
            isDroop = false;
            collider.enabled = false;
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("µé¾î°¨");
            isPlayerNearby = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}

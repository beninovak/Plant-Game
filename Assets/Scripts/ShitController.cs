using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShitController : MonoBehaviour {
    
    [HideInInspector]
    public GameObject shit, player;
    
    [HideInInspector]
    public PlayerController playerController;

    private SpriteRenderer spriteRenderer;
    private Dictionary<string, PlayerController.InventoryItem> playerInventory;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown() {
        playerInventory = playerController.inventory;
        
        float distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (distanceFromPlayer <= 2f) { // Magic number
            if (!playerInventory.ContainsKey("shit")) {
                playerInventory.Add("shit", new PlayerController.InventoryItem() { count = 1, item = shit } );
            } else {
                var shitItem = playerInventory["shit"];
                shitItem.count++;
                playerInventory["shit"] = shitItem;
            }
            Destroy(gameObject);
        }
    }

    private void OnMouseOver() {
        spriteRenderer.color = new Color(0.576f, 0.262f, 0.098f, 1f);
    }
    
    private void OnMouseExit() {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}

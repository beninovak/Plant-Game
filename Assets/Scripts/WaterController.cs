using System;
using UnityEngine;

public class WaterController : MonoBehaviour {
    
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public PlayerController playerController;
    
    private void OnMouseDown() {
        if (playerController.activeItem && playerController.activeItem.GetComponent<ItemController>().name == "kangla") {
            float distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);
            if (distanceFromPlayer <= 6f) { // Magic number
                playerController.activeItem.GetComponent<KanglaController>().MakeKanglaFull();
                playerController.ActiveItemInventoryUpdate();
                var item = playerController.inventory["kangla"];
                item.count = 5;
                playerController.inventory["kangla"] = item;
                playerController.itemCounter.text = "5";
            }
        }
    }
}

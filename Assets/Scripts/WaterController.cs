using System;
using UnityEngine;

public class WaterController : MonoBehaviour {
    
    [HideInInspector]
    public GameObject player;
    public PlayerController playerController;
    
    private void OnMouseDown() {
        if (playerController.activeItem == "Kangla") {

            float distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);
            if (distanceFromPlayer <= 6f) { // Magic number
                playerController.kanglaController.makeKanglaFull();
            }
        }
    }
}

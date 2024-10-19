using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PigController : MonoBehaviour {
    [HideInInspector]
    public GameObject pumpkinToAttack;
    [HideInInspector]
    public GameObject player;
    public PlayerController playerController;

    private Vector2 spawnPoint;
    private bool hasEatenPumpkin = false, wasFlipped = false, isDead = false;
    private float speed = 5f, distanceToMove = 0f, distanceFromPumpkin = 0f, distanceFromSpawnPoint = 0f;
    
    private SpriteRenderer spriteRenderer;
    
    public Texture2D skullCursor;
    
    private void Awake() {
        var position = transform.position;
        spawnPoint = new Vector2(position.x, position.y);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        if (pumpkinToAttack.transform.position.x > transform.position.x) {
            spriteRenderer.flipX = true;
            wasFlipped = true;
        }
        Vector2.MoveTowards(transform.position, pumpkinToAttack.transform.position, 10f);
    }

    private void FixedUpdate() {
        if (pumpkinToAttack && !isDead) {
            
            distanceToMove = speed * Time.deltaTime;
            
            if (!hasEatenPumpkin) {
                var position = transform.position;
                var positionPumpkin = pumpkinToAttack.transform.position;
                transform.position = Vector2.MoveTowards(position, positionPumpkin, distanceToMove);
                distanceFromPumpkin = Vector2.Distance(position, positionPumpkin);
                
                if (distanceFromPumpkin <= 1f) {
                    pumpkinToAttack.GetComponent<PlantController>().KillSelf(PlantController.DEATH_REASON.EATEN_BY_WILD_SWINE);
                    speed = 10f;
                    hasEatenPumpkin = true;
    
                    GetComponent<SpriteRenderer>().flipX = !wasFlipped;
                }
            } else {
                transform.position = Vector2.MoveTowards(transform.position, spawnPoint, distanceToMove);
                distanceFromSpawnPoint = Vector2.Distance(transform.position, spawnPoint);

                if (distanceFromSpawnPoint <= 0f) {
                    Destroy(gameObject);
                }
            }
        }
    }
    
    
    private void OnMouseDown() {
        if (!playerController.activeItem) return;

        if (playerController.activeItemName == "hoe") {
            float distanceToPlayer = Vector2.Distance(transform.position, playerController.gameObject.transform.position);
            if (distanceToPlayer > 6f) return;

            var temp = playerController.inventory["hoe"];
            temp.count--;
            playerController.inventory["hoe"] = temp;
            playerController.itemCounter.text = temp.count.ToString();
            
            isDead = true;
            spriteRenderer.flipY = true;
            GetComponent<Animator>().SetBool("isDead", true);
            Destroy(gameObject, 2.3f); // Magic number
        }
    }

    private void OnMouseEnter() {
        if (playerController.activeItemName == "hoe") {
            Cursor.SetCursor(skullCursor, Vector2.zero, CursorMode.Auto);
        }
    }
    
    private void OnMouseExit() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}

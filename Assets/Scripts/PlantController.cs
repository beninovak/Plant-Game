using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantController : MonoBehaviour {

    public Texture2D kanglaCursor, shitCursor;
    public Sprite seed, spriteDead, spriteAlive;
    public SpriteRenderer waterSpriteRenderer;

    [HideInInspector] 
    public GameObject player;
    [HideInInspector]
    public PlayerController playerController;

    private enum STATE {
        SEED,
        DEAD,
        ALIVE
    };

    private enum DEATH_REASON {
        UNHARVESTED,
        UNFERTILIZED,
        TOO_MUCH_WATER,
        NOT_ENOUGH_WATER,
    };

    private bool isWatered = false;
    private float timeSinceWatered = 0.0f;
    private STATE currentState = STATE.SEED;
    private bool isFertilized = false;
    private int timesFertilized = 0;
    private float timeSinceFertilized = 0.0f;
    private float timeSinceCropAvailable = 0f;
    private SpriteRenderer spriteRenderer;
    
    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        
        if (currentState == STATE.SEED) {
            if (isWatered && timeSinceWatered >= 10.0f) { // TODO - change to 60.0f
                waterSpriteRenderer.color = new Color(0.043f, 0.435f, 0.780f, 0.7f);
                if (timeSinceWatered > 20.0f) {
                    waterSpriteRenderer.color = new Color(0.043f, 0.435f, 0.780f, 0.3f);

                    if (timeSinceWatered > 30.0f) {
                        KillSelf(DEATH_REASON.NOT_ENOUGH_WATER);
                    }
                }
            }
            
            if (isFertilized) { // TODO - change to 30.0f idk
                if (timeSinceFertilized > 30.0f) {
                    isFertilized = false;
                    timesFertilized = 0;
                }

                switch (timesFertilized) {
                    case 0:
                        KillSelf(DEATH_REASON.UNFERTILIZED);
                        break;
                    case >= 3:
                        if (!isWatered) break;
                        currentState = STATE.ALIVE;
                        spriteRenderer.sprite = spriteAlive;
                        waterSpriteRenderer.color = new Color(0f, 0f, 0f, 0f);
                        break;
                }
            }
        }
        
        if (currentState == STATE.ALIVE) {
            timeSinceCropAvailable += Time.deltaTime;
            if (timeSinceCropAvailable > 20.0f) {
                KillSelf(DEATH_REASON.UNHARVESTED);
            }
        }
        
        timeSinceWatered += Time.deltaTime;
        timeSinceFertilized += Time.deltaTime;
    }
    
    private void HarvestSelf() {
        timeSinceWatered = 0f;
        timesFertilized = 0;
        timeSinceFertilized = 0f;
        currentState = STATE.SEED;
        
        // TODO - Give player pumpkin + drop seed
    }

    private void KillSelf(DEATH_REASON deathReason) {
        isWatered = false;
        timeSinceWatered = 0f;
        timesFertilized = 0;
        timeSinceFertilized = 0f;
        currentState = STATE.DEAD;
        spriteRenderer.sprite = spriteDead;

        waterSpriteRenderer.color = new Color(0f, 0f, 0f, 0f);
        Debug.Log($"Plant {gameObject.name} DIED CUZ {deathReason}");
        // TODO - Write deathReason above pumpkin
    }
    
    private void OnMouseDown() {
        if (!playerController.activeItem) return;
        
        float distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (distanceFromPlayer > 2f) return;
        
        string usedItem = playerController.activeItemName;
        //Debug.Log($"CLICKING PLANT WITH {usedItem}");

        switch (usedItem) {
            case "kangla":
                KanglaController controller = playerController.activeItem.GetComponent<KanglaController>();
                if (!controller.IsUsable()) break;
                
                if (isWatered && timeSinceWatered < 10.0f) {
                    KillSelf(DEATH_REASON.TOO_MUCH_WATER);
                    break;
                }
                
                isWatered = true;
                timeSinceWatered = 0f;
                waterSpriteRenderer.color = new Color(0.043f, 0.435f, 0.780f, 1f);
                controller.Use();
                break;
            
            case "shit":
                isFertilized = true;
                timesFertilized++;
                timeSinceFertilized = 0f;
                break;
            
            case "plow":
                if (currentState == STATE.ALIVE) {
                    HarvestSelf();
                }
                break;
            
            default:
                break;
        }
    }

    private void OnMouseEnter() {
        if (!playerController.activeItem) return;

        switch (playerController.activeItemName) {
            case "kangla":
                Cursor.SetCursor(kanglaCursor, Vector2.zero, CursorMode.Auto);
                break;
            
            case "shit":
                Cursor.SetCursor(shitCursor, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
      
    private void OnMouseExit() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}

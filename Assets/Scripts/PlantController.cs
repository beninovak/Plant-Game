using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantController : MonoBehaviour {

    [Header("Sprites")]
    public SpriteRenderer groundSpriteRenderer;
    public Sprite spriteSeed, spriteSeedDead, spritePumpkin, spritePumpkinDead;

    [Header("Cursors")]
    public Texture2D hoeCursor;
    public Texture2D shitCursor;
    public Texture2D kanglaCursor;

    [HideInInspector] 
    public GameObject player;
    [HideInInspector]
    public PlayerController playerController;
    [HideInInspector]
    public GameController gameController;

    public TextMeshPro deathReasonText;
    
    private enum STATE {
        SEED,
        DEAD,
        ALIVE
    };

    public enum DEATH_REASON {
        UNHARVESTED,
        UNFERTILIZED,
        TOO_MUCH_WATER,
        NOT_ENOUGH_WATER,
        EATEN_BY_WILD_SWINE,
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
                groundSpriteRenderer.color = new Color(0.043f, 0.435f, 0.780f, 0.7f);
                if (timeSinceWatered > 20.0f) {
                    groundSpriteRenderer.color = new Color(0.043f, 0.435f, 0.780f, 0.3f);

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
                        spriteRenderer.sprite = spritePumpkin;
                        groundSpriteRenderer.color = new Color(0f, 0f, 0f, 0f);
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
        isWatered = false;
        timeSinceWatered = 0f;
        isFertilized = false;
        timesFertilized = 0;
        timeSinceFertilized = 0f;
        timeSinceCropAvailable = 0f;
        currentState = STATE.SEED;
        spriteRenderer.sprite = spriteSeed;

        gameController.AddPumpkin();
        // TODO - Give player pumpkin + drop seed
    }

    public void KillSelf(DEATH_REASON deathReason) {
        isWatered = false;
        timeSinceWatered = 0f;
        timesFertilized = 0;
        timeSinceFertilized = 0f;
        
        switch (currentState) {
            case STATE.SEED:
                spriteRenderer.sprite = spriteSeedDead;
                break;
            
            case STATE.ALIVE:
                spriteRenderer.sprite = spritePumpkinDead;
                break;
        }
        
        currentState = STATE.DEAD;

        groundSpriteRenderer.color = new Color(0.352f, 0.156f, 0.101f, 1f);
        deathReasonText.text = $"Died because:\n {deathReason}";
        Invoke("RemoveDeathReason", 2f);
    }

    private void RemoveDeathReason() {
        Destroy(deathReasonText);
    }
    
    private void UpdatePlayerItemCount(string itemName) {
        var item = playerController.inventory[itemName];
        item.count--;
        playerController.inventory[itemName] = item;

        if (item.count <= 0) {
            playerController.UnEquipItems();
        } else {
            playerController.itemCounter.text = item.count.ToString();
        }
    }
    
    private void OnMouseDown() {
        if (!playerController.activeItem || playerController.inventory[playerController.activeItemName].count <= 0 || currentState == STATE.DEAD) return;
        
        // TODO - add click particle effect
        float distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (distanceFromPlayer > 2f) return;
        
        string usedItem = playerController.activeItemName;

        switch (usedItem) {
            case "kangla":
                KanglaController kanglaController = playerController.activeItem.GetComponent<KanglaController>();
                if (!kanglaController.IsUsable()) break;
                kanglaController.Use();

                UpdatePlayerItemCount(usedItem);
                if (isWatered && timeSinceWatered < 10.0f) {
                    KillSelf(DEATH_REASON.TOO_MUCH_WATER);
                    break;
                }
                
                isWatered = true;
                timeSinceWatered = 0f;
                groundSpriteRenderer.color = new Color(0.043f, 0.435f, 0.780f, 1f);
                break;
            
            case "shit":
                isFertilized = true;
                timesFertilized++;
                timeSinceFertilized = 0f;
                UpdatePlayerItemCount(usedItem);
                break;
            
            case "hoe":
                if (currentState == STATE.ALIVE) {
                    HarvestSelf();
                }
                UpdatePlayerItemCount(usedItem);
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
            
            case "hoe":
                Cursor.SetCursor(hoeCursor, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
      
    private void OnMouseExit() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}

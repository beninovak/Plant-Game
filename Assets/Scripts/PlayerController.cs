using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public struct InventoryItem {
        public byte count;
        public GameObject item;
        public bool hasBeenSpawned;
    }
    
    [Header ("General")]
    public Animator animator;
    private float moveSpeed = 10f;
    private float verticalMove;
    private float horizontalMove;
    public GameObject spawnPointLeftHand, spawnPointRightHand;

    [Header ("Available items")]
    public GameObject kangla;
    public GameObject shit;
    
    
    [HideInInspector]
    public GameObject activeItem;
    private SpriteRenderer activeItemSpriteRenderer;
    
    public Dictionary<string, InventoryItem> inventory = new Dictionary<string, InventoryItem>();

    private void Awake() {
        // activeItem = new GameObject();
        // activeItemSpriteRenderer = new SpriteRenderer();
        inventory.Add("kangla", new InventoryItem() { count = 1, item = kangla, hasBeenSpawned = false } );
    }

    private void Start() {
        // InvokeRepeating("PrintInventory", 1f, 2f);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            UnEquipItems();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1) && inventory.ContainsKey("kangla")) {
            EquipItem("kangla");
        } else if (Input.GetKeyDown(KeyCode.Alpha2) && inventory.ContainsKey("shit")) {
            EquipItem("shit");
        }
    }

    private void FixedUpdate() {
        verticalMove = Input.GetAxisRaw("Vertical");
        horizontalMove = Input.GetAxisRaw("Horizontal");
        
        if (verticalMove < 0) {
            animator.SetFloat("yVelocity", verticalMove);
            animator.SetBool("movingVertically", true);
            transform.Translate(new Vector2(0, -moveSpeed * Time.deltaTime));
        } else if (verticalMove > 0) {
            animator.SetFloat("yVelocity", verticalMove);
            animator.SetBool("movingVertically", true);
            transform.Translate(new Vector2(0, moveSpeed * Time.deltaTime));
        } else {
            animator.SetFloat("yVelocity", 0f);
            animator.SetBool("movingVertically", false);
        }
        
        if (horizontalMove < 0) {
            animator.SetFloat("xVelocity", horizontalMove);
            animator.SetBool("movingHorizontally", true);
            transform.Translate(new Vector2(-moveSpeed * Time.deltaTime, 0));

            if (activeItem && activeItemSpriteRenderer) {
                activeItemSpriteRenderer.flipX = false;
                activeItem.transform.position = spawnPointLeftHand.transform.position;   
            }
        } else if (horizontalMove > 0) {
            animator.SetFloat("xVelocity", horizontalMove);
            animator.SetBool("movingHorizontally", true);
            transform.Translate(new Vector2(moveSpeed * Time.deltaTime, 0));

            if (activeItem && activeItemSpriteRenderer) {
                activeItemSpriteRenderer.flipX = true;
                activeItem.transform.position = spawnPointRightHand.transform.position;
            }
        } else {
            animator.SetFloat("xVelocity", 0f);
            animator.SetBool("movingHorizontally", false);
        }
    }

    private void EquipItem(string name) {

        // string output = "";
        // foreach (var item in inventory) {
        //     output += $"{item.Key} ";
        // }
        // Debug.Log(output);
        
        var inventoryItem = inventory[name];
        Debug.Log(inventoryItem.count);
        Debug.Log(inventoryItem.hasBeenSpawned);
        if (inventoryItem.hasBeenSpawned == false) {
            if (horizontalMove <= 0) {
                // Debug.Log(inventoryItem);
                activeItem = Instantiate(inventoryItem.item, spawnPointLeftHand.transform.position, Quaternion.identity, spawnPointLeftHand.transform);
            } else {
                activeItem = Instantiate(inventoryItem.item, spawnPointRightHand.transform.position, Quaternion.identity, spawnPointRightHand.transform);
            }

            activeItem.AddComponent<ItemController>().name = name;
            inventoryItem.hasBeenSpawned = true;
            // activeItem.SetActive(true);
        } else {
            ItemController[] items = FindObjectsByType<ItemController>(FindObjectsSortMode.None);
            foreach (var item in items) {
                if (item.name == name) {
                    Debug.Log($"Found item with name {name}");
                    activeItem = item.gameObject;
                }
            }
        }
        
        inventory[name] = inventoryItem;
        activeItemSpriteRenderer = activeItem.GetComponent<SpriteRenderer>();
    }

    private void UnEquipItems() {
        if (activeItem) {
            activeItem = null;
            activeItemSpriteRenderer.sprite = null;
        }
    }

    private void UseItem() {
        
    }

    private void PrintInventory() {
        string output = "";
        foreach (var item in inventory) {
            Debug.Log(item.Value.item.name);
            output += $"{item.Value.item.name}: {item.Value.count}";
        }
        Debug.Log(output);
    }
}
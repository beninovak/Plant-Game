using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour {

    public struct InventoryItem {
        public byte count;
        public GameObject item;
    }
    
    [Header ("General")]
    public Animator animator;
    private float moveSpeed = 10f;
    private float verticalMove;
    private float horizontalMove;
    public GameObject spawnPointLeftHand, spawnPointRightHand;

    [Header("Available items")] public GameObject kangla;
    public GameObject shit;
    public GameObject hoe;
    
    [HideInInspector]
    public GameObject activeItem;
    [HideInInspector]
    public string activeItemName = "";

    private SpriteRenderer activeItemSpriteRenderer;
    
    // TODO - Should item have a general "properties" field, which would allow accesing custom item methods...E.g. MakeKanglaFull()??  
    public Dictionary<string, InventoryItem> inventory = new Dictionary<string, InventoryItem>();

    private void Awake() {
        inventory.Add("kangla", new InventoryItem() { count = 1, item = kangla } );
        inventory.Add("hoe", new InventoryItem() { count = 1, item = hoe } );
    }

    private void Start() {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");
        foreach (GameObject plant in plants) {
            plant.GetComponent<PlantController>().player = gameObject;
            plant.GetComponent<PlantController>().playerController = this;
        }
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
        } else if (Input.GetKeyDown(KeyCode.Alpha3) && inventory.ContainsKey("hoe")) {
            EquipItem("hoe");
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

    public void ActiveItemInventoryUpdate() {
        if (activeItem) {
            string _activeItemName = activeItem.GetComponent<ItemController>().name;
            var tempItem = inventory[_activeItemName];
            tempItem.item = activeItem;
            inventory[_activeItemName] = tempItem;
        }
    }

    private void EquipItem(string name) {
        UnEquipItems();
        if (horizontalMove <= 0) {
            activeItem = Instantiate(inventory[name].item, spawnPointLeftHand.transform.position, Quaternion.identity, spawnPointLeftHand.transform);
        } else {
            activeItem = Instantiate(inventory[name].item, spawnPointRightHand.transform.position, Quaternion.identity, spawnPointRightHand.transform);
        }
        activeItemName = name;
        activeItem.AddComponent<ItemController>().name = name;
        activeItemSpriteRenderer = activeItem.GetComponent<SpriteRenderer>();
    }

    private void UnEquipItems() {
        if (activeItem) {
            // activeItem.SetActive(false);
            Destroy(activeItem);
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
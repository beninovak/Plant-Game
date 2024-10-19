using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour {

    public struct InventoryItem {
        public uint count;
        public GameObject item;
        public bool isInstantiated;
        public bool isPermanent;
    }
    
    [Header ("General")]
    public Animator animator;
    private float moveSpeed = 10f;
    private float verticalMove;
    private float horizontalMove;
    public GameObject spawnPointLeftHand, spawnPointRightHand;

    [Header("Available items")] 
    public GameObject kangla;
    public GameObject shit;
    public GameObject hoe;
    
    [HideInInspector]
    public GameObject activeItem;
    [HideInInspector]
    public string activeItemName = "";

    public TextMeshPro itemCounter;
    private SpriteRenderer activeItemSpriteRenderer;
    
    // TODO - Should item have a general "properties" field, which would allow accesing custom item methods...E.g. MakeKanglaFull()??  
    public Dictionary<string, InventoryItem> inventory = new Dictionary<string, InventoryItem>();

    private void Awake() {
        inventory.Add("kangla", new InventoryItem() { count = 0, item = kangla, isInstantiated = false, isPermanent = true} );
        inventory.Add("hoe", new InventoryItem() { count = 1_000, item = hoe, isInstantiated = false, isPermanent = true} );
    }

    private void Start() {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");
        foreach (GameObject plant in plants) {
            plant.GetComponent<PlantController>().player = gameObject;
            plant.GetComponent<PlantController>().playerController = this;
        }
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

        if (!inventory[name].isPermanent && inventory[name].count <= 0) return;
        
        UnEquipItems();

        if (inventory[name].isInstantiated) {
            activeItem = inventory[name].item;
            activeItem.SetActive(true);
        } else {
            if (horizontalMove <= 0) {
                activeItem = Instantiate(inventory[name].item, spawnPointLeftHand.transform.position, Quaternion.identity, spawnPointLeftHand.transform);
            } else {
                activeItem = Instantiate(inventory[name].item, spawnPointRightHand.transform.position, Quaternion.identity, spawnPointRightHand.transform);
            }
            var item = inventory[name]; 
            item.isInstantiated = true;
            inventory[name] = item;
            activeItem.AddComponent<ItemController>().name = name;
        }
        itemCounter.text = inventory[name].count.ToString();
        activeItemName = name;
        activeItemSpriteRenderer = activeItem.GetComponent<SpriteRenderer>();
    }

    public void UnEquipItems() {
        if (activeItem) {
            activeItem.SetActive(false);
            ActiveItemInventoryUpdate();
        }
    }
}
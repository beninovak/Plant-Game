using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public Animator animator;
    private float moveSpeed = 10f;
    private float verticalMove;
    private float horizontalMove;
    
    private bool kanglaEquipped = false;
    private GameObject spawnedKangla;
    
    public GameObject kangla;
    private SpriteRenderer kanglaSpriteRenderer;
    public GameObject kanglaSpawnPointLeft, kanglaSpawnPointRight;
    
    [HideInInspector]
    public KanglaController kanglaController;
    
    [HideInInspector]
    public string activeItem = "";

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (kanglaEquipped && spawnedKangla) {
                Destroy(spawnedKangla);
                kanglaEquipped = false;
            }
        }
        
        if (!kanglaEquipped && Input.GetKeyDown(KeyCode.Alpha1)) {
            activeItem = "Kangla";
            kanglaEquipped = true;
            
            if (horizontalMove <= 0) {
                spawnedKangla = Instantiate(kangla, kanglaSpawnPointLeft.transform.position, Quaternion.identity, kanglaSpawnPointLeft.transform);
            } else {
                spawnedKangla = Instantiate(kangla, kanglaSpawnPointRight.transform.position, Quaternion.identity, kanglaSpawnPointRight.transform);
            }
            kanglaController = spawnedKangla.GetComponent<KanglaController>();
            kanglaSpriteRenderer = spawnedKangla.GetComponent<SpriteRenderer>();
        }
        
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
            
            if (kanglaEquipped) {
                kanglaSpriteRenderer.flipX = false;
                spawnedKangla.transform.position = kanglaSpawnPointLeft.transform.position;
            }
        } else if (horizontalMove > 0) {
            animator.SetFloat("xVelocity", horizontalMove);
            animator.SetBool("movingHorizontally", true);
            transform.Translate(new Vector2(moveSpeed * Time.deltaTime, 0));

            if (kanglaEquipped) {
                kanglaSpriteRenderer.flipX = true;
                spawnedKangla.transform.position = kanglaSpawnPointRight.transform.position;
            }
        } else {
            animator.SetFloat("xVelocity", 0f);
            animator.SetBool("movingHorizontally", false);
        }
    }
    
    private void FixedUpdate() {
        
        
    }
}

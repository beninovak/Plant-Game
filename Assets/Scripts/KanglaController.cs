using System;
using UnityEngine;

public class KanglaController : MonoBehaviour {
    public bool isFull = false;
    public Sprite kanglaFull, kanglaEmpty;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (isFull) {
            spriteRenderer.sprite = kanglaFull;    
        } else { 
            spriteRenderer.sprite = kanglaEmpty;
        }
    }
    
    public void makeKanglaFull() {
        isFull = true;
        spriteRenderer.sprite = kanglaFull;
    }

    public void makeKanglaEmpty() {
        isFull = false;
        spriteRenderer.sprite = kanglaEmpty;
    }
}

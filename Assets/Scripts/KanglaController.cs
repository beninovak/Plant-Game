using System;
using UnityEngine;

public class KanglaController : MonoBehaviour {
    public Sprite kanglaFull, kanglaEmpty;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = kanglaEmpty;
    }
    
    public void makeKanglaFull() {
        spriteRenderer.sprite = kanglaFull;
    }

    public void makeKanglaEmpty() {
        spriteRenderer.sprite = kanglaEmpty;
    }
}

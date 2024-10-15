using System;
using UnityEngine;

public class KanglaController : MonoBehaviour {
    public bool isFull = false;
    public Sprite kanglaFull, kanglaEmpty;
    private SpriteRenderer spriteRenderer;
    
    [HideInInspector]
    public int uses = 5;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (isFull) {
            spriteRenderer.sprite = kanglaFull;
        } else {
            spriteRenderer.sprite = kanglaEmpty;
        }
    }

    public void Use() {
        uses--;

        if (uses <= 0) {
            MakeKanglaEmpty();
        }
    }

    public bool IsUsable() {
        if (isFull && uses > 0) {
            return true;
        }

        return false;
    }

    public void MakeKanglaFull() {
        isFull = true;
        uses = 5;
        spriteRenderer.sprite = kanglaFull;
    }

    public void MakeKanglaEmpty() {
        isFull = false;
        spriteRenderer.sprite = kanglaEmpty;
    }
}

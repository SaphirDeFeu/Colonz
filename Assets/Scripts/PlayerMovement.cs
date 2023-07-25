using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    public Rigidbody2D rb;
    public float speed = 12f;
    public string movementKeys = "WASD";
    public bool hasBoat = false;
    public Vector3 spawnpoint = new Vector3(0f, 0f, 0f);

    public static Collider2D colliderCurrentlyIn;

    private char[] movKeys = { 'w', 'a', 's', 'd' };
    private Vector3 currentPosBeforeEnter;
    private Vector3 currentPosAfterEnter;
    private bool canGo = true;

    private int forbiddenCounter = 0;
    private bool canForbidCounterCount = false;

    private Vector2 movement;

    // Start is called before the first frame update
    void Start() {
        movementKeys = movementKeys.ToLower();
        movKeys = movementKeys.ToCharArray();
        transform.position = spawnpoint;
        Debug.Log("PlayerMovement.cs successfully loaded!");
    }

    public void OnMoveHorizontal(InputValue value) {
        Vector2 input = value.Get<Vector2>();
        movement.x = input.x;
    }

    public void OnMoveVertical(InputValue value) {
        Vector2 input = value.Get<Vector2>();
        movement.y = input.y;
    }

    void OnBoatToggle() {
        hasBoat = !hasBoat;
    }

    // Update is called once per frame
    void Update() {

        if(canForbidCounterCount) {
            Debug.LogWarning("Player might be blocked inside inaccessible collider!");
            forbiddenCounter += 1;
            if(forbiddenCounter > 480) {
                Debug.Log("Player might have been blocked inside inaccessible collider, removing him.");
                transform.position = spawnpoint;
                forbiddenCounter = 0;
                canForbidCounterCount = false;
            }
        }
        
        if(canGo && !InventoryManagement.isInInventory(gameObject)) {
            rb.MovePosition(rb.transform.position + ( ( new Vector3(movement.x, movement.y, 0) ).normalized * speed * Time.deltaTime ) );
        } else if(!canGo) {
            transform.position = currentPosBeforeEnter;
        }

        transform.eulerAngles = Vector3.forward * 0;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        colliderCurrentlyIn = other;
        currentPosAfterEnter = transform.position;

        switch(other.tag) {
            case "NeedsBoat":
                if(!hasBoat) {
                    canGo = false;
                    canForbidCounterCount = true;
                }
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        colliderCurrentlyIn = null;
        canForbidCounterCount = false;
        forbiddenCounter = 0;
        switch(other.tag) {
            case "NeedsBoat":
                canGo = true;
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(colliderCurrentlyIn == null) {
            colliderCurrentlyIn = other;
        }
    }

    private void LateUpdate() {
        currentPosBeforeEnter = transform.position;
    }
}

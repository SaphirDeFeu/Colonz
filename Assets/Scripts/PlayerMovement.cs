using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    public Rigidbody2D rb;
    public float speed = 12f;
    public bool hasBoat = false;
    public Vector3 spawnpoint = new Vector3(0f, 0f, 0f);

    public static Collider2D colliderCurrentlyIn;
    private Vector3 currentPosBeforeEnter;
    private Vector3 currentPosAfterEnter;
    private bool canGo = true;

    private int forbiddenCounter = 0;
    private bool canForbidCounterCount = false;

    private Vector2 movement;

    // Start is called before the first frame update
    void Start() {
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

        if(canForbidCounterCount) { // Possibilité que le joueur soit bloqué
            Debug.LogWarning("Player might be blocked inside inaccessible collider!");
            forbiddenCounter += 1;
            if(forbiddenCounter > 480) { // Après 480 frames
                Debug.Log("Player might have been blocked inside inaccessible collider, removing him.");
                transform.position = spawnpoint;
                forbiddenCounter = 0;
                canForbidCounterCount = false;
            }
        }
        
        // On vérifie si on a pas ouvert l'inventaire, et qu'on peut se déplacer là où on veut
        // Comme par exemple quand on veut se déplacer dans l'océan sans bateau
        if(canGo && !InventoryManagement.isInInventory(gameObject)) {
            rb.MovePosition(rb.transform.position + ( ( new Vector3(movement.x, movement.y, 0) ).normalized * speed * Time.deltaTime ) );
        } else if(!canGo) { // Si on peut PAS se déplacer là où on veut
            transform.position = currentPosBeforeEnter; // on revient à l'endroit où on était avant d'entrer
        }

        transform.eulerAngles = Vector3.forward * 0;
        // Il y a un bug qui fait que le joueur tourne sur lui même après collision avec un autre rigibody
        // Donc on force le joueur à se mettre à 0°
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // On spécifie dans quel collider on est
        colliderCurrentlyIn = other;
        currentPosAfterEnter = transform.position;

        switch(other.tag) {
            case "NeedsBoat": // si on entre dans un océan
                if(!hasBoat) { // et qu'on a pas de bateau
                    canGo = false; // on ne peut pas passer <shrug>
                    canForbidCounterCount = true;
                }
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        // reset le collider on est plus dans un collider
        colliderCurrentlyIn = null;
        canForbidCounterCount = false;
        forbiddenCounter = 0;
        switch(other.tag) {
            case "NeedsBoat": // c'est un océan qu'on a quitté?
                canGo = true; // si oui yay
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(colliderCurrentlyIn == null) { // en fait si on est toujours dans un collider on le remet à ce collider
            colliderCurrentlyIn = other;
        }
    }

    private void LateUpdate() {
        currentPosBeforeEnter = transform.position; // sauvegarde de la position avant la prochaine frame
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    public Tilemap[] tilemap = new Tilemap[3];
    public Rigidbody2D rb;
    public float speed = 12f;
    public bool hasBoat = false;
    public GameObject ourBoat;
    public Vector3 spawnpoint = new Vector3(0f, 0f, 0f);

    public static List<Collider2D> collidersCurrentlyIn = new List<Collider2D>();
    private Vector3 currentPosBeforeEnter;
    private Vector3 currentPosAfterEnter;
    private bool canGo = true;

    private int forbiddenCounter = 0;
    private bool canForbidCounterCount = false;

    private Dictionary<string, Color> stringToColor = new Dictionary<string, Color>();
    private Dictionary<Color, string> colorToString = new Dictionary<Color, string>();

    private Vector2 movement;

    // Start is called before the first frame update
    void Start() {

        colorToString.Add(new Color(1.0f, 1.0f, 1.0f, 1.0f), "NEUTRAL");
        colorToString.Add(new Color(1.0f, 0.0f, 0.0f, 1.0f), "RED");
        colorToString.Add(new Color(0.0f, 1.0f, 0.0f, 1.0f), "BLUE");
        colorToString.Add(new Color(0.0f, 0.0f, 1.0f, 1.0f), "GREEN");
        colorToString.Add(new Color(1.0f, 0.0f, 1.0f, 1.0f), "YELLOW");

        stringToColor.Add("NEUTRAL", new Color(1.0f, 1.0f, 1.0f, 1.0f));
        stringToColor.Add("RED", new Color(1.0f, 0.0f, 0.0f, 1.0f));
        stringToColor.Add("BLUE", new Color(0.0f, 1.0f, 0.0f, 1.0f));
        stringToColor.Add("GREEN", new Color(0.0f, 0.0f, 1.0f, 1.0f));
        stringToColor.Add("YELLOW", new Color(1.0f, 0.0f, 1.0f, 1.0f));

        transform.position = spawnpoint;
        ourBoat.SetActive(false); // on suppose qu'on spawn pas dans l'océan
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
        Vector3Int integerPos = new Vector3Int(
            (int) Math.Floor(transform.position.x),
            (int) Math.Floor(transform.position.y),
            (int) Math.Floor(transform.position.z)
        );


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

        if(tilemap[1].GetTile(integerPos).name == "main_11") {
            transform.position = currentPosBeforeEnter;
        }
        
        try {
            if(tilemap[0].GetTile(integerPos).name == "main_24") {
                transform.position = currentPosBeforeEnter;
            }
        } catch(NullReferenceException e) {
            // do nothing, there's no decoration on this tile
        }

        transform.eulerAngles = Vector3.forward * 0;
        // Il y a un bug qui fait que le joueur tourne sur lui même après collision avec un autre rigibody
        // Donc on force le joueur à se mettre à 0°
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // On spécifie dans quel collider on est
        if(!collidersCurrentlyIn.Contains(other)) {
            collidersCurrentlyIn.Add(other);
        }
        currentPosAfterEnter = transform.position;

        switch(other.tag) {
            case "NeedsBoat": // si on entre dans un océan
                if(!hasBoat) { // et qu'on a pas de bateau
                    canGo = false; // on ne peut pas passer <shrug>
                    canForbidCounterCount = true;
                } else {
                    ourBoat.SetActive(true); // montrer le bateau
                }
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        // reset le collider on est plus dans un collider
        if(collidersCurrentlyIn.Contains(other)) {
            collidersCurrentlyIn.Remove(other);
        } else {
            Debug.LogError("Unknown collider exit!");
        }
        canForbidCounterCount = false;
        forbiddenCounter = 0;
        switch(other.tag) {
            case "NeedsBoat": // c'est un océan qu'on a quitté?
                canGo = true; // si oui yay
                ourBoat.SetActive(false); // on a plus besoin du bateau
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(!collidersCurrentlyIn.Contains(other)) { // en fait si on est toujours dans un collider on le remet à ce collider
            collidersCurrentlyIn.Add(other);
        }
        if(other.tag == "Province") {
            Debug.Log("You are in " + colorToString[other.gameObject.GetComponent<SpriteRenderer>().color] + " territory.");
        }
    }

    private void LateUpdate() {
        currentPosBeforeEnter = transform.position; // sauvegarde de la position avant la prochaine frame
        // Logging
        /*string collStringLog = "Inside colliders: ";
        for(int i = 0; i < collidersCurrentlyIn.Count; i++) {
            Collider2D collider = collidersCurrentlyIn[i];
            collStringLog += collider.name + ":" + collider.tag + ", ";
        }
        if(collStringLog != "Inside colliders: ") {
            Debug.Log(collStringLog);
        }*/
    }
}

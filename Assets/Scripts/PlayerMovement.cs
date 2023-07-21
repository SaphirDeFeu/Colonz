using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour {

    public Rigidbody2D rb;
    public float speed = 12f;
    public string movementKeys = "WASD";
    public bool hasBoat = false;
    public Vector3 spawnpoint = new Vector3(0f, 0f, 0f);

    public TextMeshProUGUI loadText;

    private char[] movKeys = { 'w', 'a', 's', 'd' };
    private Vector3 currentPosBeforeEnter;
    private Vector3 currentPosAfterEnter;
    private bool canGo = true;

    private int forbiddenCounter = 0;
    private bool canForbidCounterCount = false;

    // Start is called before the first frame update
    void Start() {
        movementKeys = movementKeys.ToLower();
        movKeys = movementKeys.ToCharArray();
        loadText.text = "";
        transform.position = spawnpoint;
        Debug.Log("PlayerMovement.cs successfully loaded!");
    }

    // Update is called once per frame
    void Update() {
        bool u = Input.GetKey(movKeys[0].ToString());
        bool l = Input.GetKey(movKeys[1].ToString());
        bool d = Input.GetKey(movKeys[2].ToString());
        bool r = Input.GetKey(movKeys[3].ToString());
        bool hasBoatToggle = Input.GetKey("h");

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

        if(hasBoatToggle) {
            hasBoat = !hasBoat;
        }
        
        float h = 0;
        float v = 0;

        if(u) {
            v = 1;
        }
        if(d) {
            v -= 1;
        }
        if(l) {
            h = -1;
        }
        if(r) {
            h += 1;
        }
        
        if(canGo) {
            rb.MovePosition(rb.transform.position + ( ( new Vector3(h, v, 0) ).normalized * speed * Time.deltaTime ) );
        } else {
            transform.position = currentPosBeforeEnter;
        }

        transform.eulerAngles = Vector3.forward * 0;
    }

    private void OnTriggerEnter2D(Collider2D other) {
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
        canForbidCounterCount = false;
        forbiddenCounter = 0;
        switch(other.tag) {
            case "NeedsBoat":
                canGo = true;
                break;
        }
    }

    private void LateUpdate() {
        currentPosBeforeEnter = transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManagement : MonoBehaviour {

    public TextMeshProUGUI coalAmount;
    public TextMeshProUGUI ironAmount;
    public TextMeshProUGUI woodAmount;

    private bool hasLeftClicked = false;
    private Dictionary<string, float> inventory = new Dictionary<string, float>();

    private Collider2D withinCollider = null;

    // Start is called before the first frame update
    void Start() {
        inventory.Add("coal", 0.0f);
        inventory.Add("iron", 0.0f);
        inventory.Add("wood", 0.0f);
        Debug.Log("InventoryManagement.cs successfully loaded!");
    }

    // Update is called once per frame
    void Update() {
        hasLeftClicked = false;
        if(Input.GetMouseButtonDown(0) && !hasLeftClicked) {
            hasLeftClicked = true;
        }
        if(withinCollider != null && hasLeftClicked) {
            if((withinCollider.tag).StartsWith("RES_")) {
                attack(withinCollider);
            }
        }

        if(hasLeftClicked && withinCollider == null) {
            attack(null);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        withinCollider = other;
    }

    void OnTriggerExit2D(Collider2D other) {
        withinCollider = null;
    }

    void attack(Collider2D other) {
        if(other != null) {
            if((other.tag).StartsWith("RES_")) {
                string type = DisplayResource.GetResTypeFromTag(other.tag);
                float amount = ((float)getResAmountFromTag(other.tag)) / 4.0f;
                switch(type) {
                    case "C":
                        inventory["coal"] += amount;
                        coalAmount.text = (Mathf.FloorToInt(inventory["coal"])).ToString();
                        break;
                    case "Fe":
                        inventory["iron"] += amount;
                        ironAmount.text = (Mathf.FloorToInt(inventory["iron"])).ToString();
                        break;
                    case "Wd":
                        inventory["wood"] += amount;
                        woodAmount.text = (Mathf.FloorToInt(inventory["wood"])).ToString();
                        break;
                    default:
                        Debug.LogError("Unknown resource type collection");
                        break;
                }
                Debug.Log(amount+"\namount of coal:"+inventory["coal"]+"\nwood amount:"+inventory["wood"]);
            }
        } else {
            // do stuff
        }
    }

    public static int getResAmountFromTag(string tag) {
        if (tag.StartsWith("RES_") && tag.Length >= 6) {
            // Get the last character of the tag
            char lastChar = tag[tag.Length - 1];

            // Try to parse the last character into an integer
            if (int.TryParse(lastChar.ToString(), out int result)) {
                return result;
            }
        }

        return 0;
    } 
}

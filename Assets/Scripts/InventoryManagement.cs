using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class InventoryManagement : MonoBehaviour {

    public TextMeshProUGUI coalAmount;
    public TextMeshProUGUI ironAmount;
    public TextMeshProUGUI woodAmount;

    public GameObject inventoryCanvas;

    public bool inInventory = false;

    private bool hasFired = false;
    private Dictionary<string, float> inventory = new Dictionary<string, float>();

    private Collider2D withinCollider = null;

    // Start is called before the first frame update
    void Start() {
        // Setup l'inventaire, on assume que c'est une nouvelle partie
        inventory.Add("coal", 0.0f);
        inventory.Add("iron", 0.0f);
        inventory.Add("wood", 0.0f);
        Debug.Log("InventoryManagement.cs successfully loaded!");
    }

    // Update is called once per frame
    void Update() {
        // À chaque fois que le joueur clique sur la souris
        if(withinCollider != null && hasFired) {
            if(withinCollider.tag == "Resource") {
                Attack(withinCollider); // Check si le collider où on est est une ressource et le faire passer comme collider d'attaque
            }
        }

        if(hasFired && withinCollider == null) {
            Attack(null); // Si on est pas dans un trigger alors on attaque du vide
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        withinCollider = other;
    }

    void OnTriggerExit2D(Collider2D other) {
        withinCollider = null;
    }

    public void OnFire(InputValue value) {
        hasFired = true;
    }

    void OnToggleInventory() { // Bouger ça dans GUI.cs sinon y a trop de bloatware
        inInventory = !inInventory;
        inventoryCanvas.SetActive(inInventory);
    }

    void LateUpdate() {
        hasFired = false;
    }

    void Attack(Collider2D other) {
        if(inInventory) { // Si on est dans l'inventaire, empêcher toute action.
            return;
        }

        if(other != null) { // Si on est bien dans un collider
            if(other.tag == "Resource") {
                string type = DisplayResource.GetResTypeFromName(other); // Voir dans DisplayResource script
                float amount = ((float)GetResAmountFromName(other)) / 4.0f;
                switch(type) { // En fonction de la ressource, modifier une certaine partie de l'inventaire.
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
            }
        } else {
            // y a rien de prévu pour le moment mais on va faire une animation plus tard jsp quand
        }
    }

    public static int GetResAmountFromName(Collider2D other) {
        string name = other.name;
        string tag = other.tag;
        if (tag == "Resource") {
            // Get the last character of the tag
            char lastChar = name[name.Length - 1];

            // Try to parse the last character into an integer
            if (int.TryParse(lastChar.ToString(), out int result)) {
                return result;
            }
        }

        return 0;
    } 

    public static bool isInInventory(GameObject gameObject) {
        if((gameObject.name).StartsWith("Player")) {
            return gameObject.GetComponent<InventoryManagement>().inInventory;
        }
        return false;
    }
}

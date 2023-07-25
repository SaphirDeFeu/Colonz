using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayResource : MonoBehaviour {

    public GameObject resourceDisplayer;
    public SpriteRenderer resourceSpriteRenderer;

    public Sprite coal;
    public Sprite iron;
    public Sprite wood;
    
    public static Vector3 displayCoords;

    // Start is called before the first frame update
    void Start() {
        Debug.Log("DisplayResource.cs successfully loaded!");
    }

    void OnTriggerEnter2D(Collider2D other) { // En entrant dans un collider
        if(other.tag == "Resource") { // Si c'est une ressource
            resourceDisplayer.SetActive(true); // Activer le display

            string resType = GetResTypeFromName(other); // C'est quel type de ressource?

            if (!string.IsNullOrEmpty(resType)) { // Est-ce qu'on a bien récupérer le type de ressource?
                // On positionne le sprite correctement via des coordonnées relatives
                Vector3 absoluteCoords = other.transform.position + new Vector3(0f, 0.6f, 0f);

                Display(resType, absoluteCoords); // On montre la ressource
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Resource") { // Si on quitte le trigger d'une ressource
            resourceDisplayer.SetActive(false); // Désactiver le display
        }
    }

    public void Display(string type, Vector3 coordinates) {
        switch(type) { // Quel type de ressource est-ce
            case "C":
                resourceSpriteRenderer.sprite = coal; // Charbon
                break;
            case "Fe":
                resourceSpriteRenderer.sprite = iron; // Fer
                break;
            case "Wd":
                resourceSpriteRenderer.sprite = wood; // Bois
                break;
            default:
                Debug.LogWarning("Unknown resource sprite/type!"); // On sait pas <shrug>
                break;
        }
        resourceDisplayer.transform.position = coordinates; // Aller aux coordonnées spécifiées
        displayCoords = transform.position; // On enregistre les coordonnées
    }

    public static string GetResTypeFromName(Collider2D collider) {
        // Les ressources sont nommées tel : xx-x où xx c'est une lettre majuscule suivi d'une lettre minuscule ou une lettre majuscule tout court
        // et le x c'est la quantité de ressource, un nombre entre 1 et 4 inclus
        string name = collider.name;
        int startIndex = 0;
        int endIndex = name.LastIndexOf('-');
        if (endIndex > startIndex && endIndex <= startIndex + 3) {
            return name.Substring(startIndex, endIndex - startIndex);
        }

        return null; // Ne peut pas récupérer le type de ressource
    }
}

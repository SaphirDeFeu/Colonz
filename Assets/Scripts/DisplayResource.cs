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

    private string bc = "Resource";

    // Start is called before the first frame update
    void Start() {
        Debug.Log("DisplayResource.cs successfully loaded!");
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == bc) {
            resourceDisplayer.SetActive(true);

            string resType = GetResTypeFromName(other);

            if (!string.IsNullOrEmpty(resType)) {
                // Retrieve absolute coordinates to position the sprite correctly
                Vector3 relativeCoords = new Vector3(0f, 0.6f, 0f);
                Vector3 absoluteCoords = other.transform.position + relativeCoords;

                Display(resType, absoluteCoords);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == bc) {
            resourceDisplayer.SetActive(false);
        }
    }

    public void Display(string type, Vector3 coordinates) {
        // Load the texture of the requested resource, then move the sprite to the coordinates given
        switch(type) {
            case "C":
                resourceSpriteRenderer.sprite = coal;
                break;
            case "Fe":
                resourceSpriteRenderer.sprite = iron;
                break;
            case "Wd":
                resourceSpriteRenderer.sprite = wood;
                break;
            default:
                Debug.LogWarning("Unknown resource sprite/type!");
                break;
        }
        resourceDisplayer.transform.position = coordinates;
        displayCoords = transform.position;
    }

    public static string GetResTypeFromName(Collider2D collider) {
        string name = collider.name;
        int startIndex = 0;
        int endIndex = name.LastIndexOf('-');
        if (endIndex > startIndex && endIndex <= startIndex + 3) {
            return name.Substring(startIndex, endIndex - startIndex);
        }

        return null; // Failed to extract ore type.
    }
}

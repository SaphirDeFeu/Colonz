using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI : MonoBehaviour {

    public Button guiModifier;
    public TextMeshProUGUI guiScaleText;
    public float guiScale = 1.0f;

    public CanvasScaler[] canvasScalers; 

    // Start is called before the first frame update
    void Start() {
        guiModifier.onClick.AddListener(OnClick);
    }

    void OnClick() {
        Debug.Log("Changing GUI Scale...");
        if(guiScale < 4.0f) {
            guiScale += 1.0f;
        } else if(guiScale >= 4.0f) {
            guiScale = 1.0f;
        }

        // Récuperer tous les objets Canvas spécifiés dans la liste
        // puis mettre leur échelle à l'échelle sélectionné
        foreach(CanvasScaler scaler in canvasScalers) {
            scaler.scaleFactor = guiScale;
        }
        guiScaleText.text = "GUI Scale: " + Mathf.FloorToInt(guiScale);
        Debug.Log("New GUI Scale: " + guiScale);
    }
}

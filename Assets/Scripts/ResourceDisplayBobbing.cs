using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDisplayBobbing : MonoBehaviour {

    // Ici BOBBING/bob = se déplacer de haut en bas

    public float minBobbing = -0.3f;
    public float maxBobbing = 0.3f;

    private float currentBobbing = 0f;
    private float bobbingDirection = 1.0f;

    private int counterBeforeNextBob = 0;
    private int maxCounter = 30;

    // Start is called before the first frame update
    void Start() {
        Debug.Log("ResourceDisplayBobbing.cs successfully loaded!");
    }

    void LateUpdate() {
        if(gameObject.activeSelf) { // Est-ce qu'on est actif
            if(currentBobbing >= maxBobbing) { // Est-ce qu'on a dépassé le plafond de BOBBING
                bobbingDirection = -1.0f;
                currentBobbing = maxBobbing;
            } else if(currentBobbing <= minBobbing) { // Est-ce qu'on a dépassé le sol de BOBBING
                bobbingDirection = 1.0f;
                currentBobbing = minBobbing;
            }

            if(counterBeforeNextBob >= maxCounter) { // si on a fini de compter pour le prochain bob
                counterBeforeNextBob = 0;
                currentBobbing += ( 0.1f * bobbingDirection );
                transform.position += new Vector3(0f, 0.1f * bobbingDirection, 0f);
            } else {
                counterBeforeNextBob += 1; // On compte pour qu'on bob pas chaque frame
            }
        }
        // si on est pas actif on bob pas
    }
}

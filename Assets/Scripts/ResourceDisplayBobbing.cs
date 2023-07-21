using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDisplayBobbing : MonoBehaviour {

    public float minBobbing = -0.3f;
    public float maxBobbing = 0.3f;

    private float currentBobbing = 0f;
    private float bobbingDirection = 1.0f;

    private int counterBeforeNextBob = 0;
    private int maxCounter = 20;

    // Start is called before the first frame update
    void Start() {
        Debug.Log("ResourceDisplayBobbing.cs successfully loaded!");
    }

    // Update is called once per frame
    void Update() {
        
    }

    void LateUpdate() {
        if(gameObject.activeSelf) {
            if(currentBobbing >= maxBobbing) {
                bobbingDirection = -1.0f;
                currentBobbing = maxBobbing;
            } else if(currentBobbing <= minBobbing) {
                bobbingDirection = 1.0f;
                currentBobbing = minBobbing;
            }

            if(counterBeforeNextBob >= maxCounter) {
                counterBeforeNextBob = 0;
                currentBobbing += ( 0.1f * bobbingDirection );
                transform.position += new Vector3(0f, 0.1f * bobbingDirection, 0f);
            } else {
                counterBeforeNextBob += 1;
            }
        } else {
            counterBeforeNextBob = 0;
        }
    }
}

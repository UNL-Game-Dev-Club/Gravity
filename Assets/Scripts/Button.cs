using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	public bool triggered;
	public int triggerFrames;

    // Start is called before the first frame update
    void Start () {
        
	}

    // Update is called once per frame
    void Update () {
        if (triggered) {
        	transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(1, 0.25f, 1), 1);
        	triggerFrames++;
        }
        else {
        	transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(1, 1, 1), 1);
        	triggerFrames = 0;
        }
    }

    void OnTriggerStay2D (Collider2D other) {
    	if (other.tag == "Ground") {
    		return;
    	}
    	if (other.isTrigger) {
    		return;
    	}

    	triggered = true;
    }

    void OnTriggerExit2D (Collider2D other) {
    	if (other.tag == "Ground") {
    		return;
    	}
    	if (other.isTrigger) {
    		return;
    	}

    	triggered = false;
    }
}
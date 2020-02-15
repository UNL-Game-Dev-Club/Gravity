using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public enum DoorType {
		AllButtons,
		OneButton
	}

	public DoorType type;

	public Button[] buttons;
	public GameObject door1;
	public GameObject door2;

	public bool open;

    // Start is called before the first frame update
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
    	if (open) {
    		door1.transform.localScale = Vector3.MoveTowards(door1.transform.localScale, new Vector3(1, 0, 1), 3 * Time.deltaTime);
    		door2.transform.localScale = Vector3.MoveTowards(door2.transform.localScale, new Vector3(1, 0, 1), 3 * Time.deltaTime);
    	}
    	else {
    		door1.transform.localScale = Vector3.MoveTowards(door1.transform.localScale, new Vector3(1, 1, 1), 3 * Time.deltaTime);
    		door2.transform.localScale = Vector3.MoveTowards(door2.transform.localScale, new Vector3(1, 1, 1), 3 * Time.deltaTime);
    	}

    	int activeCount = 0;

        foreach (Button button in buttons) {
        	if (button.triggered) {
        		activeCount++;
        	}
        }

        switch (type) {
        	case DoorType.AllButtons:
        		open = (activeCount > 0);
        	break;

        	case DoorType.OneButton:
        		open = (activeCount >= buttons.Length);
        	break;
        }
    }
}
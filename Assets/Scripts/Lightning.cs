using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

	public GameObject hitBox;

    // Start is called before the first frame update
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
    	Vector2 position = new Vector2(transform.position.x, transform.position.y);
    	Vector2 upVector = new Vector2(transform.up.x, transform.up.y);

        RaycastHit2D hit = Physics2D.Raycast(position + (upVector * 0.5f), upVector, 8);

        if (hit.collider == null) {
        	hitBox.transform.localScale = new Vector3(1, 8, 1);
        }
        else {
        	hitBox.transform.localScale = new Vector3(1, hit.distance + 0.5f, 1);
        }
    }
}
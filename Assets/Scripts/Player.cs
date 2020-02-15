using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public GameObject pointer;

	public float moveSpeed;

	private Rigidbody2D rb;
	private Collider2D collider;
	
	public bool grounded;

    // Start is called before the first frame update
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate () {
    	CheckForGround();

        float movement = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(movement * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown("space")) {
        	Jump();
        }
    }

    void Jump () {

    }

    void CheckForGround () {
    	bool setGround = false;
    	ContactPoint2D[] contacts = new ContactPoint2D[10];

    	int count = collider.GetContacts(contacts);

    	foreach (ContactPoint2D contact in contacts) {
    		if (contact.collider == null) {
    			continue;
    		}

    		pointer.transform.LookAt(contact.point);

    		float angle = FixAngle(pointer.transform.eulerAngles.x);

    		Debug.Log(angle);

    		if (angle > 45 && angle < 135) {
    			setGround = true;
    		}
    	}

    	grounded = setGround;
    }

    float FixAngle (float angle) {
    	while (angle < 0) {
    		angle += 360;
    	}

    	while (angle >= 360) {
    		angle -= 360;
    	}

    	return angle;
    }
}















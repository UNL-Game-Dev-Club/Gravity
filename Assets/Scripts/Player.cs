using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public GameObject pointer;
	public GameObject mainGrid;

	public float moveSpeed;
	public float jumpForce;
	public float maxJumpHeight;
	public float fallGravity;

	public float wallDetectRange;

	private Rigidbody2D rb;
	private Collider2D mainCollider;
	private Animator animator;
	private SpriteRenderer sr;
	
	public bool grounded;

	// Jump stuff
	public bool canJump;
	public bool jumping;
	private int jumpPhase;
	private float jumpedFrom;

	private float gravityAngle;
	private float gravityDirection;

    // Start is called before the first frame update
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        gravityAngle = 0;
    }

    // Update is called once per frame
    void FixedUpdate () {
    	if (mainGrid.transform.eulerAngles == new Vector3(0, 0, gravityAngle)) {
    		CheckForWalls();
    	}
    	else {
    		rb.velocity = new Vector2(0, 0);
    		transform.eulerAngles = new Vector3(0, 0, 0);

    		mainGrid.transform.RotateAround(transform.position, transform.forward, gravityDirection * 5);

    		if (mainGrid.transform.eulerAngles.z > gravityAngle - 5 && mainGrid.transform.eulerAngles.z < gravityAngle + 5) {
    			mainGrid.transform.eulerAngles = new Vector3(0, 0, gravityAngle);
    			transform.eulerAngles = new Vector3(0, 0, 0);
    		}
    	}

    	CheckForGround();
    	UpdateJumping();

        float movement = Input.GetAxis("Horizontal");

        if (movement < 0) {
			Animate("Left");
			sr.flipX = false;
        }
        else if (movement > 0) {
			Animate("Left");
			sr.flipX = true;
        }
        else {
			Animate("Idle");
        }

        rb.velocity = new Vector2(movement * moveSpeed, rb.velocity.y);

        if (Input.GetKey("space")) {
        	Jump();
        }
    }

    // Make the player jump
    void Jump () {
    	if ((!grounded && !canJump) || jumping) {
    		return;
    	}

    	jumping = true;
    	jumpPhase = 0;

    	jumpedFrom = transform.position.y;

    	rb.velocity += new Vector2(0, jumpForce);
    }

    // Update jump-related variables
    void UpdateJumping () {
    	Vector2 downVector = new Vector2(transform.up.x * -1, transform.up.y * -1);
    	Vector2 castOrigin = new Vector2(transform.position.x, transform.position.y) + (downVector * 0.55f);
    	RaycastHit2D hit = Physics2D.Raycast(castOrigin, downVector, 0.25f);

    	if (hit.collider != null) {
    		canJump = true;
    	}
    	else {
    		canJump = false;
    	}

    	if (!jumping) {
    		rb.gravityScale = fallGravity;
    		return;
    	}

        if ((transform.position.y > jumpedFrom + maxJumpHeight || !Input.GetKey("space")) && jumpPhase < 2) {
        	jumpPhase = 2;
        	rb.gravityScale = fallGravity;
        	rb.velocity = new Vector2(rb.velocity.x, 0);
       	}

       	if (jumpPhase == 0 && rb.velocity.y > 0) {
       		jumpPhase = 1;
       		rb.gravityScale = 1;
       	}
        else if (jumpPhase == 1 && rb.velocity.y < 0) {
        	jumpPhase = 2;
        	rb.gravityScale = fallGravity;
        }
    }

    // Check for walls that the player can stick to
    void CheckForWalls () {
    	if (!jumping) {
    		return;
    	}

    	Vector2 upVector = new Vector2(transform.up.x, transform.up.y);
    	Vector2 rightVector = new Vector2(transform.right.x, transform.right.y);
    	Vector2 castOrigin = new Vector2(0, 0);
    	RaycastHit2D hit;

    	// Check to the left
    	castOrigin = new Vector2(transform.position.x, transform.position.y) + (rightVector * -0.55f);
    	hit = Physics2D.Raycast(castOrigin, rightVector * -1, wallDetectRange);

    	if (hit.collider != null && (Input.GetKey("a") || Input.GetKey("left"))) {
    		if (hit.collider.tag == "Ground") {
    			ChangeGravity(90, 1);
    		}
    	}

    	// Check to the right
    	castOrigin = new Vector2(transform.position.x, transform.position.y) + (rightVector * 0.55f);
    	hit = Physics2D.Raycast(castOrigin, rightVector, wallDetectRange);

    	if (hit.collider != null && (Input.GetKey("d") || Input.GetKey("right"))) {
    		if (hit.collider.tag == "Ground") {
    			ChangeGravity(-90, -1);
    		}
    	}
    }

    // Check if the player is on the gorund and update the grounded variable accordingly
    void CheckForGround () {
    	bool setGround = false;
    	ContactPoint2D[] contacts = new ContactPoint2D[10];

    	int count = mainCollider.GetContacts(contacts);

    	foreach (ContactPoint2D contact in contacts) {
    		if (contact.collider == null) {
    			continue;
    		}

    		pointer.transform.LookAt(contact.point);

    		float angle = FixAngle(pointer.transform.eulerAngles.x);

    		if (angle > 45 && angle < 135) {
    			setGround = true;
    		}
    	}

    	grounded = setGround;

    	if (jumpPhase > 1 && setGround) {
    		jumping = false;
    		jumpPhase = 0;
    	}
    }

    // Fix an angle so it is always between 0 and 360 degrees
    float FixAngle (float angle) {
    	while (angle < 0) {
    		angle += 360;
    	}

    	while (angle >= 360) {
    		angle -= 360;
    	}

    	return angle;
    }

    // Shift the direction of gravity
    void ChangeGravity (float amount, float direction) {
    	gravityAngle = FixAngle(gravityAngle + amount);
    	gravityDirection = direction;

    	switch (gravityAngle) {
    		case 0:
    			mainGrid.GetComponentInChildren<TileParticles>().RegenParticles(TileParticles.TileSideType.Top);
    		break;

    		case 90:
    			mainGrid.GetComponentInChildren<TileParticles>().RegenParticles(TileParticles.TileSideType.Left);
    		break;

    		case 180:
    			mainGrid.GetComponentInChildren<TileParticles>().RegenParticles(TileParticles.TileSideType.Bottom);
    		break;

    		case 270:
    			mainGrid.GetComponentInChildren<TileParticles>().RegenParticles(TileParticles.TileSideType.Right);
    		break;
    	}
    }

    void Animate (string animation) {
    	animator.ResetTrigger("Idle");
    	animator.ResetTrigger("Left");
    	animator.ResetTrigger("Right");

    	animator.SetTrigger(animation);
    }
}















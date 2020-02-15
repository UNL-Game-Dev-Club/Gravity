using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour {
	private Rigidbody2D rb;
    private CapsuleCollider2D enemyCollider;
    private Vector2 velocity;

    public bool grounded;
    public float moveSpeed;
    public bool moveRight;
    public float enemyRadius;
    public bool colliding;

    void Start() {
    	rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<CapsuleCollider2D>();

        enemyRadius = enemyCollider.size.x / 2F;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckCollisions();

        if (grounded)
        {
            velocity.x = moveRight ? moveSpeed : -1 * Mathf.Abs(moveSpeed);
            // transform.Translate(velocity * Time.deltaTime);

            rb.velocity = new Vector2(velocity.x, rb.velocity.y);
        }

        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    void CheckCollisions() {
        // Checks for floor with ledges
        colliding = false;
        Collider2D[] floorHitsLeft = Physics2D.OverlapCircleAll(new Vector2(transform.position.x - enemyRadius - 0.2f, transform.position.y - 1.5f), 0.01f);
        Collider2D[] floorHitsRight = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + enemyRadius + 0.2f, transform.position.y - 1.5f), 0.01f);

        float horizontalCheckDistance = moveRight ? enemyRadius : -enemyRadius;
        Collider2D[] sideHits = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + horizontalCheckDistance, transform.position.y), 0.01f);
        
        if (floorHitsRight.Length == 0 && floorHitsLeft.Length == 0)
            grounded = false;
        else
            grounded = true;

        if (floorHitsLeft.Length == 0 && grounded) {
        	moveRight = true;

        	return;
        }

        if (floorHitsRight.Length == 0 && grounded) {
        	moveRight = false;

        	return;
        }

        foreach (Collider2D collider2D in sideHits) {
        	if (collider2D.tag != "Player" && collider2D != enemyCollider) {
        		colliding = true;
                moveRight = !moveRight;

                break;
        	}
        }
    }
}
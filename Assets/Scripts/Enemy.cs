using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public enum EnemyType {
		Normal,
		WallCrawl
	}

	private Rigidbody2D rb;
    private CapsuleCollider2D enemyCollider;
    private Vector2 velocity;

    public EnemyType type;

    public bool grounded;
    public float moveSpeed;
    public bool moveRight;
    public float enemyRadius;
    public bool colliding;

    public GameObject[] barrels;

    void Start() {
    	rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<CapsuleCollider2D>();

        enemyRadius = enemyCollider.size.x / 2F;
    }

    // Update is called once per frame
    void Update () {
        CheckCollisions();

        if (grounded) {
            Move();
        }
        else if (type == EnemyType.WallCrawl) {
        	Vector2 upVector = new Vector2(transform.up.x, transform.up.y);
        	rb.velocity = upVector * -5;
        }
    }

    void Move () {
    	velocity.x = moveRight ? moveSpeed : -1 * Mathf.Abs(moveSpeed);

    	switch (type) {
    		case EnemyType.Normal:
    			rb.velocity = new Vector2(velocity.x, rb.velocity.y);

    			transform.eulerAngles = new Vector3(0, 0, 0);
    		break;

    		case EnemyType.WallCrawl:
    			Vector2 upVector = new Vector2(transform.up.x, transform.up.y);
    			Vector2 rightVector = new Vector2(transform.right.x, transform.right.y);
    			rb.velocity = (rightVector * velocity.x) + (upVector * -5);

    		break;
    	}
    }

    void CheckCollisions () {
    	Vector2 position = new Vector2(transform.position.x, transform.position.y);
    	Vector2 upVector = new Vector2(transform.up.x, transform.up.y);
    	Vector2 rightVector = new Vector2(transform.right.x, transform.right.y);
    	int backDirection = moveRight ? -1 : 1;

        // Checks for floor with ledges
        colliding = false;
        // Collider2D[] floorHitsLeft = Physics2D.OverlapCircleAll(new Vector2(transform.position.x - enemyRadius - 0.2f, transform.position.y - 1.5f), 0.01f);
        // Collider2D[] floorHitsRight = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + enemyRadius + 0.2f, transform.position.y - 1.5f), 0.01f);

        Collider2D[] floorHitsLeft = Physics2D.OverlapCircleAll(position + (rightVector * -(enemyRadius + 0.2f)) + (upVector * -(enemyRadius + 0.2f)), 0.01f);
        Collider2D[] floorHitsCenter = Physics2D.OverlapCircleAll(position + (rightVector * backDirection * (enemyRadius - 0.2f)) + (upVector * -(enemyRadius + 0.2f)), 0.01f);
        Collider2D[] floorHitsRight = Physics2D.OverlapCircleAll(position + (rightVector * (enemyRadius + 0.2f)) + (upVector * -(enemyRadius + 0.2f)), 0.01f);

        barrels[0].transform.position = position + (rightVector * -enemyRadius) + (upVector * -(enemyRadius + 0.2f));
        barrels[1].transform.position = position + (rightVector * enemyRadius) + (upVector * -(enemyRadius + 0.2f));
        barrels[2].transform.position = position + (rightVector * backDirection * (enemyRadius - 0.2f)) + (upVector * -(enemyRadius + 0.2f));

        float horizontalCheckDistance = moveRight ? enemyRadius : -enemyRadius;
        // Collider2D[] sideHits = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + horizontalCheckDistance, transform.position.y), 0.01f);
        Collider2D[] sideHits = Physics2D.OverlapCircleAll(position + (rightVector * horizontalCheckDistance), 0.01f);
        barrels[2].transform.position = position + (rightVector * horizontalCheckDistance);

        
        if (floorHitsRight.Length == 0 && floorHitsLeft.Length == 0)
            grounded = false;
        else
            grounded = true;

        if (type == EnemyType.WallCrawl) {
        	if (floorHitsLeft.Length == 0 && floorHitsCenter.Length == 0 && grounded && !moveRight) {
        		transform.RotateAround(position + (upVector * -(enemyRadius + 0.2f)), transform.forward, 90);
        		grounded = true;

        		return;
        	}

        	if (floorHitsRight.Length == 0 && floorHitsCenter.Length == 0 && grounded && moveRight) {
        		transform.RotateAround(position + (upVector * -(enemyRadius + 0.2f)), transform.forward, -90);
        		grounded = true;

        		return;
        	}
        }

        if (floorHitsLeft.Length == 0 && grounded && type != EnemyType.WallCrawl) {
        	moveRight = true;

        	return;
        }

        if (floorHitsRight.Length == 0 && grounded && type != EnemyType.WallCrawl) {
        	moveRight = false;

        	return;
        }

        foreach (Collider2D collider2D in sideHits) {
        	if (collider2D.tag != "Player" && collider2D != enemyCollider) {
        		OnSideCollision();

                break;
        	}
        }
    }

    // Called whenever the enemy detects a side collision
    void OnSideCollision () {
    	switch (type) {
    		case EnemyType.Normal:
    			colliding = true;
                moveRight = !moveRight;
    		break;

    		case EnemyType.WallCrawl:
    			if (moveRight) {
    				transform.Rotate(0, 0, 90);
    			}
    			else {
    				transform.Rotate(0, 0, -90);
    			}
    		break;
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
}












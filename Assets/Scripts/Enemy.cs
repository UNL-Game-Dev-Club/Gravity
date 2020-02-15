using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private CapsuleCollider2D enemyCollider;
    private Vector2 velocity;
    public bool grounded;
    public float moveSpeed;
    public bool moveRight;
    public float enemyRadius;
    public bool colliding;

    void Start() {
        enemyCollider = GetComponent<CapsuleCollider2D>();
        enemyRadius = enemyCollider.size.x / 2F;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckCollisions();

        if (grounded && !colliding)
        {
            velocity.x = moveRight ? moveSpeed : -1 * Mathf.Abs(moveSpeed);
            transform.Translate(velocity * Time.deltaTime);
        }


    }


    void CheckCollisions() {
        // Checks for floor with ledges
        colliding = false;
        Collider2D[] floorHitsLeft = Physics2D.OverlapCircleAll(new Vector2(transform.position.x - enemyRadius, transform.position.y - 1.5f), 0.01f);
        Collider2D[] floorHitsRight = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + enemyRadius, transform.position.y - 1.5f), 0.01f);
        float horizontalCheckDistance = moveRight ? enemyRadius : -enemyRadius;
        Collider2D[] sideHits = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + horizontalCheckDistance, transform.position.y), 0.01f);
        
        if (floorHitsRight.Length == 0 && floorHitsLeft.Length == 0)
            grounded = false;
        else
            grounded = true;
        
        if (floorHitsLeft.Length == 0 || floorHitsRight.Length == 0 || sideHits.Length != 0)
        {
            if (!sideHits.Any(hit => (hit.gameObject.tag == "Player" || hit == enemyCollider)))
            {
               if(grounded)
                {
                    Debug.Log($"Floor Hit Left {floorHitsLeft.Length}, Floor Hit Right {floorHitsRight.Length}, Sides {sideHits.Length}");
                    colliding = true;
                    moveRight = !moveRight;
                }
            }

        }

    }
}

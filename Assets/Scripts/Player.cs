using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float walkSpeed = 2f;

    public Rigidbody2D rb;
    public CollisionChecker coll;

    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 15f;

    void Start() {
        coll = GetComponent<CollisionChecker>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        ProcessWalkInput();

        SmoothFall();
    }

    void Update() {

    }

    public void ProcessWalkInput() {
        float x = Input.GetAxis("Horizontal");

        //Vector2 newPos = new Vector2(x * walkSpeed * Time.fixedDeltaTime, 0);
        //rb.MovePosition(rb.position + newPos);

        rb.velocity = new Vector2(x * walkSpeed, rb.velocity.y);
    }

    public void SmoothFall() {
        rb.gravityScale = 3;

        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}

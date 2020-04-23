using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {
    private Rigidbody2D rb;

    public Player player;
    public Walnut walnut;
    private SpriteRenderer spr;

    public float moveSpeed = 1f;
    public int damage = 1;
    //public float velocityNeededToDie = 0.5f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        walnut = FindObjectOfType<Walnut>();
        spr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() {
        WalkTowardsPlayer();
    }

    private void WalkTowardsPlayer() {
        if (player) {
            Vector3 dir = (player.transform.position - transform.position);

            rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);

            if (rb.velocity.x > 0) {
                spr.flipX = true;
            }
            else {
                spr.flipX = false;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && player.canBeHit) {
            player.TakeDamage(damage);
        }

        if (collision.gameObject.tag == "Walnut" && walnut.canBeHit) {
            //if (collision.rigidbody.velocity.magnitude > velocityNeededToDie) {
            walnut.TakeDamage(damage);
            Destroy(transform.gameObject);
            //}
        }
    }
}

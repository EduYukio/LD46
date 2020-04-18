using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody2D rb;
    private CollisionChecker collisionChecker;
    private SpriteRenderer spr;

    public BoxCollider2D playerCollisionBox;
    public GameObject walnut;
    public GameObject walnutSprite;


    public float walkSpeed = 2f;
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 15f;
    public float xLaunchSpeed = 5f;
    public float yLaunchSpeed = 1.5f;

    public Vector2 rightHandPosition = new Vector2(1.522f, 0.38f);
    public Vector2 leftHandPosition = new Vector2(-1.522f, 0.38f);


    //flags
    public bool isNearWalnut = false;
    public bool walnutEquipped = false;
    public bool canPickOrDrop = true;



    private float xInput;

    private float xNormalOffset = 0;
    private float xExtendedOffset = 0.55f;

    private float xNormalSize = 2;
    private float xExtendedSize = 3.1f;

    private float yOffset;
    private float ySize;

    void Start() {
        collisionChecker = GetComponent<CollisionChecker>();
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();

        yOffset = playerCollisionBox.offset.y;
        ySize = playerCollisionBox.size.y;
    }

    void FixedUpdate() {
        Walk();
        SmoothFall();
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.E)) {
            canPickOrDrop = true;
        }

        ProcessPickAndDropInput();
        ProcessWalkInput();
        ProcessLaunchInput();
    }

    public void ProcessWalkInput() {
        xInput = Input.GetAxis("Horizontal");
    }

    public void Walk() {
        //Vector2 newPos = new Vector2(x * walkSpeed * Time.fixedDeltaTime, 0);
        //rb.MovePosition(rb.position + newPos);

        rb.velocity = new Vector2(xInput * walkSpeed, rb.velocity.y);

        if ((xInput > 0 && spr.flipX) || (xInput < 0 && !spr.flipX)) {
            FlipPlayer();
        }
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

    public void ProcessPickAndDropInput() {
        if (Input.GetKeyDown(KeyCode.E) && canPickOrDrop) {
            if (walnutEquipped) {
                DropWalnut();
            }
            else if (isNearWalnut) {
                PickWalnut();
            }
            canPickOrDrop = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Walnut") {
            isNearWalnut = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Walnut") {
            isNearWalnut = false;
        }
    }

    public void DropWalnut() {
        walnut.SetActive(true);
        walnutSprite.SetActive(false);

        playerCollisionBox.offset = new Vector2(xNormalOffset, yOffset);
        playerCollisionBox.size = new Vector2(xNormalSize, ySize);

        walnut.transform.position = walnutSprite.transform.position;
        walnut.transform.rotation = walnutSprite.transform.rotation;

        walnutEquipped = false;
    }

    public void PickWalnut() {
        walnut.SetActive(false);
        walnutSprite.SetActive(true);

        playerCollisionBox.offset = new Vector2(xExtendedOffset, yOffset);
        playerCollisionBox.size = new Vector2(xExtendedSize, ySize);

        walnutEquipped = true;
    }

    public void FlipPlayer() {
        spr.flipX = !spr.flipX;
        walnutSprite.transform.localPosition = new Vector2(-walnutSprite.transform.localPosition.x, walnutSprite.transform.localPosition.y);
        //iverter o imã também
    }

    public void ProcessLaunchInput() {
        if (walnutEquipped && Input.GetKeyDown(KeyCode.Mouse0)) {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = (worldMousePosition - transform.position).normalized;

            float playerDistance = (worldMousePosition - transform.position).magnitude;
            float walnutDistance = (worldMousePosition - walnutSprite.transform.position).magnitude;

            if (walnutDistance > playerDistance) {
                FlipPlayer();
            }

            DropWalnut();
            Rigidbody2D walnutRb = walnut.GetComponent<Rigidbody2D>();
            walnutRb.velocity = new Vector3(dir.x * xLaunchSpeed, dir.y * yLaunchSpeed, 0);
            walnutRb.angularVelocity = 20f;
        }
    }
}

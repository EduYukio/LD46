using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    private Rigidbody2D rb;
    private CollisionChecker collisionChecker;
    private SpriteRenderer spr;

    public BoxCollider2D playerCollisionBox;
    public GameObject walnut;
    public GameObject walnutSprite;
    public Walnut walnutScript;

    public GameObject magnetSprite;
    public HeartArray playerHearts;

    public int maxHealth = 3;
    public int health = 3;

    public float walkSpeed = 2f;
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 15f;
    public float magnetSpeed = 1f;
    public float xLaunchSpeed = 5f;
    public float yLaunchSpeed = 1.5f;

    public Vector2 rightHandPosition = new Vector2(1.522f, 0.38f);
    public Vector2 leftHandPosition = new Vector2(-1.522f, 0.38f);


    //flags
    public bool isNearWalnut = false;
    public bool walnutEquipped = false;
    public bool canPickOrDrop = true;
    public bool isUsingMagnet = false;
    public bool dead = false;



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
        walnutScript = walnut.GetComponent<Walnut>();

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

        if (!dead && !walnutScript.dead) {
            ProcessPickAndDropInput();
            ProcessWalkInput();
            ProcessLaunchInput();
            ProcessMagnetInput();
        }
    }






    public void ProcessWalkInput() {
        xInput = Input.GetAxis("Horizontal");
    }

    public void Walk() {
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

            if (isUsingMagnet) {
                PickWalnut();
            }
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
        Vector2 walnutLocalPos = walnutSprite.transform.localPosition;
        Vector2 magnetLocalPos = magnetSprite.transform.localPosition;

        walnutSprite.transform.localPosition = new Vector2(-walnutLocalPos.x, walnutLocalPos.y);

        magnetSprite.transform.localPosition = new Vector2(-magnetLocalPos.x, magnetLocalPos.y);
        magnetSprite.transform.localEulerAngles = new Vector3(0, 0, -magnetSprite.transform.localEulerAngles.z);
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

    public void ProcessMagnetInput() {
        if (!walnutEquipped && Input.GetButton("Fire2")) {
            isUsingMagnet = true;

            Vector3 dir = (transform.position - walnut.transform.position).normalized;
            Rigidbody2D walnutRb = walnut.GetComponent<Rigidbody2D>();
            walnutRb.velocity += new Vector2(dir.x * magnetSpeed, dir.y * magnetSpeed);
        }
        else {
            isUsingMagnet = false;
        }
    }

    public void TakeDamage(int damage) {
        health -= damage;
        playerHearts.RemoveHeart();

        if (health <= 0) {
            Die();
        }
    }

    public void Die() {
        StartCoroutine(ReloadSceneDelay(1.5f));

        //fazer algo bonito, pode ser alpha virando 0, tela piscando, qualqeur coisa
    }

    IEnumerator ReloadSceneDelay(float waitTime) {
        dead = true;
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Regenerate() {
        health = maxHealth;
        playerHearts.RestoreAllHearts();
    }
}

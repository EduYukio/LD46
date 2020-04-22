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
    public Animator playerAnimator;

    public GameObject magnetSprite;
    public HeartArray playerHearts;

    public AudioSource hurtSound;
    public AudioSource imaSound;
    public AudioSource blipSound;
    public AudioSource errorSound;

    public int maxHealth = 3;
    public int health = 3;

    public float walkSpeed = 2f;
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 15f;
    public float magnetSpeed = 1f;
    public float xLaunchSpeed = 5f;
    public float yLaunchSpeed = 1.5f;
    public float immunityTime = 2f;


    //flags
    public bool isNearWalnut = false;
    public bool walnutEquipped = false;
    public bool canPickOrDrop = true;
    public bool isUsingMagnet = false;
    public bool dead = false;
    public bool canThrow = true;
    public bool canBeHit = true;
    public bool hasMagnet = false;

    private float xInput;

    private float yOffset;
    private float ySize;
    private Jump jumpScript;

    void Start() {
        collisionChecker = GetComponent<CollisionChecker>();
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        walnutScript = walnut.GetComponent<Walnut>();
        jumpScript = GetComponent<Jump>();

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

        if (isUsingMagnet && !imaSound.isPlaying) {
            imaSound.Play();
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
            if (walnutEquipped && canThrow) {
                DropWalnut();
            }
            else if (walnutEquipped && !canThrow) {
                errorSound.Play();
            }
            else if (!walnutEquipped && isNearWalnut) {
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

    private void OnTriggerStay2D(Collider2D collision) {
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
        walnutScript.canBeHit = true;

        walnutSprite.SetActive(false);

        walnut.transform.position = walnutSprite.transform.position;
        walnut.transform.rotation = walnutSprite.transform.rotation;

        walnutEquipped = false;
        blipSound.Play();
    }

    public void PickWalnut() {
        walnutScript.canBeHit = false;
        walnut.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        walnut.SetActive(false);

        walnutSprite.SetActive(true);

        walnutEquipped = true;

        if (!walnutScript.walnutHearts_UI.activeSelf) {
            walnutScript.walnutHearts_UI.SetActive(true);
            walnutScript.walnutFace_UI.SetActive(true);
        }
        blipSound.Play();

        StartCoroutine(DisableJumpTemporarily(0.2f));
    }

    IEnumerator DisableJumpTemporarily(float waitTime) {
        jumpScript.enabled = false;
        yield return new WaitForSeconds(waitTime);
        jumpScript.enabled = true;
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && walnutEquipped) {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = (worldMousePosition - transform.position).normalized;

            float playerDistance = (worldMousePosition - transform.position).magnitude;
            float walnutDistance = (worldMousePosition - walnutSprite.transform.position).magnitude;

            bool needToFlip = walnutDistance > playerDistance;

            if (needToFlip) {
                FlipPlayer();
                if (canThrow) {
                    //pode dar ruim ao virar e entrar na parede
                    StartCoroutine(WaitToFlipAndCheckIfCanThrow(0.04f, dir, spr.flipX));
                }
                else {
                    //se precisa virar e nao pode tacar, quer dizer que ja ta na parede e quer
                    //tacar pro outro lado, entao nao precisa checar (só dá ruim se for
                    //em um lugar mto pequeno, evita)
                    ThrowWalnut(dir);
                }
            }
            else if (canThrow) {
                ThrowWalnut(dir);
            }
            else {
                errorSound.Play();
            }
        }
    }

    IEnumerator WaitToFlipAndCheckIfCanThrow(float waitTime, Vector3 dir, bool playerFlipX) {
        yield return new WaitForSeconds(waitTime);
        if (playerFlipX != spr.flipX) {
            FlipPlayer();
        }

        if (canThrow) {
            ThrowWalnut(dir);
        }
        else {
            errorSound.Play();
        }
    }

    public void ThrowWalnut(Vector3 dir) {
        DropWalnut();
        Rigidbody2D walnutRb = walnut.GetComponent<Rigidbody2D>();
        walnutRb.velocity = new Vector3(dir.x * xLaunchSpeed, dir.y * yLaunchSpeed, 0);
        walnutRb.angularVelocity = 20f;
    }

    public void ProcessMagnetInput() {
        if (hasMagnet && !walnutEquipped && Input.GetButton("Fire2")) {
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
        hurtSound.Play();

        canBeHit = false;
        playerAnimator.SetBool("IsBeingHit", true);
        StartCoroutine(BlinkingAnimationTimer(immunityTime));

        if (health <= 0) {
            Die();
        }
    }

    IEnumerator BlinkingAnimationTimer(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        playerAnimator.SetBool("IsBeingHit", false);
        canBeHit = true;
    }

    public void Die() {
        StartCoroutine(ReloadSceneDelay(0.5f));

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

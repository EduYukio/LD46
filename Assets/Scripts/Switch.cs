using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {
    public Gate gateConnected;
    public bool playerIsOnSwitch = false;
    public bool walnutIsOnSwitch = false;

    public bool switchState = false;

    public BoxCollider2D coll;
    public SpriteRenderer sprRender;

    public Sprite spriteOn;
    public Sprite spriteOff;

    public AudioSource blipSound;
    public AudioSource blipClosingSound;


    public bool waitingToStabilize = false;


    void Start() {
        coll = GetComponent<BoxCollider2D>();
        sprRender = GetComponent<SpriteRenderer>();

        sprRender.sprite = spriteOff;
    }

    void Update() {
        CheckForPresence();

        if (switchState && gateConnected.closed) {
            gateConnected.Open();
            blipSound.Play();
        }
        else if(!switchState && !gateConnected.closed) {
            gateConnected.Close();
            blipClosingSound.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            playerIsOnSwitch = true;
        }

        if (collision.tag == "Walnut") {
            walnutIsOnSwitch = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            playerIsOnSwitch = false;
        }

        if (collision.tag == "Walnut") {
            walnutIsOnSwitch = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (transform.tag == "PressurePlate") {
            if (collision.gameObject.tag == "Player") {
                playerIsOnSwitch = false;
            }

            if (collision.gameObject.tag == "Walnut") {
                walnutIsOnSwitch = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (transform.tag == "PressurePlate") {
            if (collision.gameObject.tag == "Player") {
                playerIsOnSwitch = true;
            }

            if (collision.gameObject.tag == "Walnut") {
                walnutIsOnSwitch = true;
            }
        }
    }

    private void CheckForPresence() {
        //qualquer coisa pisou no botao off
        //ativa e nao volta
        if (transform.tag == "Button" && !switchState) {
            if (playerIsOnSwitch || walnutIsOnSwitch) {
                ButtonOn();
            }
        }
        //player pisou na lever off, deixa preparada pra receber "E"
        //ativa e nao volta
        else if (transform.tag == "Lever" && !switchState) {
            if (playerIsOnSwitch) {
                LeverOn();
            }
        }
        //qualquer coisa pisou na pressure off, abaixa ela e põe on
        //se parou de pisar, levanta ela e põe off
        else if (transform.tag == "PressurePlate") {
            if (playerIsOnSwitch || walnutIsOnSwitch) {
                PressurePlateOn();
            }
            else {
                PressurePlateOff();
            }
        }
    }

    private void ButtonOn() {
        switchState = true;
        sprRender.sprite = spriteOn;
        blipSound.Play();
    }

    private void LeverOn() {
        if (Input.GetKeyDown(KeyCode.E)) {
            switchState = true;
            sprRender.sprite = spriteOn;
            blipSound.Play();
        }
    }

    private void PressurePlateOn() {
        if (!waitingToStabilize) {
            waitingToStabilize = true;

            switchState = true;
            sprRender.sprite = spriteOn;

            coll.size = new Vector2(0.48f, 0.13f);
            coll.offset = new Vector2(0, -0.13f);

            StartCoroutine(WaitForPlateToStabilize(0.8f));
        }
    }

    private void PressurePlateOff() {
        if (!waitingToStabilize) {
            waitingToStabilize = true;

            switchState = false;
            sprRender.sprite = spriteOff;

            coll.size = new Vector2(0.48f, 0.4f);
            coll.offset = new Vector2(0, 0);

            StartCoroutine(WaitForPlateToStabilize(0.8f));
        }
    }

    IEnumerator WaitForPlateToStabilize(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        waitingToStabilize = false;
    }
}

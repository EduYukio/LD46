using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour {
    public Player player;

    public bool playerIsNear = false;

    void Start() {

    }

    void Update() {
        if (playerIsNear && Input.GetKeyDown(KeyCode.E)) {
            player.hasMagnet = true;
            player.magnetSprite.SetActive(true);
            Destroy(transform.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            playerIsNear = false;
        }
    }
}

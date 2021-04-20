using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Magnet : MonoBehaviour {
    public Player player;

    public bool playerIsNear = false;

    public UnityEvent magnetPicked;

    void Update() {
        if (playerIsNear && Input.GetKeyDown(KeyCode.E)) {
            player.blipSound.Play();
            player.hasMagnet = true;
            player.magnetSprite.SetActive(true);
            Destroy(transform.gameObject);
            magnetPicked.Invoke();
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

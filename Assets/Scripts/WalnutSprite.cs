using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalnutSprite : MonoBehaviour {
    public Player player;

    void Start() {

    }

    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Ground" || collision.tag == "Gate") {
            player.canThrow = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Ground" || collision.tag == "Gate") {
            player.canThrow = true;
        }
    }
}

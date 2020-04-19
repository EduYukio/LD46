using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harm : MonoBehaviour {
    public int damage = 1;

    public Player player;
    public Walnut walnut;

    void Start() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.isTrigger) {
            if (collision.tag == "Player") {
                player.TakeDamage(damage);
            }
            else if(collision.tag == "Walnut") {
                walnut.TakeDamage(damage);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walnut : MonoBehaviour {
    public int health = 3;

    public HeartArray walnutHearts;

    void Start() {
        for (int i = 0; i < health; i++) {
            walnutHearts.CreateHeart();
        }
    }

    void FixedUpdate() {
    }

    public void TakeDamage(int damage) {
        health -= damage;
        walnutHearts.RemoveHeart();

        if (health <= 0) {
            Die();
        }
    }

    public void Die() {
        Debug.Log("nut dead =(");
    }
}
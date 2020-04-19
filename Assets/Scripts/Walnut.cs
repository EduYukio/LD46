using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Walnut : MonoBehaviour {
    public int maxHealth = 5;
    public int health = 5;

    public bool dead = false;

    public HeartArray walnutHearts;

    void Start() {
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
        StartCoroutine(ReloadSceneDelay(1.5f));

        //fazer algo bonito, pode ser alpha virando 0, tela piscando, qualqeur coisa
        // mensagem no player falando OH NO
    }

    IEnumerator ReloadSceneDelay(float waitTime) {
        dead = true;
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Regenerate() {
        health = maxHealth;
        walnutHearts.RestoreAllHearts();
    }
}
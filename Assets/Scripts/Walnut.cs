using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Walnut : MonoBehaviour {
    public int maxHealth = 5;
    public int health = 5;

    public bool dead = false;
    public bool canBeHit = true;
    public float immunityTime = 2f;

    public HeartArray walnutHearts;
    public Animator walnutAnimator;

    void Start() {
    }

    void FixedUpdate() {
    }

    public void TakeDamage(int damage) {
        health -= damage;
        walnutHearts.RemoveHeart();

        canBeHit = false;
        walnutAnimator.SetBool("IsBeingHit", true);
        StartCoroutine(BlinkingAnimationTimer(immunityTime));

        if (health <= 0) {
            Die();
        }
    }

    IEnumerator BlinkingAnimationTimer(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        walnutAnimator.SetBool("IsBeingHit", false);
        canBeHit = true;
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
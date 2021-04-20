using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Walnut : MonoBehaviour {
    public int maxHealth = 3;
    public int health = 3;

    public bool dead = false;
    public bool canBeHit = true;
    public float immunityTime = 2f;

    public GameObject walnutHearts_UI;
    public GameObject walnutFace_UI;
    public HeartArray walnutHearts;
    public Animator walnutAnimator;
    public AudioSource hurtSound;

    void Start() {
        if (SceneManager.GetActiveScene().name == "Tutorial") {
            walnutHearts_UI.SetActive(false);
            walnutFace_UI.SetActive(false);
        }
    }

    public void TakeDamage(int damage) {
        health -= damage;
        walnutHearts.RemoveHeart();

        hurtSound.Play();
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
        StartCoroutine(ReloadSceneDelay(0.5f));
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartArray : MonoBehaviour {

    void Start() {
    }

    public void RemoveHeart() {
        float xMax = -999999;
        Transform lastHeart = null;
        foreach (Transform child in transform) {
            if (child.gameObject.activeSelf && child.position.x > xMax) {
                lastHeart = child;
            }
        }

        if (lastHeart != null) {
            lastHeart.gameObject.SetActive(false);
        }
    }

    public void RestoreAllHearts() {
        foreach (Transform child in transform) {
            if (!child.gameObject.activeSelf) {
                child.gameObject.SetActive(true);
            }
        }
    }
}
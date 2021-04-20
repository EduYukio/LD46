using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {
    public bool playerIsNear = false;
    public Player player;
    public string NextLevel;

    void Start() {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update() {
        if (playerIsNear && player.walnutEquipped) {
            Player.checkpointPos = null;
            SceneManager.LoadScene(NextLevel);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        playerIsNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        playerIsNear = false;
    }
}

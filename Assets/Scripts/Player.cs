using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float walkSpeed = 3f;

    void Start() {

    }
    void FixedUpdate() {
        ProcessWalkInput();

    }

    // Update is called once per frame
    void Update() {

    }

    public void ProcessWalkInput() {
        float x = Input.GetAxisRaw("Horizontal");

        //if (x == 0 && y == 0) {
        //    playerAnimator.SetBool("IsWalking", false);
        //}
        //else {
        //    playerAnimator.SetBool("IsWalking", true);
        //}

        transform.Translate(x * Time.fixedDeltaTime * walkSpeed, 0, 0);
    }
}

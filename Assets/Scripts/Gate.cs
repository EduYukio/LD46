using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {
    public bool closed = true;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Open() {
        closed = false;
        gameObject.SetActive(false);
    }

    public void Close() {
        closed = true;
        gameObject.SetActive(true);
    }
}

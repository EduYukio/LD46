using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartArray : MonoBehaviour {
    //public GameObject characterImage;
    //public GameObject heartImage;
    //public float spaceBetweenHearts = 20f;

    //private List<GameObject> heartArray = new List<GameObject>();
    //private Vector2 charPos;

    void Start() {
        //for (int i = 0; i < length; i++) {

        //}
        //charPos = characterImage.transform.position;
        //Debug.Log(charPos.y);
    }

    //public void CreateHeart(float yOffset) {
    //    float offsetFromCharImg = 60f;
    //    float space = offsetFromCharImg + spaceBetweenHearts * heartArray.Count;
    //    //Vector3 basePos = transform.parent.transform.position;
    //    //Vector3 basePos = new Vector3(-360f, 185f + yOffset, 0);
    //    //Vector3 basePos = new Vector3(0,0, 0);
    //    //Vector3 basePos = charPos;
    //    //Vector3 heartPosition = basePos + new Vector3(space, 0, 0);
    //    Vector3 heartPosition = charPos;

    //    GameObject newHeart = Instantiate(heartImage, transform.position, Quaternion.identity);
    //    heartArray.Add(newHeart);
    //    //newHeart.transform.parent = this.gameObject.transform;
    //    newHeart.transform.SetParent(transform, false);
    //    //newHeart.transform.localScale = new Vector3(0.5f, 0.5f, 1);
    //    newHeart.transform.position = transform.position;
    //    newHeart.transform.position = charPos;

    //    //newHeart.transform.position += new Vector3(-170, 105 + yOffset, 0);
    //    //newHeart.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    //}

    public void RemoveHeart() {
        //foreach (child in transform.childCount
        float xMax = -999999;
        Transform lastHeart = null;
        foreach (Transform child in transform) {
            if (child.position.x > xMax) {
                lastHeart = child;
            }
        }

        if (lastHeart != null) {
            Destroy(lastHeart.gameObject);
        }

        //if (heartArray.Count > 0) {
        //    int heartIndex = heartArray.Count - 1;
        //    GameObject heartObj = heartArray[heartIndex];
        //    heartArray.RemoveAt(heartIndex);
        //    Destroy(heartObj);
        //}
    }
}
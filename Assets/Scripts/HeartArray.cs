using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartArray : MonoBehaviour {
    public GameObject characterImage;
    public GameObject heartImage;
    public float spaceBetweenHearts = 20f;

    private List<GameObject> heartArray = new List<GameObject>();
    private Vector2 charPos;

    void Start() {
        charPos = characterImage.transform.position;
    }

    public void CreateHeart() {
        float offsetFromCharImg = 60f;
        float space = offsetFromCharImg + spaceBetweenHearts * heartArray.Count;
        Vector3 heartPosition = new Vector3(charPos.x + space, charPos.y, 0);

        GameObject newHeart = Instantiate(heartImage, heartPosition, Quaternion.identity);
        heartArray.Add(newHeart);
        //newHeart.transform.parent = this.gameObject.transform;
        newHeart.transform.SetParent(this.gameObject.transform);
    }

    public void RemoveHeart() {
        if (heartArray.Count > 0) {
            int heartIndex = heartArray.Count - 1;
            GameObject heartObj = heartArray[heartIndex];
            heartArray.RemoveAt(heartIndex);
            Destroy(heartObj);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamOfLight : MonoBehaviour {
    public Player player;
    public Walnut walnut;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.isTrigger) {
            if (collision.tag == "Player") {
                player.Regenerate();
                if (player.walnutEquipped) {
                    walnut.Regenerate();
                }
            }
            else if (collision.tag == "Walnut") {
                walnut.Regenerate();
            }
        }
    }


}

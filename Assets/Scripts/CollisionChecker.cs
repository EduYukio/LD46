using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour {
    [Header("Layers")]
    public LayerMask groundLayer;
    public bool onGround;

    [Header("Collision")]

    public Vector2 collisionSize = new Vector2(0.1f, 0.1f);
    public Vector2 bottomOffset;
    private Color debugCollisionColor = Color.red;

    void Update() {
        //onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, collisionSize, 0f, groundLayer);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset };

        //Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionSize);
        Gizmos.DrawWireCube((Vector2)transform.position + bottomOffset, collisionSize);
    }
}

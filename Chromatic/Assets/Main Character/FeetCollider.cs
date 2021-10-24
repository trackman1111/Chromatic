using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetCollider : MonoBehaviour
{
    private int jumpCounter;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        jumpCounter = 2;
        rb = transform.parent.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Resets jump for the collider off the feet

        jumpCounter = 2; 

    }

    public void DoubleJump()
    {
        print(jumpCounter);
        if (jumpCounter != 0)
        {

            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, 8), ForceMode2D.Impulse);
            jumpCounter--;
        }

    }
}

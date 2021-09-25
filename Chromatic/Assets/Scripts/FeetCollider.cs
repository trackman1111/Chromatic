using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetCollider : MonoBehaviour
{
    // Private Variables
    private int jumpCounter;
    private Rigidbody2D rb;
    

    // Start is called before the first frame update
    void Start()
    {
        // Calls Parent Rigidbody to add impulse, and sets jump to 2
        jumpCounter = 2;
        rb = transform.parent.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void Update()
    {
        // If a Space is inputted, call DoubleJump()
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoubleJump();
        }      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Resets jump for the collider of the feet
        jumpCounter = 2;
    }
      
    // DoubleJump 
    void DoubleJump()
    {
        if (jumpCounter != 0)
        {
            // Sets Y velocity to 0 to allow the same DoubleJump Impulse
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, 8), ForceMode2D.Impulse);
            jumpCounter--;
        }
    }
}

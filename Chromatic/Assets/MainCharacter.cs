using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed = 9.0f;
    public bool hasRed = false;
    public bool hasBlue = false;
    public bool hasYellow = false;
    public bool hasGreen = false;



    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), 0);
        transform.Translate(move * Time.deltaTime * moveSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "red")
        {
            hasRed = true;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "blue")
        {
            hasBlue = true;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "green")
        {
            hasGreen = true;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "yellow")
        {
            hasYellow = true;
            Destroy(collision.gameObject);
        }
    }
}

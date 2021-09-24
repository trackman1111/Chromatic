using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed = 9.0f;
    float gravscale;
    public float ClimbSpeed;
    public bool hasRed = false;
    public bool hasBlue = false;
    public bool hasYellow = false;
    public bool hasGreen = false;
    private bool climbing;
    public Sprite vine;
    public Tilemap background;
    private Vector2 move;



    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        gravscale = rb.gravityScale;
        climbing = false;
    }

    // Update is called once per frame
    void Update()
    {
        Sprite currentSprite = background.GetSprite(new Vector3Int((int)(transform.position.x), (int)transform.position.y, 0));
        float xSpeed = Input.GetAxis("Horizontal");
        float ySpeed = Input.GetAxis("Vertical");


        if (Input.GetKeyDown(KeyCode.Space) && !climbing)
        {
            rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        }
        if (currentSprite != null && currentSprite.Equals(vine) && !climbing)
        {
            climbing = true;
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0, 0);
        }
        else if (climbing && (currentSprite == null || !currentSprite.Equals(vine)))
        {
            rb.gravityScale = 1;
            climbing = false;
        }
        if (!climbing)
        {
            ySpeed = 0;
        }
        move = new Vector2(xSpeed, ySpeed);
        transform.Translate(move * Time.deltaTime * moveSpeed);

        
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    private int jumpCounter;
    private Rigidbody2D rb;
    private float moveSpeed = 15.0f;
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

    }


}

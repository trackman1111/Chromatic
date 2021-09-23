using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    Rigidbody2D rgb2d;
    float gravscale;
    public float ClimbSpeed;

    // Start is called before the first frame update
    void Start()
    {

        rgb2d = gameObject.GetComponent<Rigidbody2D>();
        gravscale = rgb2d.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Vine")
        {

            if (Input.GetKey(KeyCode.W))
            {
                //moves character up and removes gravity while holding 'W'
                rgb2d.gravityScale = 0;

                transform.Translate(Vector2.up * Time.deltaTime * ClimbSpeed);
            
            } else
            {
                //Puts gravity scale back to normal when not holding W
                rgb2d.gravityScale = gravscale;
            }

        }
    }


    //Puts gravity scale back to normal when leaving the vine
    private void OnTriggerExit2D(Collider2D collision)
    {
        rgb2d.gravityScale = gravscale;
    }


}

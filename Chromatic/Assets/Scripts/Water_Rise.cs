using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Rise : MonoBehaviour
{

    public bool rise;
    private GameObject water;
    public Vector3 waterpos;
    public float speed = 1;

    private Vector3 distance;

    // Start is called before the first frame update
    void Start()
    {
        water = gameObject;

        speed = speed * 0.01f;
        distance = new Vector3(waterpos.x - water.transform.position.x, waterpos.y - water.transform.position.y, waterpos.z);
        //Automatically moves to the z position so it moves from invisible to visible range
    }

    // Update is called once per frame
    void Update()
    {
        if (rise == true && waterpos.y >= water.transform.position.y)
        {
            //moves towards the goal position. Automatically moves to the z position so it moves from invisible to visible range
            water.transform.position = new Vector3(water.transform.position.x, water.transform.position.y, waterpos.z);

            water.transform.position += new Vector3(distance.x * speed, distance.y * speed, 0);
            print(new Vector3(distance.x * speed, distance.y * speed, distance.z));
        }
    }
}

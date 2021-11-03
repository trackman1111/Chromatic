using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Vector3 checkpoint;
    // Start is called before the first frame update
    void Start()
    {
        checkpoint = gameObject.transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<Health>().startpos = checkpoint;
    }
}

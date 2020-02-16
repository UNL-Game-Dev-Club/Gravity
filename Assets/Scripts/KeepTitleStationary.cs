using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepTitleStationary : MonoBehaviour
{
    public GameObject player;
    private bool running = false;
    void Awake()
    {
        running = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!running)
            return;

        if(Input.GetAxis("Horizontal") != 0 || Input.GetKey("space"))
        {
            foreach(Rigidbody2D rb in gameObject.GetComponentsInChildren<Rigidbody2D>())
            {
                rb.gravityScale = 1;
            }
            running = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveFoward : MonoBehaviour
{
    
    Rigidbody rb = null;

    public float multi = 100;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddRelativeForce(0, 0, Input.GetAxis("Vertical") * multi);
    }
}

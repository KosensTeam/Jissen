using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movecycle : MonoBehaviour
{
    float vector = 1;
    float up = 0;
    public float speed = 0;
    private Rigidbody rb;
    GameObject cycle;
    float x;
    float y;
    // Start is called before the first frame update
    void Start()
    {
        rb = GameObject.Find("default").GetComponent<Rigidbody>();
        //cycle = GameObject.Find("cycle2");
        //Vector3 tmp = cycle.transform.position;
        //cycle.transform.position = new Vector3(tmp.x, tmp.y, tmp.z);
    }

    // Update is called once per frame
    void Update()
    { 
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        Transform rbTransform = rb.transform;
        Vector3 worldAngle = new Vector3(0, 360, 0) - rbTransform.eulerAngles;

        float movex = (float)Math.Cos((worldAngle.y -180) * (Math.PI / 180)) * moveVertical * 5f;
        float movez = (float)Math.Sin((worldAngle.y -180)* (Math.PI / 180)) * moveVertical * 5f;
        Debug.Log(worldAngle.y);
        Vector3 force = new Vector3(movex, 0, movez);    // 力を設定
        rb.AddForce(force, ForceMode.Force);
        //rb.transform.position += force;
        rb.angularVelocity = new Vector3(0, moveHorizontal*3f, 0);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //入力を取得（横はx軸,縦はz軸）
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Rigidbody rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(x*speed, 0, z*speed);
    }

}

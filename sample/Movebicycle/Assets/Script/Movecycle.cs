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
    bool fly = false;
    public int itemnum = 0;
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

        if (fly == false)
        {
            //物体に接続している時
            float movex = (float)Math.Cos((worldAngle.y - 180) * (Math.PI / 180)) * moveVertical * 5f;
            float movez = (float)Math.Sin((worldAngle.y - 180) * (Math.PI / 180)) * moveVertical * 5f;
            //Debug.Log(worldAngle.y);
            Vector3 force = new Vector3(movex, 0, movez);    // 力を設定
            rb.AddForce(force, ForceMode.Force);
            //rb.transform.position += force;
            rb.angularVelocity = new Vector3(0, moveHorizontal * 3f, 0);
        }
        else
        {
            //物体から離れた時
            float movex = (float)Math.Cos((worldAngle.y - 180) * (Math.PI / 180)) * moveVertical * 5f;
            float movez = (float)Math.Sin((worldAngle.y - 180) * (Math.PI / 180)) * moveVertical * 5f + (float)Math.Sin((worldAngle.x) * (Math.PI / 180)) * moveVertical * 2f;
            float movey = (float)moveVertical * itemnum * 3f;
            //Debug.Log(worldAngle.y);
            Vector3 force = new Vector3(movex, movey, movez);    // 力を設定
            rb.AddForce(force, ForceMode.Force);
            rb.angularVelocity = new Vector3(-moveHorizontal * 1f, moveHorizontal * 0.5f, 0);
            Debug.Log("z:" + rb.position.z);
        }

    }

    // 玉が他のオブジェクトにぶつかった時に呼び出される
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(itemnum);
        // ぶつかったオブジェクトが収集アイテムだった場合
        if (other.gameObject.CompareTag("Pick up"))
        {
            // その収集アイテムを非表示にします
            other.gameObject.SetActive(false);
            //fly = true;
            itemnum++;
        }
        Debug.Log(itemnum);
    }

    //物体から離れた時に呼ばれる
    void OnCollisionExit(Collision collision)
    {
        fly = true;
        rb.angularVelocity = new Vector3(0, 0, 0.5f);
        Debug.Log("Exit"); // ログを表示する
    }

    //物体が設置している場合に呼ばれる
    void OnCollisionStay(Collision collision)
    {
       
    }

    //物体が設置している場合に呼ばれる
    void OnCollisionEnter(Collision collision)
    {
        fly = false;
        itemnum = 0;
        Debug.Log("Enter"); // ログを表示する
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.iOS;

public class BicycleController2 : MonoBehaviour
{
    //直に動かして操作
    
    private Rigidbody m_rigidbody = null;
    private Quaternion rot;
    private Vector3 localAngles;
    public float max_speed = 100;
    public float max_right_speed = 10;
    public float force = 1000;
    public float m_speed;
    public bool onGround { get; set; }
    public float deg;
    public float multi_x = 0.02f;    //空中前傾姿勢の係数
    public float multi_handle = 0.2f;
    public float m_anglex;    //x軸の角度
    
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        rot = m_rigidbody.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        //地面についている間はハンドルおよびアクセル操作ができる
        m_speed = Vector3.Dot(m_rigidbody.velocity, m_rigidbody.transform.forward);
        localAngles = transform.eulerAngles;
        if (onGround)
        {
            if (m_speed < max_speed)
            {
                m_rigidbody.AddRelativeForce(0, 0, force * Input.GetAxis("Vertical"));
            }

            rot = Quaternion.Euler(localAngles.x, localAngles.y,
                      Input.GetAxis("Horizontal") * -deg) *
                  Quaternion.AngleAxis(m_speed * Input.GetAxis("Horizontal") * multi_handle, Vector3.up);
            m_rigidbody.MoveRotation(rot);
        }
        else //空中時の動作
        {
            m_anglex = localAngles.x + Input.GetAxis("Vertical") * deg * multi_x;
            rot = Quaternion.Euler(m_anglex, localAngles.y,
                Input.GetAxis("Horizontal") * -deg);
            m_rigidbody.MoveRotation(rot);
        }
        
        //横方向の滑り止め
        /*
        float m_horizontal_speed = Vector3.Dot(m_rigidbody.velocity, m_rigidbody.transform.right);
        if (Math.Abs(m_horizontal_speed) > 50)
        {

        }
        */
    }
    
    
}


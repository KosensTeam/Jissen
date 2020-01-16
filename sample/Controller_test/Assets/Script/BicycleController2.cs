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
    
    private bool onGround { get; set; }
    private Rigidbody m_rigidbody = null;
    private Quaternion rot;
    public Vector3 localAngles;
    private Vector3 angles;
    public float max_speed = 100;
    public float max_right_speed = 10;
    public float force = 1000;
    public float m_frontSpeed;
    public float maxDeg;    //最大車体傾き
    public float multi_x = 0.02f;    //空中前傾姿勢の係数
    public float multi_handle = 0.2f;
    public float m_horizontal_speed;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        rot = m_rigidbody.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        //地面についている間はハンドルおよびアクセル操作ができる
        m_frontSpeed = Vector3.Dot(m_rigidbody.velocity, m_rigidbody.transform.forward);
        localAngles = transform.localEulerAngles;
        angles = transform.eulerAngles;
        if (onGround)
        {
            if (m_frontSpeed < max_speed)
            {
                m_rigidbody.AddRelativeForce(0, 0, force * Input.GetAxis("Vertical"));
            }

            
            rot = Quaternion.Euler(localAngles.x, localAngles.y,
                      Input.GetAxis("Horizontal") * -maxDeg) *
                  Quaternion.AngleAxis(m_frontSpeed * Input.GetAxis("Horizontal") * multi_handle, Vector3.up);
                  
            m_rigidbody.MoveRotation(rot);
        }
        else //空中時の動作
        {

            //x軸の回転をデフォ(rot)に加える
            rot *= Quaternion.AngleAxis(Input.GetAxis("Vertical") * maxDeg * multi_x, Vector3.right);
            //z軸の回転を負荷＆適用
            m_rigidbody.MoveRotation(rot * Quaternion.Euler(0, 0,
                                         Input.GetAxis("Horizontal") * -maxDeg));
            

            //回転テスト
            //m_rigidbody.MoveRotation(m_rigidbody.rotation * Quaternion.AngleAxis(1, transform.right));
        }
        
        //横方向の滑り止め
        m_horizontal_speed = Vector3.Dot(m_rigidbody.velocity, m_rigidbody.transform.right);
        if (Math.Abs(m_horizontal_speed) > max_right_speed)
        {
            m_rigidbody.velocity -= Math.Sign(m_horizontal_speed) * transform.right * 1;
        }
    }
    
    
}


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
    private float turnDeg = 20;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        rot = m_rigidbody.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        //角度・速度取得
        Quaternion frot;
        m_frontSpeed = Vector3.Dot(m_rigidbody.velocity, m_rigidbody.transform.forward);
        localAngles = transform.localEulerAngles;
        angles = transform.eulerAngles;
        
        //地面についている間はハンドルおよびアクセル操作ができる
        if (onGround)
        {
            frot = GroundRun();
        }
        else //空中時の動作
        {
            frot = AirRun();
        }
        m_rigidbody.MoveRotation(frot);
        
        
    }

    Quaternion GroundRun()
    {

        if (m_frontSpeed < max_speed)
        {
            m_rigidbody.AddRelativeForce(0, 0, force * Input.GetAxis("Vertical"));
        }

        rot = Quaternion.Euler(localAngles.x, localAngles.y,
                  Input.GetAxis("Horizontal") * -maxDeg) *
              Quaternion.AngleAxis(m_frontSpeed * Input.GetAxis("Horizontal") * multi_handle, Vector3.up);


        /*    //簡易コードにしようとした残骸
        rot = transform.localRotation * Quaternion.AngleAxis(m_frontSpeed * Input.GetAxis("Horizontal") * multi_handle, transform.up);

        m_rigidbody.MoveRotation(rot * Quaternion.AngleAxis(Input.GetAxis("Horizontal") * -maxDeg, Vector3.forward));
        */
        
        //横方向の滑り止め
        m_horizontal_speed = Vector3.Dot(m_rigidbody.velocity, m_rigidbody.transform.right);
        if (Math.Abs(m_horizontal_speed) > max_right_speed)
        {
            m_rigidbody.velocity -= Math.Sign(m_horizontal_speed) * transform.right * 1;
        }

        return rot;
    }

    Quaternion AirRun()
    {

        //x軸の回転をデフォ(rot)に加える
        rot *= Quaternion.AngleAxis(Input.GetAxis("Vertical") * maxDeg * multi_x, Vector3.right);

        //指定角度以上なら旋回
        float dDeg = Math.Abs(Input.GetAxis("Horizontal") * maxDeg) - turnDeg;
        if (dDeg >= 0)
        {
            rot *= Quaternion.AngleAxis(Input.GetAxis("Horizontal")
                                        * multi_handle * dDeg, new Vector3(0,1, 0));
        }
            
        //z軸の回転を負荷＆適用
        return rot * Quaternion.AngleAxis(Input.GetAxis("Horizontal") * -maxDeg, Vector3.forward);

        



        //回転テスト
        //m_rigidbody.MoveRotation(m_rigidbody.rotation * Quaternion.AngleAxis(1, transform.right));
    }
    
    
}


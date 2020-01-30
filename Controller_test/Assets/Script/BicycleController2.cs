using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public struct CInput
{
    public float horizontal { get; set; }
    public float vertical { get; set;}

    public CInput(float angle, float speed)
    {
        this.horizontal = angle;
        this.vertical = speed;
    }
}

public struct CTime
{
    public float trigger { get; set; }
    float deltaTime { get; set; }
    public float start { get; set; }
    public float end { get; set; }
    

    public CTime(float trigger, float deltaTime, float start, float end)
    {
        this.trigger = trigger;
        this.deltaTime = deltaTime;
        this.start = start;
        this.end = end;
    }
    
    public float elapsed()
    {
        return start + (Time.time - trigger) / deltaTime * (end - start);
    }
}

public class BicycleController2 : MonoBehaviour
{
    //直に動かして操作


    public bool onGround { get; set; }
    public bool onWall { get; set; }
    private Rigidbody m_rigidbody = null;
    private Quaternion rot;
    public Vector3 localAngles;
    private Vector3 angles;
    public float max_speed = 100;
    public float max_right_speed = 10;
    public float force = 1000;
    public float m_frontSpeed;
    private Vector3 m_groundVelocity;
    public float maxDeg;    //最大車体傾き
    public float multi_x = 0.02f;    //空中前傾姿勢の係数
    public float multi_handle = 0.2f;
    public float m_horizontal_speed;
    public float turnDeg = 20;
    public float airSpeedMulti = 2;
    public CInput cInput;
    public bool bikeEnable = false;    //コントローラーかどうか
    public CTime cTime = new CTime(0, 0.1f, 0, 0);
    public float t;
    
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        rot = m_rigidbody.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        t = cTime.elapsed();
        
        //入力機器
        InputSel();
        
        //角度・速度取得
        Quaternion frot;
        m_frontSpeed = Vector3.Dot(m_rigidbody.velocity, m_rigidbody.transform.forward);
        localAngles = m_rigidbody.rotation.eulerAngles;
        angles = m_rigidbody.rotation.eulerAngles;
        
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
        m_groundVelocity = m_rigidbody.velocity;

        if (m_frontSpeed < max_speed)
        {
            m_rigidbody.AddRelativeForce(0, 0, force * cInput.vertical);
        }
        
        rot = Quaternion.Euler(localAngles.x, localAngles.y,
                  cInput.horizontal * -maxDeg * (m_frontSpeed / max_speed)) *
              Quaternion.AngleAxis((m_frontSpeed + 10) * cInput.horizontal * multi_handle, Vector3.up);


        //横方向の滑り止め
        m_horizontal_speed = Vector3.Dot(m_rigidbody.velocity, m_rigidbody.transform.right);
        if (Math.Abs(m_horizontal_speed) > max_right_speed)
        {
            m_rigidbody.velocity -= Math.Sign(m_horizontal_speed) * transform.right * 1;
        }
        
        //常時減速
        if (cInput.vertical == 0)
        {
            ;
        } 
        return rot;
    }

    Quaternion AirRun()
    {

        //x軸の回転をデフォ(rot)に加える
        if (cInput.vertical < 0) cInput.vertical = 0;
        float angle = (cInput.vertical - 0.2f) *maxDeg * multi_x;
 
        rot *= Quaternion.AngleAxis((cInput.vertical - 0.2f) * maxDeg * multi_x, Vector3.right);

        //指定角度以上なら旋回
        float dDeg = Math.Abs(cInput.horizontal * maxDeg) - turnDeg;
        if (dDeg >= 0)
        {
            rot *= Quaternion.AngleAxis(cInput.horizontal
                                        * multi_handle * dDeg, new Vector3(0,1, 0));
            //空中での速度変換
            m_rigidbody.velocity =  Quaternion.AngleAxis(cInput.horizontal
                                                         * multi_handle * dDeg, new Vector3(0,1, 0)) * m_rigidbody.velocity;

        }
            
                
        //z軸の回転を負荷＆適用
        return rot * Quaternion.AngleAxis(cInput.horizontal * -maxDeg, Vector3.forward);

        //回転テスト
        //m_rigidbody.MoveRotation(m_rigidbody.rotation * Quaternion.AngleAxis(1, transform.right));
    }

    public bool WallCheck()
    {
        return true;
    }
    
    void WallRun()
    {
        
    }

    void InputSel()
    {
        if (!bikeEnable)
        {
            cInput.horizontal = Input.GetAxis("Horizontal");
            cInput.vertical = Input.GetAxis("Vertical");
        }
        else
        {
            cInput.horizontal = cTime.elapsed();
        }
    }

}


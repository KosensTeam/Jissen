using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleBalance: MonoBehaviour
{
    //バランスとるスクリプト
    
    private Transform myTransform;
    private Rigidbody myRigitbody;
    public Vector3 angles;
    public float maxAngle;
    public float torque;
    public float t;
    public float m_d_anglez;

    void Start()
    {
        myTransform = GetComponent<Transform>();
        myRigitbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        angles = myTransform.localEulerAngles;
        
        m_d_anglez = (angles.z + 180) % 360 - 180;
        t = m_d_anglez * Vector3.Dot(myRigitbody.angularVelocity, myTransform.forward);


        myRigitbody.AddTorque(0, 0, -(m_d_anglez / 360) * torque);
    }
}

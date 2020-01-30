using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundChecker : MonoBehaviour
{
    //接地した場合の処理
    public UnityEvent OnEnterGround;
    //地面から離れた場合の処理
    public UnityEvent OnExitGround;
    //接地数
    private int enterNum = 0;

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("OnGround!");
        enterNum++;
        OnEnterGround.Invoke();
    }

    private void OnTriggerExit(Collider collision)
    {
        Debug.Log("ExitGround!");
        enterNum--;
        if (enterNum <= 0)
        {
            OnExitGround.Invoke();
        }
    }
}

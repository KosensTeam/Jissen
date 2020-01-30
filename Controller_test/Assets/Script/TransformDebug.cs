using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformDebug : MonoBehaviour
{
    public GameObject obj = null;
    private Transform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Text text = obj.GetComponent<Text>();
        text.text = "angles:" + myTransform.eulerAngles.z;

    }
}

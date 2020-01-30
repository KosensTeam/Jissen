using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SerialLight : MonoBehaviour {

	public SerialHandler serialHandler;
	public Text text;

	private BicycleController2 controller;
	private string[] strs;
	public GameObject bicycle;
	
	// Use this for initialization
	void Start () {
		//信号を受信したときに、そのメッセージの処理を行う
		serialHandler.OnDataReceived += OnDataReceived;

		controller = bicycle.GetComponent<BicycleController2>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*
	 * シリアルを受け取った時の処理
	 */
	void OnDataReceived(string message) {
		try {
			text.text = message; // シリアルの値をテキストに表示
			
			//変換・送信
			strs = message.Split(',');
			controller.cInput = new CInput(-float.Parse(strs[0]) / 100, float.Parse(strs[1]) / 100);
			controller.cTime.trigger = Time.time;
			controller.cTime.start = controller.cTime.end;
			controller.cTime.end = controller.cInput.horizontal;
			controller.cInput.horizontal = controller.cTime.elapsed();
		} catch (System.Exception e) {
			Debug.LogWarning(e.Message);
		}
	}
}
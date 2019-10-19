using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using UnityEngine.UI;
using System.Threading;
using System.Text;
using System.Net;

public class PlayerControls : MonoBehaviour {

	private float angle;
	private float maxAngle = 60f;
	private float minAngle = -60f;
	private float maxPos = 2.25f;
	private float minPos = -2.25f;
	private float xPos = -4.0f;
	public static SerialPort stream = new SerialPort("/dev/cu.usbmodem14101", 9600);
	public string userinput;


	// Use this for initialization
	void Start () {
		stream.Open();
		stream.ReadTimeout = 15;
	}
	
	// Update is called once per frame
	void Update () {
		try {
			while(stream.BytesToRead > 0) {
				userinput = stream.ReadLine();
			}
			float pos;
			float.TryParse (userinput, out angle);
			if (angle >= 0) {
				pos = -(angle / maxAngle) * maxPos ;
			} else {
				pos = -(angle / minAngle) * minPos ;
			}
			transform.position = new Vector3(xPos, Mathf.Clamp(pos,minPos,maxPos), 0.0f); 
			stream.BaseStream.Flush();
		} catch (TimeoutException e) {
			return;
		}
	}
}

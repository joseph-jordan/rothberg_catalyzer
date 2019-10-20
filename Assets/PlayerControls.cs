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
    public static float globalMinAngle = float.MaxValue;
    public static float globalMaxAngle = float.MinValue;
    public static int totalHits;
    public static bool updatedAngles = false;
private float maxAngle = 60f;
private float minAngle = -60f;
private float maxPos = 2.25f;
private float minPos = -2.25f;
private float xPos = -4.0f;
public static SerialPort stream = new SerialPort("/dev/cu.usbmodem14101", 9600);
public string userinput;
private Thread streamReaderThread;


// Use this for initialization
void Start () {
stream.Open();
stream.ReadTimeout = 15;
streamReaderThread = new Thread(new ThreadStart(ReadInput));
streamReaderThread.IsBackground = true;
streamReaderThread.Start();
}

void ReadInput() {
while(true) {
try {
userinput = stream.ReadLine();
                float posGlobal;
                float.TryParse(userinput, out posGlobal);
                if (globalMaxAngle < posGlobal)
                {
                    updatedAngles = true;
                    globalMaxAngle = posGlobal;
                }
                if (globalMinAngle > posGlobal)
                {
                    updatedAngles = true;
                    globalMinAngle = posGlobal;
                }
                stream.BaseStream.Flush();
} catch (TimeoutException e) {

}
}
}

// Update is called once per frame
void Update () {
float pos;
float.TryParse (userinput, out angle);
if (angle >= 0) {
pos = -(angle / maxAngle) * maxPos ;
} else {
pos = -(angle / minAngle) * minPos ;
}
transform.position = new Vector3(xPos, Mathf.Clamp(pos,minPos,maxPos), 0.0f);
}

    void OnCollisionEnter2D(Collision2D col)
    {
        totalHits++;
    }
}
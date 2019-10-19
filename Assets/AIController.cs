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

public class AIController : MonoBehaviour
{
    private float maxPos = 2.25f;
    private float minPos = -2.25f;
    private float xPos = 4.0f;
    public GameObject Ball;


    // Use this for initialization
    void Start()
    {
        if (Ball == null)
        {
            Ball = GameObject.FindWithTag("Ball");
        }
    }

    // Update is called once per frame
    void Update()
    {
        double ballY = Ball.GetComponent<Rigidbody2D>().position[1];
        ballY = Math.Max(Math.Min(ballY, 7.75), -7.75);

        double myY = this.GetComponent<Rigidbody2D>().position[1];

        double diff = ballY - myY;
        double random = GetRandomNumber(0.015, 0.23);
        double boost = (0.16 * diff * random);
        if (diff > 0.28 || diff < -0.28)
        {
            float pos = (float)((0.03738 * sign(diff) + boost) + myY);
            if (pos < 0)
            {
                pos = Mathf.Max(pos, minPos);
            }
            pos = Mathf.Min(pos, maxPos);
            transform.position = new Vector3(xPos, pos, 0.0f);
        }
        return;

    }

    public double GetRandomNumber(double minimum, double maximum)
    {
        System.Random random = new System.Random();
        return random.NextDouble() * (maximum - minimum) + minimum;
    }

    int sign(double d)
    {
        if (d >= 0)
        {
            return 1;
        }
        return -1;
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO.Ports;
using System.Globalization;
using System.Runtime.InteropServices;
using System;

public class Serial : MonoBehaviour {

	static SerialPort _serialPort;

    void Awake()
    {

        try 
        {
            _serialPort = new SerialPort("COM1", 9600);
            _serialPort.Open();
        }
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
        }
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI(){

	}

	public void MotivateMotor()
	{
        if(_serialPort.IsOpen)
        {
            _serialPort.Write("1");
        }
	}

	void OnDestroy(){
		_serialPort.Close();
	}
}

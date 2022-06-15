using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Threading;

public class ForceCapture : MonoBehaviour {

	//手指按压力
	public float[] m_FingerForce;
	public float[] m_BiasedForce;
	private long biasedFrameNum = 0;
	private bool bShowdata = false;
	const int FACTOR = 55;

    [DllImport("MP4623.DLL")]
	public static extern IntPtr MP4623_OpenDevice (Int32 dev_num);
	[DllImport("MP4623.DLL")]
	public static extern Int32 MP4623_CAL(IntPtr hDevice);
	[DllImport("MP4623.DLL")]
	public static extern Int32 MP4623_AD(IntPtr hDevice, Int32 stch, Int32 endch, Int32 gain, Int32 sidi, Int32 trsl, Int32 trpol, Int32 clksl, Int32 clkpol, Int32 tdata);
	[DllImport("MP4623.DLL")]
	public static extern Int32 MP4623_ADRead(IntPtr hDevice, Int32 rdlen, Int32[] addata);
	[DllImport("MP4623.DLL")]
	public static extern Int32 MP4623_ADStop(IntPtr hDevice);
	[DllImport("MP4623.DLL")]
	public static extern Int32 MP4623_ADPoll(IntPtr hDevice);
	
	
	IntPtr hDevice;
	Int32[] addata;
	string[] str;
	int _x ;

	// Use this for initialization
	void Start () {
		m_FingerForce = new float[GameControl.NumStrings];
		m_BiasedForce = new float[GameControl.NumStrings];
		InitialADCard ();
		//固定时间调用 数据更新函数  2s后执行  每秒调用20次
		InvokeRepeating ("CallData", 2f, 0.05f);
	}


	//初始化AD数据采集卡
	void InitialADCard(){
		addata = new Int32[600000];
		str = new string[15];
		
		try{
			hDevice = MP4623_OpenDevice(0);
		}
		catch(Exception ex){
			Debug.Log(ex);
		}
		
		try{
			MP4623_CAL (hDevice);
		}
		catch(Exception ex){
			Debug.Log(ex);
		}
		
		MP4623_AD(hDevice, 0, 14, 1, 0, 0, 0, 0, 0, 100);
		
		_x = MP4623_ADPoll (hDevice);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.S))
						bShowdata = !bShowdata;
	}

	//固定时间调用此函数  更新手指数据
	void CallData(){
		MP4623_AD(hDevice, 0, 14, 1, 0, 0, 0, 0, 0, 100);
		
		Thread.Sleep (10);
		
		_x = MP4623_ADPoll (hDevice);
		if (_x < 0) {
			Debug.Log ("ad fifo over!!!");
		} else {
			if (MP4623_ADRead(hDevice, _x, addata) == -1)
				Debug.Log("ad fifo over!!!");
			else
			{
				for (int i = 0; i < m_FingerForce.Length; i++)
				{
					if(biasedFrameNum < 10)
					{
						m_BiasedForce[i] += -((addata[i] >> 4) - 2048) * 5000 / 2048;//mp4623 is 12bit ad
					}
					else if(biasedFrameNum == 10)
					{
						m_BiasedForce[i] = m_BiasedForce[i]/10f;
					}
					else
					{
						m_FingerForce[i] = (-((addata[i] >> 4) - 2048) * 5000 / 2048-m_BiasedForce[i])/FACTOR;
					}
				}
//				Debug.Log(string.Format("Biased Data: {0}, {1}, {2}, {3}, {4}", m_BiasedForce[0],m_BiasedForce[1],m_BiasedForce[2],m_BiasedForce[3],m_BiasedForce[4]));
				biasedFrameNum++;
			}
		}
	}

	void OnGUI(){
		if(bShowdata == true)
		for (int i = 0; i < m_FingerForce.Length; i++) {
			int _width = (i%7)*110;
			int _height = ((int)(i/7))*50;
			GUI.Label(new Rect(_width, _height, 100, 40), "Ch"+i.ToString()+": "+m_FingerForce[i].ToString());
		}
	}
}

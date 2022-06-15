using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Text;

public class GameControl : MonoBehaviour
{
    public int nowtrials;
    public int trialnumber=100;
    // AD card script
    public ForceCapture forceCapture;
    public float xishu=1.5f;
    public Serial serial;
    //Game Score
    public int score;
    public GUIText scoreText;
    // Node prefabs, Node positions
    public GameObject NodePrefabs;

    public const int NumStrings = 4;

    public string user;
    public static int my;////defined by me
    public static double RT;////defined by me
    public static double SumRT = 0.0;////defined by me

    // object to display score
    public GameObject m_score;

    //2016.9.10
    //public GameObject obj;

    // visual effect
    public GameObject[] parGameObject;

    // Store the finger force
    private bool isTrainingStart;
    private ForceFingerButton[] fingerForce;
    private bool[] nodeState;
    private float reactingTime;
    private DataRecord dataRecord;
    public  Text scoretext;
    public static int scoreData = 0;
    public static int grade=1; // Numbers of fingers
    private SaveDataElement[] saveDataElement=new SaveDataElement[NumStrings];
    public int currentNoteNumber;
    public bool[] fingerOccupied;
    public int[] currentFinger;

    public bool[] fingerSuccess=new bool[NumStrings];
    public bool allDestroy;
    public int x = 1;
    public bool[] lianxu = new bool[4] { false,false,false,false};
    public Color newcolor;
    public SerialPortController sp;
    System.Random random = new System.Random(1);
    public int RandomRange(int min,int max)
    {
        return random.Next(min, max);
    }
    public Color RandomColor()
    {
        //随机颜色的HSV值,饱和度不变，只改变H值
        //H、S、V三个值的范围都是在0~1之间
        //float t = Random.Range(0f, 1f);
        int t = RandomRange(1, 100);
        float r, g;
        if (t > 90)
        {
            r = 1f;
            g = 0f;
        }
        else
        {
            r = 0f;
            g = 1f;
        }
        float b = 0f;
        //float a = Random.Range(0f, 1f);//加透明度
        Color color = new Color(r, g, b);
        return color;
    }
    void Awake()
    {
        //  Initial the script variable		
        InitialButtonReferences();
        //	Initial the node stata
        nodeState = new bool[NumStrings] { false, false, false, false };
        isTrainingStart = false;
        dataRecord = new DataRecord();
        ADAlgo.user = user;
    }

    //Use this for initialization
    void Start()
    {
        // initial the adaptive algorithm 
        ADAlgo.Initial(x);
        // initial parameter
        //saveDataElement = new SaveDataElement();
        dataRecord.Start();
        ResetNodeState();
        sp = GameObject.Find("Guitar").GetComponent<SerialPortController>();
        byte[] marker1 = new byte[5] { 0x01, 0xE1, 0x01, 0x00, 0x00 };//////////////
        sp.WriteData(marker1);
        currentNoteNumber = 0;
        fingerOccupied =new bool[NumStrings]{false, false, false, false};
        currentFinger = new int[grade];
        
        nowtrials = 0;
        
    }

    public void AllDestroy(int nodeIndex)
    {
        bool destroy=true;
        for (int i = 0; i < grade; i++)
        {
            if (currentFinger[i] == nodeIndex)
            {
                fingerSuccess[i] = true;
            }
        }

        for (int j = 0; j < grade; j++)
        {
            if (!fingerSuccess[j])
                destroy = false;
        }

        if (destroy)
        {
            allDestroy = true;
        }

        

    }

    void InitialButtonReferences()
    {
        fingerForce = new ForceFingerButton[NumStrings];
        for (int i = 0; i < NumStrings; ++i)
        {
            fingerForce[i] = GameObject.Find("StringButton" + (i + 1)).GetComponent<ForceFingerButton>();
        }
    }

  
    public void DataWrite()
    {
        for (int i = 0; i < grade; i++)
        {
           
            dataRecord.WriteDataToExcel(saveDataElement[i]);
        }

        
    }

    public void UpdatesaveDataElement_false(int nodeIndex)
    {
        for (int i = 0; i < grade; i++)
        {
            if (currentFinger[i] == nodeIndex)
            {
                //saveDataElement[i].fingerIndex = nodeIndex;
                saveDataElement[i].reactingTime = saveDataElement[i].deadline;
                saveDataElement[i].result = 0;
            }
        }
    }

    public void UpdatesaveDataElement_true(int nodeIndex)
    {
        for (int i = 0; i < grade; i++)
        {
            if (currentFinger[i] == nodeIndex)
            {
                saveDataElement[i].reactingTime = Time.time - reactingTime;
                saveDataElement[i].result = 1;
            }
        }
    }


    void EndTrial()
    {
        for (int i = 0; i < grade; i++)
        {
            fingerSuccess[i] = false;
        }
        // record the start time
        reactingTime = Time.time;
        //发送marker-0表示脑电开始，打标签加上记录PC时间
        

        //	check the node state, update the score according to the results
        if (ADAlgo.resultIndex > 0)
        {
            /*if (lianxu[0] && lianxu[1] && lianxu[2] && lianxu[3])
            {
                x = (x + 1) <= 12 ? x+1 : 12;
            }
            else if(!lianxu[0] && !lianxu[1] && !lianxu[2] && !lianxu[3])
            {
                x = (x - 1) >= 0 ? x - 1 : 0;
            }
            ADAlgo.Initial(x);*/
            if (CheckNodeState())
            {
                // if success
                UpdateScore();
                //reactingTime = reactingTime - Time.time;
                //saveDataElement.reactingTime = reactingTime;
                //saveDataElement.result = 1;
                //my = saveDataElement.result;
                my = 1;
                //ADAlgo.SetResult(true);

            }
            else
            {
                // if failed
                //saveDataElement.reactingTime = saveDataElement.deadline;
                //saveDataElement.result = 0;
                //RT = saveDataElement.deadline;
                //my = saveDataElement.result;
                my = 0;
                //ADAlgo.SetResult(false);
                //dataRecord.WriteData(saveDataElement);
            }
            nowtrials++;
            //发送marker-1表示脑电结束，打标签加上记录PC时间
            byte[] marker1 = new byte[5] { 0x01, 0xE1, 0x01, 0x00, 0x01 };//////////////
            sp.WriteData(marker1);

            SumRT += RT; ///total time consume
            scoreData += my; ///total score
            DataWrite();
            CreateNode();
            scoretext.text = scoreData.ToString();
            if (nowtrials >= trialnumber) UnityEditor.EditorApplication.isPlaying = false;
            // reset the node state
            ResetNodeState();
            marker1 = new byte[5] { 0x01, 0xE1, 0x01, 0x00, 0x00 };//////////////
            sp.WriteData(marker1);
        }
 
    }

    //	Create Node
    void CreateNode() 
    {

        bool plusindex = false;
        if (currentNoteNumber == 0)
        {


            // create the new nodes
            int fingerIndex = Random.Range(0, 4);
            currentFinger[currentNoteNumber] = fingerIndex;
            fingerOccupied[fingerIndex] = true;
            //int fingerIndex = Random.Range(1, 2);
            //int fingerIndex = 1;
            plusindex = true;
            Value newTrialValue = ADAlgo.GetValue(currentNoteNumber, plusindex);
            float targetForce = newTrialValue.targetforce;
            float tolerance = newTrialValue.tolerance;
            float upper = newTrialValue.upperbound;
            float lower = newTrialValue.lowerbound;
            float deadline = newTrialValue.GetDeadLine();
            // save the data to the save data element
            saveDataElement[currentNoteNumber].fingerIndex = fingerIndex;
            saveDataElement[currentNoteNumber].targetForce = targetForce;
            saveDataElement[currentNoteNumber].tolerance = tolerance;
            saveDataElement[currentNoteNumber].upperbound = upper;
            saveDataElement[currentNoteNumber].lowerbound = lower;
            saveDataElement[currentNoteNumber].deadline = deadline;
            saveDataElement[currentNoteNumber].trialNumber = ADAlgo.resultIndex;
            newcolor = RandomColor();
            CreateNewNode(fingerIndex, tolerance, deadline, targetForce,newcolor);

            currentNoteNumber++;

            plusindex = false;
            newTrialValue = ADAlgo.GetValue(currentNoteNumber, plusindex);
            // equalized all cues
            targetForce = newTrialValue.targetforce;
            tolerance = newTrialValue.tolerance;
            upper = newTrialValue.upperbound;
            lower = newTrialValue.lowerbound;
            deadline = newTrialValue.GetDeadLine();
            // ʹ����Ƭ��ֵһ��
           
            for (int i = 1; i < grade; i++)
            {
                fingerIndex = Random.Range(0, 4);
                while (fingerOccupied[fingerIndex])
                {
                    fingerIndex = Random.Range(0, 4);
                }

                currentFinger[currentNoteNumber] = fingerIndex;
                fingerOccupied[fingerIndex] = true;
                saveDataElement[currentNoteNumber].fingerIndex = fingerIndex;
                saveDataElement[currentNoteNumber].targetForce = targetForce;
                saveDataElement[currentNoteNumber].tolerance = tolerance;
                saveDataElement[currentNoteNumber].upperbound = upper;
                saveDataElement[currentNoteNumber].lowerbound = lower;
                saveDataElement[currentNoteNumber].deadline = deadline;
                saveDataElement[currentNoteNumber].trialNumber = ADAlgo.resultIndex;

                CreateNewNode(fingerIndex, tolerance, deadline, targetForce,newcolor);

                currentNoteNumber++;


            }
            

        }
        //CreateNewNodeByIndex(fingerIndex);
    }

    void ResetNodeState()
    {
        for (int i = 0; i < NumStrings; i++)
        {
            SetNodeState(i, true);
        }
    }

    //check current stata
    public bool CheckNodeState()
    {
        allDestroy = false;
        for (int i = 0; i < grade; i++)
        {
            if (nodeState[currentFinger[i]] == false)
                return false;
        }
        return true;
        
        
    }

    // set node state
    public void SetNodeState(int index, bool success)
    {
        nodeState[index] = success;
    }

    // update the node frame
    public void UpdateNodeFrame()
    {
        if (CheckNodeState())
        {
            // motivate the vibration motor
            serial.MotivateMotor();
            // record the reacting time
            //saveDataElement.reactingTime = Time.time - reactingTime;
            RT = Time.time - reactingTime;
        }

        Invoke("EndTrial", 0.5f);
    }


    void CreateNewNodeByIndex(int index)
    {
        int _posIndex = index;		                    //node position 1-5
        SetNodeState(index, false);		                //set node state
        float _tolerance = Random.Range(1.0f, 3.0f);	//change the force tolerance
        float _holdtime = Random.Range(0.5f, 1.5f);	    //set the deadline
        float _height = Random.Range(0.5f, 2f);	        //node value, implying finger force value
        GameObject _newNode = (GameObject)Instantiate(NodePrefabs, new Vector3(_posIndex - 2, _height, 1.666f), Quaternion.Euler(new Vector3(0, 0, 0)));
        _newNode.transform.localScale = new Vector3(1, 1, 1);
        _newNode.transform.Find("Cylinder").GetComponent<StringNode>().NodeIndex = _posIndex;
        _newNode.transform.localScale = new Vector3(1f, _tolerance, 1f);

        //Set destroy time
        _newNode.transform.Find("Cylinder").GetComponent<StringNode>().SetTime(_holdtime);

    }

    // Parameter: finger index, tolerance, deadline, target force
    void CreateNewNode(int index, float tol, float dl, float tf,Color newcolor)
    {

        int _posIndex = index;		        //node position 1-5
        SetNodeState(index, false);		    //set node state
        float _tolerance = tol * 2.5f;	    //change the force tolerance
        float _holdtime = dl;		        //set the deadline
        float _height = tf / 2f;	            //node value, implying finger force value
        GameObject _newNode = (GameObject)Instantiate(NodePrefabs, new Vector3(_posIndex - 2, _height, 1.666f), Quaternion.Euler(new Vector3(0, 0, 0)));
        _newNode.transform.localScale = new Vector3(1, 1, 1);
        _newNode.transform.Find("Cylinder").GetComponent<StringNode>().NodeIndex = _posIndex;
        //_newNode.transform.FindChild("Cylinder").GetComponent<BoxCollider>().size = new Vector3(0.8f,_tolerance,0.8f);
        _newNode.transform.localScale = new Vector3(1f, _tolerance, 1f);
        _newNode.transform.Find("Cylinder").GetComponent<MeshRenderer>().material.color = newcolor;
        //Set destroy time
        _newNode.transform.Find("Cylinder").GetComponent<StringNode>().SetTime(_holdtime);
       
    }

    void Update()
    {
        // update the finger force value to change the position of paddle
        for (int i = 0; i < NumStrings; ++i)
        {
            fingerForce[i].ForceValue = xishu * forceCapture.m_FingerForce[i];
        }

        // space key indicates the training start
        if (Input.GetKeyDown(KeyCode.Space) && isTrainingStart == false)
        {
            isTrainingStart = true;
            //	Invoke method in 0.5 second
            Invoke("CreateNode", 0.5f);
        }

        // display the score
        if (m_score != null)
        {
            m_score.GetComponent<TextMesh>().text = ADAlgo.resultIndex.ToString();
        }
    }

    public void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();
       
    }

    public void MotivateEffect(bool success, int _index, float _height)
    {
        GameObject particles;
        if (success)
            particles = (GameObject)Instantiate(parGameObject[1]);
        else
            particles = (GameObject)Instantiate(parGameObject[0]);

        particles.transform.position = new Vector3(_index - 2, _height, 1.666f);
#if UNITY_3_5
			particles.SetActiveRecursively(true);
#else
        particles.SetActive(true);
        for (int i = 0; i < particles.transform.childCount; i++)
            particles.transform.GetChild(i).gameObject.SetActive(true);
#endif

        ParticleSystem ps = particles.GetComponent<ParticleSystem>();
        if (ps != null && ps.loop)
        {
            ps.gameObject.AddComponent<CFX3_AutoStopLoopedEffect>();
            ps.gameObject.AddComponent<CFX_AutoDestructShuriken>();
        }
    }

    void OnDestroy()
    {
        //if (dataRecord.IsOpen())
        //    dataRecord.Close();

        ADAlgo.Close();
    }

    void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reset the Camera Pos");
            OVRManager.display.RecenterPose();
        }

         //check the trial number;
        if (ADAlgo.bSampleFlag == true)
        {
            Application.LoadLevel("blackscene");
            Debug.Log("Sample Over");
        }
    }
}
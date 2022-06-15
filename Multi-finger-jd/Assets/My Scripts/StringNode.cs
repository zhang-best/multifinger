using UnityEngine;
using System.Collections;

public class StringNode : MonoBehaviour {

	public float InitialHeight= 0.1f;
	public float Speed = 50;
	public int NodeIndex = 0;
    public bool success = false;


	private GameControl gameControl;
    private float durationTime;
    public Color nowcolor;
    private Transform parentTransform;
    public GameObject paddle1, paddle2, paddle3, paddle4;
    public bool issuccess;
    public double alltime;
	// Use this for initialization
	void Start () {
        durationTime = 0.0f;
		gameControl = GameObject.Find ("Guitar").GetComponent<GameControl> ();
        paddle1 = GameObject.Find("StringButton1/Paddle");
        paddle2 = GameObject.Find("StringButton2/Paddle");
        paddle3 = GameObject.Find("StringButton3/Paddle");
        paddle4 = GameObject.Find("StringButton4/Paddle");
        issuccess = true;
        alltime = 0;
       
	}

	public void SetTime(float destroyTime = 1f){
		Invoke ("Miss", destroyTime);
	}
	
	// Update is called once per frame
	void Update () {
        nowcolor = GameObject.Find("Guitar").GetComponent<GameControl>().newcolor;
        alltime += Time.deltaTime;
        if ((paddle1.transform.position.y > 0.3 || paddle2.transform.position.y > 0.3 || paddle3.transform.position.y > 0.3 || paddle4.transform.position.y > 0.3))
        {
            issuccess = false;
        }
        if (nowcolor[0] > 0.5f && alltime > 1.5f && issuccess && parentTransform == null && (!success))
        {
            success = true;
            parentTransform = paddle1.transform.parent;
            
        }
        if ((durationTime != 0 && Time.time - durationTime > 1.00))
        {
            if (nowcolor[0] < 0.5f&&parentTransform != null&&(!success))
            {
                success = true;
            }

        }
        if (gameControl.allDestroy)
        {
            for(int i=0;i<GameControl.grade;i++)
            {
                gameControl.fingerSuccess[i] = false;
            }
            Destroy(gameObject.transform.parent.gameObject);
            gameControl.currentNoteNumber--;
            if (gameControl.currentNoteNumber == 0)
            {
                GameObject.Find("Guitar").GetComponent<GameControl>().UpdateNodeFrame();
            }
        }
        
	}

	// if missed
	void Miss(){
        if (success)
        {
            issuccess = false;
            // active sound
            parentTransform.gameObject.GetComponent<AudioSource>().Play();
            // active visual effect
            GameObject.Find("Guitar").GetComponent<GameControl>().MotivateEffect(true, NodeIndex, transform.position.y);
            // set flag
            GameObject.Find("Guitar").GetComponent<GameControl>().SetNodeState(NodeIndex, true);
            gameControl.UpdatesaveDataElement_true(NodeIndex);
            Destroy(gameObject.transform.parent.gameObject);

            gameControl.fingerOccupied[NodeIndex] = false;
           
        }
        else{
            GameObject.Find("Guitar").GetComponent<AudioSource>().Play();
            GameObject.Find("Guitar").GetComponent<GameControl>().MotivateEffect(false, NodeIndex, transform.position.y);
            // update the node frame
            GameObject.Find("Guitar").GetComponent<GameControl>().SetNodeState(NodeIndex, false);
            gameControl.UpdatesaveDataElement_false(NodeIndex);
            // self-destroy
            Destroy(gameObject.transform.parent.gameObject);

            gameControl.fingerOccupied[NodeIndex] = false;
            //gameControl.currentNoteNumber--;
            
            
        }
        
        gameControl.currentNoteNumber--;
        Destroy(gameObject.transform.parent.gameObject);
        if (gameControl.currentNoteNumber == 0)
        {
            GameObject.Find("Guitar").GetComponent<GameControl>().UpdateNodeFrame();
        }
	}

	// if clicked
	void OnTriggerExit(Collider other) 
    {
        //Debug.Log("Trigger Exit");

        durationTime = 0.0f;

        //if (Time.time - durationTime > 0.10)
        //{
        //    Debug.Log("Trial Done");
        //    //Get the Finger Button Stata
        //    Transform _parentGo = other.transform.parent;
        //    // active sound
        //    _parentGo.gameObject.GetComponent<AudioSource>().Play();
        //    // set flag
        //    GameObject.Find("Guitar").GetComponent<GameControl>().SetNodeState(NodeIndex, true);
        //    // update the node frame
        //    GameObject.Find("Guitar").GetComponent<GameControl>().UpdateNodeFrame();
        //    // self-destroy
        //    Destroy(gameObject.transform.parent.gameObject);
        //}
        //else
        //{
        //    durationTime = Time.time;
        //}
	}

    void OnTriggerEnter(Collider other)
    {
        durationTime = Time.time;
        parentTransform = other.transform.parent;
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseCS : MonoBehaviour
{
    bool isGamePause;

    public Canvas pauseCanvas;
    public Canvas ongameCanvas;
    //public Canvas endCanvas;

    public Text pausetest;
    public Text Scoretext;
    public Text TimeText;
    public Text onGameScoreText;

    public bool isPause = false;


    public int pauseThreshold =10;
    public int endThreshold = 30;
    void Start()
    {
        pausetest.text = (" "+System.Environment.NewLine+ "Take a break!"+System.Environment.NewLine);
        Application.DontDestroyOnLoad(this.gameObject);  
        pauseCanvas.enabled = false;
        //endCanvas.enabled = false;
        

    }

    void Update()
    {
        if (ADAlgo.resultIndex > 0)
        {

            
            ///////// Trials of a session
            if (ADAlgo.resultIndex % pauseThreshold == 1 && ADAlgo.resultIndex != 1&&isPause == false)
            {
                if (!isGamePause)
                {
                    ongameCanvas.enabled = false;
                    pauseCanvas.enabled = true;
                    Scoretext.text = GameControl.scoreData.ToString();
                    TimeText.text = GameControl.SumRT.ToString("0.00");
                    isGamePause = true;
                    Time.timeScale = 0.0f;    

                }

            }
            else if (ADAlgo.resultIndex % pauseThreshold != 1 && isPause==true )
            {
                isPause = false;
            }
        }


        if (ADAlgo.resultIndex >= endThreshold + 1)
        {
            
            pausetest.text=("Congretulations!"+System.Environment.NewLine +" You have finished the game"+System.Environment.NewLine+"按Esc键退出"+System.Environment.NewLine);
            
            Time.timeScale = 0.0f;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ADAlgo.bSampleFlag = true;
            }

        }

        if (Input.GetKeyDown(KeyCode.P))
        {

            if (!isGamePause)
            {
                Time.timeScale = 0.0f;
                isGamePause = !isGamePause;

            }

            else
            {
                Time.timeScale = 1.0f;
                isPause = true;
                ongameCanvas.enabled = true;
                pauseCanvas.enabled = false;
                isGamePause = !isGamePause;
                GameControl.scoreData = 0;
                onGameScoreText.text = GameControl.scoreData.ToString();
               

            }
            
            
        }

    }

   

    

}


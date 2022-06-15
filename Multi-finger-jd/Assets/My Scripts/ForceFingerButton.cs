using UnityEngine;
using System.Collections;

public class ForceFingerButton : MonoBehaviour
{
	// Finger Force Value
	private float forceValue = 0;
	// Finger Index
	public int FingerIndex;
	// The button move index  Force/Height
	public const float MoveIndex = 3f;
    // initial position of the paddle
    private Vector3 initialPositionOfPaddle;
	
	// Use this for initialization
	void Start ()
	{
		// get the initial position fo the paddle
        initialPositionOfPaddle = transform.Find("Paddle").transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		OnStringChange (forceValue);
	}

	public void OnStringChange(float value)
	{
        float paddleHeight = value / MoveIndex;
        Vector3 paddlePosition = initialPositionOfPaddle;
		if( value > 0.1 )
		{
			//Only press this if less then two buttons are already pressed
			//The keyboard limits multiple key presses arbitrarily, sometimes its 2, sometimes 3
			//So I locked it to a maximum of two key presses at the same time for consistency
			
			//Move the paddle upwards
            paddlePosition.y += paddleHeight;

			//Enable the light
			transform.Find( "Light" ).GetComponent<Light>().enabled = true;
		}
		else
		{
			//Move paddle down
			
			//Disable light
			transform.Find( "Light" ).GetComponent<Light>().enabled = false;
		}
		
		//Set paddle position
        transform.Find("Paddle").transform.position = paddlePosition;
	}
	
	public float ForceValue {
		set {
			forceValue = value;
		}
		
		get {
			return forceValue;
		}
	}
}

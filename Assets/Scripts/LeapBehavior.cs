using UnityEngine;
using System.Collections;
using Leap;

public class LeapBehavior : MonoBehaviour {

	// Controller object from the Leap Motion API
	private Controller leapController;

	// our GameObjects
	public GameObject fingersGO;
	public GameObject circleGO;
	public GameObject cubeGO;

	// reference to the scripts attached to our GameObjects above
	private FingersBehavior fingersBehavior;
	private CircleBehavior circleBehavior;
	private CubeBehavior cubeBehavior;

	// hold reference to a previous Frame object
	private Frame prevFrame;
	
	// native Unity function
	void Start () 
	{	
		// standard way to instantiate Controller object from the Leap Motion API
		leapController = new Controller ();

		// we must enable the gesture we want to detect
		leapController.EnableGesture (Gesture.GestureType.TYPECIRCLE);

		// getting access to the scripts attached to the GameObjects
		fingersBehavior = fingersGO.GetComponent<FingersBehavior>();
		circleBehavior = circleGO.GetComponent<CircleBehavior>();
		cubeBehavior = cubeGO.GetComponent<CubeBehavior>();

		// hide the cube on screen first
		cubeGO.renderer.enabled = false;
	}
	
	// native Unity function, called every frame
	void Update () 
	{
		// get the frame object that gives us tracking data
		Frame frame = leapController.Frame();

		// get the list of pointables from our frame object
		PointableList pointables = frame.Pointables;

		/****************************
		 * 1st task - display fingers
		 * by passing a list of pointables
		 * that contain position information
		 ****************************/
		fingersBehavior.Render(pointables);

		/**********************************
		 * 2nd task - detect circle gesture
		 * by getting a list of generic gestures
		 *********************************/
		handleGestures(frame.Gestures (), pointables);

		/***********************************
		 * 3rd task - handle scaling motions
		 ***********************************/
		if(prevFrame != null) {

			/* derive scaleFactor from the overall motion between
			 * the current frame and the previous frame. */
			float scaleFactor = frame.ScaleFactor(prevFrame);

			/* The scale factor is always positive, and is either less than, equal to or greater than 1. 
			 * If less than 1, contraction took place. If greater than 1, expansion took place. No scaling
			 * took place if equal to 1. */
			cubeBehavior.setScale( scaleFactor );
		}
		prevFrame = frame;

		// show the cube
		if(!cubeGO.renderer.enabled) {
			cubeGO.renderer.enabled = true;
		}
	}

	void handleGestures(GestureList gestures, PointableList pointables)
	{
		// g is a generic Gesture, we need to find the one that has the type circle
		foreach (Gesture g in gestures) 
		{
			// if user is using 1 finger, and gesture type is circle, and gesture is finished
			if(pointables.Count == 1 && g.Type == Gesture.GestureType.TYPECIRCLE && g.State == Gesture.GestureState.STATESTOP) {
				handleCircle( g );
			}
		}
	}

	void handleCircle(Gesture g)
	{
		// we create an actual CircleGesture object from the generic gesture object, g
		CircleGesture circle_g = new CircleGesture(g);

		// determine if circle is drawn clockwise
		if (circle_g.Pointable.Direction.AngleTo (circle_g.Normal) <= Mathf.PI / 2) {
			// draw clockwise circle on screen
			circleBehavior.showCircle (new Vector3 (circle_g.Center.x, circle_g.Center.y, -circle_g.Center.z), circle_g.Radius * 2, "clockwise");
		} else {
			// draw circle on screen in counter clockwise
			circleBehavior.showCircle (new Vector3 (circle_g.Center.x, circle_g.Center.y, -circle_g.Center.z), circle_g.Radius * 2, "counter-clockwise");
		}
	}
}

using UnityEngine;
using System.Collections;
using Leap;

public class FingersBehavior : MonoBehaviour {

	public GameObject fingerPrefab;

	private GameObject[] fingersGO;
	private int numFingers = 10;
	private Pointable p;
	private GameObject c;
	private int i;

	public void Render(PointableList pointables)
	{
		// first we create our game objects to represent circles
		if(fingersGO == null) {
			createFingersGameObject();
		}

		// show or hide fingers
		for(i=0; i<numFingers; i++)
		{
			// finger object
			c = fingersGO[i];
			// pointable object from Leap
			p = pointables[i];

			// if pointable exists
			if(p.IsValid) {
				// position, note reversed Z
				c.transform.position = new Vector3(p.TipPosition.x, p.TipPosition.y, -p.TipPosition.z);
				if(!c.renderer.enabled) {
					// show
					c.renderer.enabled = true;
				}
			} else {
				// hide
				c.renderer.enabled = false;
			}
		}

	}

	private void createFingersGameObject()
	{
		fingersGO = new GameObject[numFingers];
		for(i=0; i<numFingers; i++) {
			fingersGO[i] = Instantiate(fingerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			fingersGO[i].transform.parent = transform;
		}
	}
}

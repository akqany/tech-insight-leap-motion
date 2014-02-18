using UnityEngine;
using System.Collections;

public class CircleBehavior : MonoBehaviour {
	
	public GameObject circleCWPrefab; // clockwise graphic
	public GameObject circleCCWPrefab; // counter-clockwise graphic

	private GameObject circle;
	private Vector3 targetScale;

	public void showCircle(Vector3 pos, float diameter, string dir)
	{
		// remove the previously drawn circle on screen
		if(circle != null) {
			Destroy(circle);
		}

		// a prefab is a resuable GameObject
		if(dir == "clockwise") {
			circle = Instantiate (circleCWPrefab, pos, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 300)))) as GameObject; // show clockwise 
		} else {
			circle = Instantiate (circleCCWPrefab, pos, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 300)))) as GameObject; // show counter clockwise
		}

		// get the size of the graphics
		Vector3 size = circle.GetComponentInChildren<MeshCollider>().bounds.size;

		// scale it based on how big user draws the circle
		targetScale = new Vector3 (diameter/size.x, diameter/size.y, 1);

		// set scale bigger so we can animate it inside Update()
		circle.transform.localScale = new Vector3(targetScale.x * 1.5f, targetScale.y * 1.5f, 3);

		// set the circle to be a child of current transform
		circle.transform.parent = transform;
	}

	void Update()
	{
		if(circle != null) {
			circle.transform.localScale = Vector3.Lerp(circle.transform.localScale, targetScale, Time.deltaTime * 12);
		}
	}
}

using UnityEngine;
using System.Collections;

public class FingerScript : MonoBehaviour {

	public void Render(Vector3 pos)
	{
		gameObject.transform.position = pos;
	}
}

using UnityEngine;
using System.Collections;

public class CubeBehavior : MonoBehaviour {

	public void setScale(float scale)
	{
		gameObject.transform.localScale += new Vector3(scale - 1, scale - 1, scale - 1);
	}
}

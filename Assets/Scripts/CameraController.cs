using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	public Transform target;

	void Update()
	{
		if (target != null)
			transform.LookAt( target );
	}
}

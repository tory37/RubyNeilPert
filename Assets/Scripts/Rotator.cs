using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public bool random;

	public Vector3 minRotation;
	public Vector3 maxRotation;

	public float changeTime;

	private Vector3 rotation;

	void Start()
	{
		StartCoroutine(ChangeRotation());
	}

	void Update()
	{
		if (random)
			transform.Rotate(rotation * Time.deltaTime);
		else
			transform.Rotate(maxRotation * Time.deltaTime);
	}

	IEnumerator ChangeRotation()
	{
		while ( true )
		{
			rotation = new Vector3(Random.Range(minRotation.x, maxRotation.x), Random.Range(minRotation.y, maxRotation.y), Random.Range(minRotation.z, maxRotation.z));

			yield return new WaitForSeconds(changeTime);
		}
	}
}

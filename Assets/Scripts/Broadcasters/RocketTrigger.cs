using UnityEngine;
using System.Collections;

public class RocketTrigger : Broadcaster {

	public static RocketTrigger Instance;

	void Awake ()
	{
		Instance = this;
	}

	void OnTriggerEnter (Collider other)
	{
		if ( other.transform.tag == "Rocket" )
			Broadcast( (int)Messages.HitExplodeBarrier );
	}
}

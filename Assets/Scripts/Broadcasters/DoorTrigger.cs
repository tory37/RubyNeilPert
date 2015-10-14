using UnityEngine;
using System.Collections;

public class DoorTrigger : Broadcaster {

	public static DoorTrigger Instance;

	void Awake ()
	{
		Instance = this;
	}

	void OnTriggerEnter(Collider other)
	{
		if ( other.transform.tag == "Door" )
			Broadcast( (int)Messages.OpenedDoor );
	}

}

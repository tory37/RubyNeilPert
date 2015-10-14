using UnityEngine;
using System.Collections;

public class HologramTrigger : Broadcaster {

	public static HologramTrigger Instance;

	void Awake ()
	{
		Instance = this;
	}

	void OnTriggerEnter(Collider other)
	{
		if ( other.transform.tag == "Dino" )
			Broadcast( (int)Messages.EnteredHologram );
	}

}

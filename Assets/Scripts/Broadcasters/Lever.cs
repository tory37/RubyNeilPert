using UnityEngine;
using System.Collections;

public class Lever : Broadcaster {

	public static Lever Instance;

	void Awake ()
	{
		Instance = this;
	}

	void OnCollisionEnter(Collision col)
	{
		if ( col.transform.tag == "Wood Balance" )
			Broadcast( (int)Messages.HitLevel );
	}

}

using UnityEngine;
using System.Collections;

public class BallButton : Broadcaster 
{

	public static BallButton Instance;

	void Awake ()
	{
		Instance = this;
	}

	void OnCollisionEnter(Collision col)
	{
		if ( col.transform.tag == "Ball" )
			Broadcast( (int)Messages.HitButton );
	}

}

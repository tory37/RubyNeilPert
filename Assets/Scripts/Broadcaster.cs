using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Broadcaster : MonoBehaviour
{

	protected List<IObserver> observers;

	protected void Awake()
	{
		observers = new List<IObserver>();
		Debug.Log("Creating list");
	}

	public void Subscribe(IObserver observer)
	{
		if ( observers == null )
			observers = new List<IObserver>();
		observers.Add( observer );
	}

	public void Unsubscribe(IObserver observer)
	{
		observers.Remove(observer);
	}
	protected void Broadcast(int message)
	{
		foreach (IObserver o in observers)
		{
			o.RecieveUpdate( message );
		}
	}

}

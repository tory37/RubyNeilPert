using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Broadcaster : MonoBehaviour
{

	protected List<IObserver> observers;

	protected void Start()
	{
		observers = new List<IObserver>();
	}

	public void Subscribe(IObserver observer)
	{
		observers.Add( observer );
	}

	public void Unsubscribe(IObserver observer)
	{

	}
	protected void Broadcast(int message)
	{
		foreach (IObserver o in observers)
		{
			o.RecieveUpdate( message );
		}
	}

}

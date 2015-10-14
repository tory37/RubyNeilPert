using UnityEngine;
using System.Collections.Generic;

public enum Messages
{
	HitButton,
	HitLevel,
	OpenedDoor,
	EnteredHologram,
	HitExplodeBarrier
}

public class SingleObserverFSM : MonoFSM
{
	public enum States
	{
		Start = 0,
		BallsFalling = 1,
		PushPeg = 2,
		OpenDoor = 3,
		MoveDino = 4,
		MoveRocket = 5,
		End = 6
	}

	#region Editor Interface

	[Header( "Objects" )]
	public GameObject ballBlocker;
	public Transform pusherPeg;
	public Transform dinoHorse;
	public Transform rocket;
	public Transform camera;
	public Transform hologram;
	public Transform door;
	public GameObject explosion;

	[Header( "Values" )]
	public float pusherPegSpeed;
	public float dinoSpeed;
	public float rocketSpeed;
	public float doorSpeed;

	public static SingleObserverFSM Instance
	{
		get {return instance;}
		set 
		{
			if (instance == null)
				instance = value;
			else
				Destroy(value.gameObject);
		}
	}
	private static SingleObserverFSM instance;

	#endregion

	void Awake()
	{
		Instance = this;
	}

	protected override void SetStates()
	{
		states = new Dictionary<int, State>
		{
			{ (int)States.Start, new StartState() },
			{ (int)States.BallsFalling, new BallsFallingState() },
			{ (int)States.PushPeg , new PushPegState() },
			{ (int)States.OpenDoor, new OpenDoorState() },
			{ (int)States.MoveDino, new MoveDinoState() },
			{ (int)States.MoveRocket, new MoveRocketState() },
			{ (int)States.End, new EndState() }
		};
	}

	protected override void Initialize()
	{
		BallButton.Instance.Subscribe( (IObserver)states[(int)States.BallsFalling] );
		DoorTrigger.Instance.Subscribe( (IObserver)states[(int)States.OpenDoor] );
		HologramTrigger.Instance.Subscribe( (IObserver)states[(int)States.MoveDino] );
		Lever.Instance.Subscribe( (IObserver)states[(int)States.PushPeg] );
		RocketTrigger.Instance.Subscribe( (IObserver)states[(int)States.MoveRocket] );
	}
}

public class StartState : State
{
	private SingleObserverFSM fsm;

	public override void Initialize(MonoFSM callingfsm)
	{
		fsm = SingleObserverFSM.Instance;
	}

	public override void CheckTransitions()
	{
		if ( Input.GetKeyDown( KeyCode.Space ) )
			fsm.Transition( (int)SingleObserverFSM.States.BallsFalling );

	}

	public override void OnExit()
	{
		GameObject.Destroy( fsm.ballBlocker );
	}
}

public class BallsFallingState : State, IObserver
{
	private SingleObserverFSM fsm;

	private bool transition = false;

	public override void OnEnter()
	{
		Debug.Log( "Entering Balls Falling" );
	}

	public override void Initialize(MonoFSM callingfsm)
	{
		fsm = SingleObserverFSM.Instance;
	}

	public void RecieveUpdate(int message)
	{
		if ( message == (int)Messages.HitButton )
			transition = true;
	}

	public override void CheckTransitions()
	{
		if ( transition )
			fsm.Transition( (int)SingleObserverFSM.States.PushPeg );
	}
}

public class PushPegState : State, IObserver
{
	private SingleObserverFSM fsm;

	private bool transition = false;

	private Rigidbody pusherPegRb;

	public override void Initialize(MonoFSM callingfsm)
	{
		fsm = SingleObserverFSM.Instance;
		pusherPegRb = fsm.pusherPeg.GetComponent<Rigidbody>();
	}

	public void RecieveUpdate(int message)
	{
		if ( message == (int)Messages.HitLevel )
			transition = true;
	}

	public override void OnEnter()
	{
		Debug.Log( "Entering Push Peg" );
		Camera.main.GetComponent<CameraController>().target = fsm.pusherPeg;
	}

	public override void OnFixedUpdate()
	{
		pusherPegRb.MovePosition(pusherPegRb.position + Vector3.up * fsm.pusherPegSpeed * Time.deltaTime);
	}

	public override void CheckTransitions()
	{
		if ( transition )
			fsm.Transition( (int)SingleObserverFSM.States.OpenDoor );
	}
}

public class OpenDoorState : State, IObserver
{
	private SingleObserverFSM fsm;

	private bool transition = false;

	public override void Initialize(MonoFSM callingfsm)
	{
		fsm = SingleObserverFSM.Instance;
	}

	public override void OnEnter()
	{
		Debug.Log( "Entering Open Door" );
		Camera.main.GetComponent<CameraController>().target = fsm.dinoHorse;
	}

	public void RecieveUpdate(int message)
	{
		if ( message == (int)Messages.OpenedDoor )
			transition = true;
	}

	public override void OnUpdate()
	{
		fsm.door.Translate(Vector3.up * fsm.doorSpeed * Time.deltaTime);
	}

	public override void CheckTransitions()
	{
		if ( transition )
			fsm.Transition( (int)SingleObserverFSM.States.MoveDino );
	}
}

public class MoveDinoState : State, IObserver
{
	private SingleObserverFSM fsm;

	private bool transition = false;

	public override void Initialize(MonoFSM callingfsm)
	{
		fsm = SingleObserverFSM.Instance;
	}

	public void RecieveUpdate(int message)
	{
		if ( message == (int)Messages.EnteredHologram )
			transition = true;
	}

	public override void OnEnter()
	{
		Debug.Log( "Entering Move Dino" );
	}

	public override void OnUpdate()
	{
		fsm.dinoHorse.position = Vector3.MoveTowards(fsm.dinoHorse.position, fsm.hologram.position, fsm.dinoSpeed * Time.deltaTime);
	}

	public override void CheckTransitions()
	{
		if ( transition )
			fsm.Transition( (int)SingleObserverFSM.States.MoveRocket );
	}
}

public class MoveRocketState : State, IObserver
{
	private SingleObserverFSM fsm;

	private bool transition = false;

	public override void Initialize(MonoFSM callingfsm)
	{
		fsm = SingleObserverFSM.Instance;
	}

	public void RecieveUpdate(int message)
	{
		if ( message == (int)Messages.HitExplodeBarrier )
			transition = true;
	}

	public override void OnEnter()
	{
		Debug.Log( "Entering Move Rocket" );
		Camera.main.GetComponent<CameraController>().target = fsm.rocket;
	}

	public override void OnUpdate()
	{
		fsm.rocket.Translate( Vector3.up * fsm.rocketSpeed * Time.deltaTime);
	}

	public override void CheckTransitions()
	{
		if ( transition )
			fsm.Transition( (int)SingleObserverFSM.States.End );
	}
}

public class EndState : State
{

	private SingleObserverFSM fsm;

	public override void Initialize(MonoFSM callingfsm)
	{
		fsm = SingleObserverFSM.Instance;
	}

	public override void OnEnter()
	{
		Debug.Log( "Entering End" );

		Vector3 position = fsm.rocket.position;
		GameObject.Destroy(fsm.rocket.gameObject);
		GameObject.Instantiate( fsm.explosion, position, Quaternion.identity );

		Debug.Log( "Done Bitch" );
	}
}
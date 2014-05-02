using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public class PlayerInput
	{
		public bool jump;
		public bool shoot;
		public int horizontal;
		public Vector2 aimDirection;
	}

	public PlayerInput input = new PlayerInput();

	public Animator animator;
	public Animator gunAnimator;
	public CollisionEventSender groundEventSender; 	

	public float movementSpeed;
	public float jumpStrength;

	public float jumpDeceleration;

	public Vector2 gravity;

	public uint shotsLeft;

	public float currentJumpForce = 0.0f;

	int asdaf = 0;

	void OnEnable()
	{
		Player me = this;

		groundEventSender.TriggerEnter2D += (CollisionEventSender in_trigger, Collider2D in_other) => {
			me._onGround = true;
			++ asdaf;
			Debug.Log(asdaf + " ENTER");
		};

		groundEventSender.TriggerStay2D += (CollisionEventSender in_trigger, Collider2D in_other) => {
			me._onGround = true;
			++ asdaf;
			Debug.Log(asdaf + " STAY");
		};

		groundEventSender.TriggerExit2D += (CollisionEventSender in_trigger, Collider2D in_other) => {
			me._onGround = false;
			++ asdaf;
			Debug.Log(asdaf + " EXIT");
		};
	}

	void OnDisable()
	{

	}

	// returns the world position of the wielded gun
	public Vector2 gunPosition
	{
		get {
			return transform.position;
		}
	}

	public bool onGround {

		get {
			return _onGround;
		}

	}

	private bool _onGround;
}

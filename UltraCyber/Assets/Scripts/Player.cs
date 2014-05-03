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

	public GameObject graphics;
	public Transform gunOrigin;

	public PlayerInput input = new PlayerInput();

	public Animator animator;
	public Animator gunAnimator;
	public CollisionEventSender groundEventSender; 	

	public float movementForce;
	public float jumpForce;

	public uint shotsLeft;

	void OnEnable()
	{
		Player me = this;

		groundEventSender.TriggerEnter2D += (CollisionEventSender in_trigger, Collider2D in_other) => {
			me._onGround = true;
		};

		groundEventSender.TriggerStay2D += (CollisionEventSender in_trigger, Collider2D in_other) => {
			me._onGround = true;
		};

		groundEventSender.TriggerExit2D += (CollisionEventSender in_trigger, Collider2D in_other) => {
			me._onGround = false;
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

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	[System.Serializable]
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
	public float jumpDeceleration;
	public float currentJumpForce;

	public float score;
	public int lastScoreInt;

	public float weaponCooldown;

	public uint shotsLeft;

	[SerializeField]
	public SpriteRenderer bodyRenderer;
	[SerializeField]
	public SpriteRenderer gunRenderer;

	public SpriteRenderer diskIndicator;

	public float dataCooldown;

	public Color tint {

		get {
			return bodyRenderer.color;
		}

		set {
			bodyRenderer.color = value;
			gunRenderer.color = value;
		}
	}

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

	public bool dead = false;
	
	public bool onGround {

		get {
			return _onGround;
		}

	}

	private bool _onGround;
}

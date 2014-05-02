using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
	// temp
	public GUISkin debugGUISkin;
	public GameObject playerPrefab;

	public Config config;

	public Player[] players;

	void OnEnable()
	{
		int numPlayers = 1;

		players = new Player[numPlayers];

		for (int i = 0; i < numPlayers; ++i)
		{
			Player player = InstantiatePlayer();
			players[i] = player;

			RespawnPlayer(player, FindSpawnPoint());
		}
	}

	void OnDisable()
	{
		foreach (Player player in players)
		{
			if (player && player.gameObject)
				Destroy(player.gameObject);
		}
	}

	Player InstantiatePlayer()
	{
		GameObject playerGo = (GameObject)Instantiate(playerPrefab);
		// do any other initial setup here
		Player player = playerGo.GetComponent<Player>();

		return player;
	}

	void RespawnPlayer(Player player, Vector2 position)
	{
		player.movementForce = config.playerMovementForce;
		player.jumpForce = config.playerJumpForce;
	}

	Vector2 FindSpawnPoint()
	{
		return Vector2.zero;
	}

	void Update()
	{
		UpdateInput();
	}

	void FixedUpdate()
	{
		for (int i = 0; i < players.Length; ++i)
		{
			UpdatePlayer(players[i]);
		}
	}

	void UpdateInput()
	{
		for (uint i = 0; i < players.Length; ++i)
		{
			Player player = players[(int)i];
			player.input.horizontal = GameInput.GetXboxAxis(i, GameInput.Xbox360Axis.DpadX);
			player.input.jump = GameInput.GetXboxButton(i, GameInput.Xbox360Button.A);
			player.input.shoot = GameInput.GetXboxButton(i, GameInput.Xbox360Button.B);
			player.input.aimDirection = new Vector2(player.input.horizontal, GameInput.GetXboxAxis(i, GameInput.Xbox360Axis.DpadY));
		}
	}

	string debugString ="";

	void UpdatePlayer(Player player)
	{
		var rigidBody = player.GetComponent<Rigidbody2D>();
		Animator animator = player.animator;
//
//		player.gravity.Normalize();
//
//		if (player.currentJumpForce > 0.0f)
//		{
//			player.currentJumpForce -= player.jumpDeceleration * Time.deltaTime;
//			if (player.currentJumpForce < 0.0f)
//				player.currentJumpForce = 0.0f;
//		}
//
//		Vector2 gravPerp = new Vector2(-player.gravity.y, player.gravity.x);
//
//		debugString = "" + gravPerp;
//
//		Vector2 finalMovement = Vector2.zero;
//		//if (player.onGround)

//
//		if (rigidBody.velocity.magnitude < player.movementSpeed)
//			rigidBody.AddForce(gravPerp * (((float)player.input.horizontal) * player.movementAcceleration * Time.deltaTime));
//		//finalMovement += gravPerp * (((float)player.input.horizontal) * player.movementSpeed * Time.deltaTime);
//		if (Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon)
//		{
//			float decrease = player.movementDeceleration * Time.deltaTime;
//			if (rigidBody.velocity.x < decrease)
//			{
//				rigidBody.velocity = new Vector2(0.0f, rigidBody.velocity.y);
//			}
//			else
//			{
//				rigidBody.AddForce(new Vector2(-Mathf.Sign(rigidbody.velocity.x) * decrease, 0.0f));
//			}
//		}
//
//
//		if (!player.onGround)
//			rigidBody.AddForce(player.gravity * (config.playerGravityStrength * Time.deltaTime));
//
//		if (player.input.jump && player.onGround)
//			player.currentJumpForce = player.jumpStrength;
//
//		if (player.currentJumpForce > 0.0f)
//		{
//			rigidBody.AddForce(-player.gravity * (player.currentJumpForce * Time.deltaTime));
//		}
//
//		Vector2 dir = finalMovement.normalized;
//

		rigidBody.AddForce(Vector2.right * (((float)player.input.horizontal) * player.movementForce * Time.deltaTime));

		if (player.input.jump && player.onGround)
		{
			// impulse
			rigidBody.AddForce(Vector2.up * (player.jumpForce / Time.deltaTime));
		}

		Vector2 dir = rigidBody.velocity.normalized;
		float dot = Vector2.Dot(dir, Vector3.down);

		if (!player.onGround)
		{
			if (dot > 0.9f)
			{
				//debugString = "fall";
				animator.Play("PlayerFall");
			}
			else if (dot < -0.9f)
			{
				//debugString = "jump";
				animator.Play("PlayerJump");
			}
		}
		else
		{
			if (rigidBody.velocity.magnitude > 0.05f && (Vector2.Dot(dir, Vector2.right) > 0.7f || Vector2.Dot(dir, Vector2.right) < -0.7f))
			{
				//debugString = "walk";
				animator.Play("PlayerWalk");
			}
			else
			{
				//debugString = "idle";
				animator.Play("PlayerIdle");
			}
		}

		if (player.input.horizontal != 0)
		{
			player.graphics.transform.localScale = new Vector3(player.input.horizontal > 0 ? 1.0f : -1.0f, 1.0f, 1.0f); 
		}

		player.input.horizontal = 0;
		player.input.jump = false;
		player.input.shoot = false;
		debugString = "" + rigidBody.velocity;

		float maxSpeed = config.maxSpeed;

		if (rigidBody.velocity.sqrMagnitude >= maxSpeed * maxSpeed)
			rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
	}

	void OnGUI()
	{
		GUI.skin = debugGUISkin;
		GUILayout.Label(string.IsNullOrEmpty(debugString) ? "HERRO WROLD" : debugString);
		GUI.skin = null;
	}
}

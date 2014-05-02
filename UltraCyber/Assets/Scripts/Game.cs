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
		player.movementSpeed = config.playerMovementSpeed;
		player.jumpStrength = config.playerJumpForce;
		player.gravity = config.playerGravity;
		player.currentJumpForce = 0.0f;
		player.jumpDeceleration = config.playerJumpDeceleration;
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
		Animator animator = player.animator;

		player.gravity.Normalize();

		if (player.currentJumpForce > 0.0f)
		{
			player.currentJumpForce -= player.jumpDeceleration * Time.deltaTime;
			if (player.currentJumpForce < 0.0f)
				player.currentJumpForce = 0.0f;
		}
		Vector2 gravPerp = new Vector2(-player.gravity.y, player.gravity.x);

		debugString = "" + gravPerp;

		Vector2 finalMovement = Vector2.zero;
		//if (player.onGround)
		finalMovement += gravPerp * (((float)player.input.horizontal) * player.movementSpeed * Time.deltaTime);

		if (!player.onGround)
			finalMovement += player.gravity * (config.playerGravityStrength * Time.deltaTime);

		if (player.input.jump && player.onGround)
			player.currentJumpForce = player.jumpStrength;

		finalMovement += -player.gravity * (player.currentJumpForce * Time.deltaTime);

		player.transform.position = player.transform.position + (Vector3)finalMovement;

		Vector2 dir = finalMovement.normalized;

		float dot = Vector2.Dot(dir, player.gravity);

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
			if (finalMovement.sqrMagnitude > 0.0f && (Vector2.Dot(dir, gravPerp) > 0.7f || Vector2.Dot(dir, gravPerp) < -0.7f))
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
			player.transform.localScale = new Vector3(player.input.horizontal > 0 ? 1.0f : -1.0f, 1.0f, 1.0f); 
		}

		player.input.horizontal = 0;
		player.input.jump = false;
		player.input.shoot = false;
	}

	void OnGUI()
	{
		GUI.skin = debugGUISkin;
		GUILayout.Label(string.IsNullOrEmpty(debugString) ? "HERRO WROLD" : debugString);
		GUI.skin = null;
	}
}

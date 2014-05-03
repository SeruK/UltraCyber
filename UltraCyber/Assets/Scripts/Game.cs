using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
	[SerializeField]
	public EffectSpawner effectSpawner;
	// temp
	public GUISkin debugGUISkin;
	public GameObject playerPrefab;
	public GameObject bulletPrefab;
	public GameObject noBulletPrefab;
	public GameObject muzzleFlashPrefab;

	public Config config;

	public Player[] players;

	void OnEnable()
	{
		int numPlayers = 2;

		players = new Player[numPlayers];

		for (int i = 0; i < numPlayers; ++i)
		{
			Player player = InstantiatePlayer("Player " + i);
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

	void DetachAndDestroyAfter(Component obj, float delay)
	{
		StartCoroutine(_DetachAndDestroyAfter(obj, delay));
	}

	IEnumerator _DetachAndDestroyAfter(Component obj, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (obj)
		{
			obj.transform.parent = null;
			Destroy (obj.gameObject);
		}
	}

	void DestroyAfter(UnityEngine.Object obj, float delay)
	{
		StartCoroutine(_DestroyAfter(obj, delay));
	}

	IEnumerator _DestroyAfter(UnityEngine.Object obj, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (obj)
			Destroy(obj);
	}

	Player InstantiatePlayer(string name)
	{
		GameObject playerGo = (GameObject)Instantiate(playerPrefab);
		playerGo.name = name;
		// do any other initial setup here
		Player player = playerGo.GetComponent<Player>();

		return player;
	}

	void RespawnPlayer(Player player, Vector2 position)
	{
		player.movementForce = config.playerMovementForce;
		player.jumpForce = config.playerJumpForce;
		player.transform.position = FindSpawnPoint();
		player.shotsLeft = (uint)config.shots;
	}

	Vector2 FindSpawnPoint()
	{
		return new Vector2(1,2.015f);// Vector2.zero;
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.A)) 
			effectSpawner.SpawnExplosion(new Vector2(1,1));
			
		UpdateInput();
	}

	void FixedUpdate()
	{
		for (int i = 0; i < players.Length; ++i)
		{
			UpdatePlayer(players[i]);
		}
	}

	private bool DirKeyHeld() {
		return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);
	}
	private int GetKeyDir(){
		if (Input.GetKey(KeyCode.LeftArrow))
			return -1;
		else
			return 1;
	}

	void UpdateInput()
	{
		for (uint i = 0; i < players.Length; ++i)
		{
			Player player = players[(int)i];
			player.input.horizontal = DirKeyHeld() ? GetKeyDir() : GameInput.GetXboxAxis(i, GameInput.Xbox360Axis.DpadX);
			player.input.jump = GameInput.GetXboxButtonDown(i, GameInput.Xbox360Button.A) || Input.GetKeyDown(KeyCode.Space);
			player.input.shoot = GameInput.GetXboxButtonDown(i, GameInput.Xbox360Button.B) || Input.GetKeyDown(KeyCode.X);
			player.input.aimDirection = new Vector2(player.input.horizontal, GameInput.GetXboxAxis(i, GameInput.Xbox360Axis.DpadY));
		}
	}

	string debugString ="";

	void UpdatePlayer(Player player)
	{
		if (player.input.shoot)
		{
			var dirr = GetWeaponDirection(player);
			float angle = Mathf.Atan2(dirr.y, dirr.x) * Mathf.Rad2Deg;
			var weaponDir = Quaternion.AngleAxis(angle, Vector3.forward);

			if (player.shotsLeft > 1u)
			{
				--player.shotsLeft;
				ShootBullet(player);
				var muzzleFlash = Instantiate(muzzleFlashPrefab, player.gunOrigin.position, weaponDir) as GameObject;
				muzzleFlash.transform.parent = player.graphics.transform;
				DetachAndDestroyAfter(muzzleFlash.transform, 0.1f);
			}
			else
			{
				var noBullet = Instantiate(noBulletPrefab, player.gunOrigin.position, weaponDir) as GameObject;
				noBullet.transform.parent = player.graphics.transform;
				DetachAndDestroyAfter(noBullet.transform, 0.2f);
			}
		}

		var rigidBody = player.GetComponent<Rigidbody2D>();
		Animator animator = player.animator;

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

		if (Vector2.Dot(player.input.aimDirection, Vector2.up) > 0.5f)
		{
			player.gunAnimator.Play("GunUp");
		}
		else if (Vector2.Dot(player.input.aimDirection, Vector2.up) < -0.5f)
		{
			player.gunAnimator.Play("GunDown");
		}
		else
		{
			player.gunAnimator.Play("GunSide");
		}
	}

	void ShootBullet(Player player)
	{
		var bulletGo = Instantiate(bulletPrefab, player.gunOrigin.position, Quaternion.identity) as GameObject;
		var rigid = bulletGo.GetComponent<Rigidbody2D>();

		Vector2 dir = GetWeaponDirection(player);

		rigid.AddForce(dir * (config.bulletForce / Time.deltaTime) + player.GetComponent<Rigidbody2D>().velocity);
		bulletGo.GetComponent<CollisionEventSender>().CollisionEnter2D += BulletImpact;
		DestroyAfter(bulletGo, config.bulletLife);
	}

	Vector2 GetWeaponDirection(Player player)
	{
		if (Vector2.Dot(player.input.aimDirection, Vector2.up) > 0.5f)
		{
			return Vector2.up;
		}
		else if (Vector2.Dot(player.input.aimDirection, Vector2.up) < -0.5f)
		{
			return -Vector2.up;
		}
		else
		{
			return new Vector2(player.graphics.transform.localScale.x, 0.0f);
		}
	}

	void BulletImpact(CollisionEventSender sender, Collision2D collision)
	{
		var rigid = collision.rigidbody;

		if (rigid)
		{
			rigid.AddForce(collision.relativeVelocity.normalized * (config.bulletImpactForce / Time.deltaTime));
		}

		Destroy(sender.gameObject);
	}

	void OnGUI()
	{
		GUI.skin = debugGUISkin;
		GUILayout.Label(string.IsNullOrEmpty(debugString) ? "HERRO WROLD" : debugString);
		GUI.skin = null;
	}
}

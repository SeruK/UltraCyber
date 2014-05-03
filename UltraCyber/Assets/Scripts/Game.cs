﻿using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
	public enum DataHolder
	{
		None,
		PlayerOne,
		PlayerTwo
	}

	public DataHolder dataHolder;

	[SerializeField]
	public EffectSpawner effectSpawner;
	
	[System.Serializable]
	public class CustomAudioClip
	{
		public AudioClip clip;
		public float volume = 1.0f;
		public float pitch = 1.0f;
	}

	// temp
	public GUISkin debugGUISkin;
	public GameObject playerPrefab;
	public GameObject bulletPrefab;
	public GameObject noBulletPrefab;
	public GameObject muzzleFlashPrefab;
	public GameObject dataPrefab;

	// TODO move this somewhere
	public CustomAudioClip spawnClip;
	public CustomAudioClip shootClip;
	public CustomAudioClip impactClip;
	public CustomAudioClip impactPlayerClip;
	public CustomAudioClip footstepClip;
	public CustomAudioClip jumpClip;
	public CustomAudioClip clickClip;
	public CustomAudioClip dataClip;
	public CustomAudioClip dataDropClip;

	public Config config;

	public SpriteRenderer playerOneWinsIndicator;
	public SpriteRenderer playerTwoWinsIndicator;
	public SpriteRenderer drawIndicator;
	public SpriteRenderer startToPlayIndicator;

	public Player[] players;

	public Camera camera;

	public MapLoaderTurbo mapLoader;

	private float footstepCooldown;
	private GameObject data;

	void OnEnable()
	{
		Restart();
	}

	void OnDisable()
	{
		if (data)
			Destroy (data);
		RemovePlayers();
		mapLoader.Clear();
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

	void RemovePlayers()
	{
		if (players == null)
			return;

		foreach (Player player in players)
		{
			if (player && player.gameObject)
				Destroy(player.gameObject);
		}

		players = null;
	}

	void Restart()
	{
		if (data)
			Destroy (data);
		RemovePlayers();

		playerOneWinsIndicator.enabled = false;
		playerTwoWinsIndicator.enabled = false;
		drawIndicator.enabled = false;
		startToPlayIndicator.enabled = false;

		gameEnd = false;

		dataHolder = DataHolder.None;

		mapLoader.Recreate();

		int numPlayers = 2;
		
		players = new Player[numPlayers];
		
		for (int i = 0; i < numPlayers; ++i)
		{
			Player player = InstantiatePlayer("Player " + i);
			players[i] = player;
			player.tint = config.playerColors[i];

			player.gameObject.layer = i == 0 ? 8 : 13;



			Vector2 spawnPoint = i == 0 ? new Vector2(1, 2.015f) : new Vector2(9, 2.015f);
			RespawnPlayer(player, spawnPoint);

		}

		camera.transform.position = new Vector3(camera.transform.position.x, config.cameraStartY, camera.transform.position.z);
	}

	Player InstantiatePlayer(string name)
	{
		GameObject playerGo = (GameObject)Instantiate(playerPrefab);
		playerGo.name = name;
		// do any other initial setup here
		Player player = playerGo.GetComponent<Player>();

		return player;
	}

	void TryInstantiateData()
	{
		if (data)
			return;

		Vector2 spawnPoint;
		if (FindDataSpawnPoint(out spawnPoint))
		{
			data = Instantiate(dataPrefab, spawnPoint, Quaternion.identity) as GameObject;
			var eventSender = data.GetComponent<CollisionEventSender>();
			eventSender.TriggerEnter2D += DataImpact;
		}
	}

	void RespawnPlayer(Player player, Vector2 position)
	{
		PlayClipAtPoint(spawnClip, position, 1.0f);

		player.movementForce = config.playerMovementForce;
		player.jumpForce = config.playerJumpForce;
		player.jumpDeceleration = config.playerJumpDeceleration;
		player.transform.position = position;
		player.shotsLeft = (uint)config.shots;
		player.dead = false;
		player.graphics.GetComponent<SpriteRenderer>().enabled = true;
	}

	bool FindSpawnPoint(out Vector2 spawnPoint, int offset)
	{
		int camY = Mathf.CeilToInt(camera.transform.position.y + offset);

		while (true)
		{
			if (camY >= config.mapHeight)
			{
				--camY;
				continue;
			}

			if (camY < 0)
				break;

			var list = mapLoader.GetRow(camY);
			var lowestList = camY == 0 ? null : mapLoader.GetRow(camY - 1);
			for (int x = 0; x < list.Count; ++x)
			{
				bool superCont = false;
				foreach (Player playaaa in players)
				{
					if (Mathf.FloorToInt(playaaa.transform.position.x) == x &&
					    Mathf.FloorToInt(playaaa.transform.position.y) == camY)
					{
						superCont = true;
						break;
					}
				}

				if (superCont)
					continue;

				MapLoaderTurbo.BlockType bt = list[x];
				MapLoaderTurbo.BlockType btLower = lowestList == null ? MapLoaderTurbo.BlockType.Filled : lowestList[x];
				if (bt == MapLoaderTurbo.BlockType.Empty && btLower == MapLoaderTurbo.BlockType.Filled)
				{
					spawnPoint = new Vector2(-1.0f + (float)x, camY);
					return true;
				}
			}

			--camY;
		}

		spawnPoint = new Vector2(1, 2.015f);
		return false;// Vector2.zero;
	}

	bool FindDataSpawnPoint(out Vector2 spawnPoint)
	{
		return FindSpawnPoint(out spawnPoint, 6);
	}

	void KillPlayer(Player player)
	{
		for (int i = 0; i < players.Length; ++i)
		{
			if (players[i] == player)
			{
				if (dataHolder == (DataHolder)(i + 1))
				{
					dataHolder = DataHolder.None;
					PlayClip(dataDropClip);
					break;
				}
			}
		}

		player.dead = true;
		player.graphics.GetComponent<SpriteRenderer>().enabled = false;
	}

	void PlayClip(CustomAudioClip clip)
	{
		PlayClipAtPoint(clip, Vector2.zero, 1.0f);
	}

	void PlayClipAtPoint(CustomAudioClip clip, Vector2 pos, float volume)
	{
		if (!clip.clip)
			return;

		GameObject go = new GameObject(clip.clip.name);

		AudioSource audio = go.AddComponent<AudioSource>();

		audio.volume = clip.volume;
		audio.pitch = clip.pitch;
		audio.PlayOneShot(clip.clip);

		DestroyAfter(go, clip.clip.length + 0.1f);

		//AudioSource.PlayClipAtPoint(clip.clip, Vector3.zero, volume);
	}

	void Update()
	{
		if (gameEnd)
		{
			for (uint i = 0; i < 2; ++i)
			{
				if (GameInput.GetXboxButtonDown(i, GameInput.Xbox360Button.Start))
				{
					Restart ();
				}
			}
			return;
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			Restart();
			return;
		}
#if UNITY_EDITOR
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			KillPlayer(players[0]);
			return;
		}
#endif

		footstepCooldown -= Time.deltaTime;
		if (Input.GetKeyUp(KeyCode.A)) 
			effectSpawner.SpawnExplosion(new Vector2(1,1));

		if (dataHolder == DataHolder.None && !data)
		{
			TryInstantiateData();
		}

		foreach (Player p in players)
		{
			if (!p.dead)
				continue;

			Vector2 spawnPoint;
			if (FindSpawnPoint(out spawnPoint, 4))
			{
				RespawnPlayer(p, spawnPoint);
			}
		}

		UpdateInput();

		switch (dataHolder)
		{
		case DataHolder.None:
			if (data)
				debugString = "Get the data!";
			else
				debugString = "";
			players[1].diskIndicator.enabled = players[0].diskIndicator.enabled = false;
			break;

		case DataHolder.PlayerOne:
			players[0].score += config.pointsPerSecond * Time.deltaTime;
			players[0].diskIndicator.enabled = true;
			debugString = "P1 has the data! Score: " + ((int)players[0].score).ToString();
			break;

		case DataHolder.PlayerTwo:
			players[1].score += config.pointsPerSecond * Time.deltaTime;
			players[1].diskIndicator.enabled = true;
			debugString = "P2 has the data! Score: " + ((int)players[1].score).ToString();
			break;

		default:
			Debug.LogError("derp");
			break;
		}
	}

	void FixedUpdate()
	{
		if (gameEnd)
		{
			return;
		}

		camera.transform.position += new Vector3(0.0f, config.cameraPanSpeed * Time.deltaTime, 0.0f);

		float maxY = 0;

		foreach (Player p in players)
		{
			if (p.transform.position.y > maxY)
				maxY = p.transform.position.y;
		}

		if (maxY > camera.transform.position.y)
		{
			camera.transform.position += new Vector3(0.0f, config.cameraPanSpeed * 3.0f * Time.deltaTime, 0.0f);
		}

		camera.transform.position = new Vector3(camera.transform.position.x,
		                                        Mathf.Clamp(camera.transform.position.y, config.cameraStartY, config.cameraEndY),
		                                        camera.transform.position.z);


		for (int i = 0; i < players.Length; ++i)
		{
			var p = players[i];

			if (p.transform.position.y < camera.transform.position.y - 6)
			{
				KillPlayer(p);
			}
			else if (p.transform.position.y >= mapLoader.hoverCraft.transform.position.y)
			{
				EndGame();
				break;
			}

			UpdatePlayer(players[i]);
		}
	}

	private bool DirKeyHeld() {
		return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);
	}

	private bool VertDirKeysHeld() {
		return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow);
	}

	private int GetKeyDir(){
		if (Input.GetKey(KeyCode.LeftArrow))
			return -1;
		else
			return 1;
	}

	private int GetVertKeyDir() {
		return Input.GetKey(KeyCode.UpArrow) ? 1 : -1;
	}

	bool gameEnd = false;

	void EndGame()
	{
		if (gameEnd)
			return;

		gameEnd = true;

		for (int i = 0; i < players.Length; ++i)
		{
			KillPlayer(players[i]);
		}

		int p1 = Mathf.FloorToInt(players[0].score);
		int p2 = Mathf.FloorToInt(players[1].score);

		startToPlayIndicator.enabled = true;

		if (p1 == p2)
		{
			debugString = "Draw! Everyone's a winner! Start to play again";
			Debug.Log("DRAW!!!!");

			drawIndicator.enabled = true;
		}
		else if (p1 > p2)
		{
			playerOneWinsIndicator.enabled = false;
			debugString = "Player one wins! Start to play again";
			Debug.Log("P1 WINS!!!!");
		}
		else
		{
			playerTwoWinsIndicator.enabled = false;
			debugString = "Player two wins! Start to play again";
			Debug.Log("P2 WINS!!!!");
		}
	}

	void UpdateInput()
	{
		for (uint i = 0; i < players.Length; ++i)
		{
			Player player = players[(int)i];

			if (player.dead)
				continue;

			player.input.horizontal = DirKeyHeld() ? GetKeyDir() : GameInput.GetXboxAxis(i, GameInput.Xbox360Axis.DpadX);
			player.input.jump = GameInput.GetXboxButton(i, GameInput.Xbox360Button.A) || Input.GetKey(KeyCode.Space);
			player.input.shoot = GameInput.GetXboxButtonDown(i, GameInput.Xbox360Button.B) || Input.GetKeyDown(KeyCode.X);
			player.input.aimDirection = new Vector2(player.input.horizontal, 
			                                        VertDirKeysHeld() ? GetVertKeyDir() : GameInput.GetXboxAxis(i, GameInput.Xbox360Axis.DpadY));
		}
	}

	string debugString ="";

	void UpdatePlayer(Player player)
	{
		if (player.dead)
			return;

		if (player.weaponCooldown > 0.0f)
		{
			player.weaponCooldown -= Time.deltaTime;
		}

		if (player.input.shoot && player.weaponCooldown <= 0.0f)
		{
			var dirr = GetWeaponDirection(player);
			float angle = Mathf.Atan2(dirr.y, dirr.x) * Mathf.Rad2Deg;
			var weaponDir = Quaternion.AngleAxis(angle, Vector3.forward);
			player.weaponCooldown = config.weaponCooldown;

			/*if (player.shotsLeft > 1u)
			{*/
				//--player.shotsLeft;
				ShootBullet(player);
				var muzzleFlash = Instantiate(muzzleFlashPrefab, player.gunOrigin.position, weaponDir) as GameObject;
				muzzleFlash.transform.parent = player.graphics.transform;
				DetachAndDestroyAfter(muzzleFlash.transform, 0.1f);
				PlayClipAtPoint(shootClip, player.gunOrigin.position, 1.0f);
			/*}
			else
			{
				var noBullet = Instantiate(noBulletPrefab, player.gunOrigin.position, weaponDir) as GameObject;
				noBullet.transform.parent = player.graphics.transform;
				DetachAndDestroyAfter(noBullet.transform, 0.2f);
				PlayClip(clickClip);
			}*/
		}

		var rigidBody = player.GetComponent<Rigidbody2D>();
		Animator animator = player.animator;

		rigidBody.AddForce(Vector2.right * (((float)player.input.horizontal) * player.movementForce * Time.deltaTime));

		if (player.input.jump && player.onGround)
		{
			// impulse
			//rigidBody.AddForce(Vector2.up * (player.jumpForce / Time.deltaTime));
			player.currentJumpForce = player.jumpForce;
			PlayClipAtPoint(jumpClip, rigidBody.transform.position, 1.0f);
		}

		if (player.currentJumpForce > 0.0f)
		{
			rigidBody.AddForce(Vector2.up * (player.currentJumpForce * Time.deltaTime));
			player.currentJumpForce -= player.jumpDeceleration * Time.deltaTime;
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
				if (footstepCooldown <= 0.0f)
				{
					footstepCooldown = 0.2f;
					PlayClipAtPoint(footstepClip, Vector2.zero, 0.2f);
				}
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

		var bullet = bulletGo.GetComponent<Bullet>();

		for (int i = 0; i < players.Length; ++i)
		{
			if (player == players[i])
			{
				if (i == 1)
				{
					bullet.gameObject.layer = 12;
				}
				break;
			}
		}

		bullet.onHit += (Bullet bull, Collision2D coll) => {
			var playa = coll.gameObject.GetComponent<Player>();
			if (playa)
			{
				playa.rigidbody2D.AddForce(coll.relativeVelocity.normalized * (config.bulletImpactForce / Time.deltaTime));
				PlayClip(impactPlayerClip);
			}
			else
			{
				PlayClip(impactClip);
			}
		};

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

	void DataImpact(CollisionEventSender sender, Collider2D other)
	{
		Player p = other.GetComponent<Player>();

		if (!p)
			return;

		for (int i = 0; i < players.Length; ++i)
		{
			if (p == players[i])
			{
				dataHolder = (DataHolder)(i + 1);
				PlayClip(dataClip);
				Destroy(data);
				break;
			}
		}
	}

	void OnGUI()
	{
		GUI.skin = debugGUISkin;
		GUILayout.Label(string.IsNullOrEmpty(debugString) ? "HERRO WROLD" : debugString);

//		if (players != null)
//		{
//			if (players.Length > 0)
//				GUI.Label(new Rect(10.0f, Screen.height - 50.0f, Screen.width, 40.0f), "" + players[0].score);
//			if (players.Length > 1)
//				GUI.Label(new Rect(Screen.width - 210.0f, Screen.height - 50.0f, 200.0f, 40.0f), "" + players[1].score);
//		} 

		GUI.skin = null;
	}
}

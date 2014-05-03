using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour
{
	public float playerMovementForce;
	public float playerJumpForce;
	public float playerJumpDeceleration;

	public float maxSpeed = 10.0f;
	public float pointsPerSecond = 2.0f;
	public int shots = 6;

	public float bulletForce;
	public float bulletLife;
	public float bulletImpactForce;

	public float cameraPanSpeed;
	public float cameraStartY;
	public float cameraEndY;

	public int mapWidth = 11;
	public int mapHeight = 600;

	public float weaponCooldown = 0.2f;

	public Color[] playerColors = {};
}

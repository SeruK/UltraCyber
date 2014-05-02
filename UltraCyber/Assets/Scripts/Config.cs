using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour
{
	public float playerMovementSpeed;
	public float playerJumpForce;
	public float playerGravityStrength;
	public Vector2 playerGravity = new Vector2(0.0f, -1.0f);
	public float playerJumpDeceleration;
	public int shots = 6;
}

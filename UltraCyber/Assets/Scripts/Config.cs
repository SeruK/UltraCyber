using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour
{
	public float playerMovementForce;
	public float playerJumpForce;

	public Vector2 playerGravity = new Vector2(0.0f, -1.0f);

	public float maxSpeed = 10.0f;

	public int shots = 6;
}

using UnityEngine;
using System.Collections;

public class ParallaxScroller : MonoBehaviour {


	[SerializeField]
	private float camMovementMultiplier;

	public Vector2 constantSpeed = Vector2.zero;

	public Camera cam;

	private Vector2 oldCamPos;
	void Start () {
		oldCamPos = cam.transform.position;
	}
	
	//save up waypoints
	void Update () {
		Vector2 toGo = (Vector2)cam.transform.position - oldCamPos;

		transform.position += (Vector3)(toGo *camMovementMultiplier);
		oldCamPos = cam.transform.position;
	}
}

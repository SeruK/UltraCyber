using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		for (uint i = 0; i < 2; ++i)
		{
			if (Input.GetKeyDown(KeyCode.Space) || GameInput.GetXboxButton(i, GameInput.Xbox360Button.A) || GameInput.GetXboxButtonDown(i, GameInput.Xbox360Button.B)) {
				Application.LoadLevel(1);
			}

		}
	
	}
}

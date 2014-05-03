using UnityEngine;
using System.Collections;

public class CamMong : MonoBehaviour {

	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {


		//if(GameInput.GetXboxButton(0, GameInput.Xbox360Button.A) || Input.GetKey(KeyCode.Space) 
		//player.input.shoot = GameInput.GetXboxButtonDown(0, GameInput.Xbox360Button.B) || Input.GetKeyDown(KeyCode.X);

		Camera.main.backgroundColor = Helper.GetRandomColor();
	
	}
}

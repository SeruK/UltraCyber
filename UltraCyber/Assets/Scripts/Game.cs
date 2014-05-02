using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
	// temp
	public GUISkin debugGUISkin;

	void Update()
	{
		UpdateInput();
		UpdatePlayer();
	}

	void UpdateInput()
	{

	}

	void UpdatePlayer()
	{
		
	}

	void OnGUI()
	{
		GUI.skin = debugGUISkin;
		GUILayout.Label("HERRO WROLD");
		GUI.skin = null;
	}
}

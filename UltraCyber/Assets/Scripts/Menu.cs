using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public AudioClip voice;
	public AudioSource musicSource;
	public float voiceWait;

	void OnEnable()
	{
		StartCoroutine(PlayVoice());
	}

	IEnumerator PlayVoice()
	{
		yield return new WaitForSeconds(voiceWait);
		AudioSource.PlayClipAtPoint(voice, Vector3.zero);
		yield return new WaitForSeconds(voice.length);
		musicSource.Play();
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

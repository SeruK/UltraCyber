using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public AudioClip voice;
	public AudioClip explosionClip;
	public AudioSource musicSource;
	public float voiceWait;

	public Animator explosionAnim;
	public SpriteRenderer explosion;
	public SpriteRenderer logo;

	bool finishedIntro;

	void OnEnable()
	{
		finishedIntro = false;
		logo.enabled = explosion.enabled = false;
		StartCoroutine(PlayVoice());
	}

	bool shake = false;

	IEnumerator PlayVoice()
	{
		yield return new WaitForSeconds(voiceWait);
		AudioSource.PlayClipAtPoint(voice, Vector3.zero);
		logo.enabled = true;
		yield return new WaitForSeconds(voice.length);
		shake = true;
		AudioSource.PlayClipAtPoint(explosionClip, Vector3.zero);
		explosion.enabled = true;
		explosionAnim.Play("TitleAnim");
		musicSource.Play();
		finishedIntro = true;
	}

	// Update is called once per frame
	void Update () {
		if (shake)
			CameraShaker.Instance.Shake();

		if (!finishedIntro)
			return;

		for (uint i = 0; i < 2; ++i)
		{
			if (Input.anyKeyDown || 
			    GameInput.GetXboxButton(i, GameInput.Xbox360Button.A) || 
			    GameInput.GetXboxButtonDown(i, GameInput.Xbox360Button.B) ||
			    GameInput.GetXboxButtonDown(i, GameInput.Xbox360Button.Start)) {
				Application.LoadLevel(1);
			}

		}
	
	}
}

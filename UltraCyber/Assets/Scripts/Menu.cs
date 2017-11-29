using UnityEngine;
using UE = UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {

	public AudioClip voice;
	public AudioClip explosionClip;
	public AudioSource musicSource;
	public float voiceWait;

	public Animator explosionAnim;
	public SpriteRenderer explosion;
	public SpriteRenderer logo;

	public GameObject playButton;

	bool finishedIntro;

	void OnEnable()
	{
		finishedIntro = false;
		logo.enabled = explosion.enabled = false;
		StartCoroutine(PlayVoice());

		SelectPlayButton();
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

#if !USE_REWIRED
		if(GameInput.GetAnyButtonDown())
		{
			Application.LoadLevel(1);
		}
#endif // !USE_REWIRED
	}

	void SelectPlayButton()
	{
		EventSystem.current.SetSelectedGameObject(playButton);
    }

	public void ToggleFullscreen()
	{
		Screen.fullScreen = !Screen.fullScreen;
	}

	public void LoadLevel()
	{
		Application.LoadLevel(1);
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}

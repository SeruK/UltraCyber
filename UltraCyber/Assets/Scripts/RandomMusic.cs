using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class RandomMusic : MonoBehaviour
{
	public AudioClip[] clips = {};

	void Awake()
	{
		if (clips != null && clips.Length > 0)
		{
			audio.clip = clips[Random.Range(0, clips.Length)];
			audio.Play();
	    }
	}
}

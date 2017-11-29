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
			GetComponent<AudioSource>().clip = clips[Random.Range(0, clips.Length)];
			GetComponent<AudioSource>().Play();
	    }
	}
}

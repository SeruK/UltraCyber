using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	[SerializeField]
	private AudioClip impactSound;


	void OnCollisionEnter2D(Collision2D collision) {

		Debug.Log("HEJ!!");
		EffectSpawner.Instance.SpawnBulletImpact(transform.position);
		GameObject.Destroy(gameObject);
	}

}

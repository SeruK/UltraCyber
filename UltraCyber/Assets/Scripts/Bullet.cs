using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	[SerializeField]
	private AudioClip impactSound;


	void OnCollisionEnter2D(Collision2D collision) {

		EffectSpawner.Instance.SpawnBulletImpact(transform.position);
		GameObject.Destroy(gameObject);
	}

}

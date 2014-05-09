using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	public delegate void HitHandler(Bullet Bullet, Collision2D collision);

	public event HitHandler onHit;
	public bool dead = false;

	void OnEnable()
	{
		dead = false;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Die(collision);
	}

	public void Die()
	{
		Die(null);
	}

	private void Die(Collision2D collision)
	{
		if (dead)
			return;
		dead = true;
		if (onHit != null)
			onHit(this, collision);
		EffectSpawner.Instance.SpawnBulletImpact(transform.position);
		GameObject.Destroy(gameObject);
	}
}

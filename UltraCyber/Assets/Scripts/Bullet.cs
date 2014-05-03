using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

	public delegate void HitHandler(Bullet Bullet, Collision2D collision);

	public event HitHandler onHit;

	void OnCollisionEnter2D(Collision2D collision)
	{

		/*BlockTurbo turb = collision.gameObject.GetComponent<BlockTurbo>();

		if (turb)
		{
			turb.KillMe();
		}*/
		if (onHit != null)
			onHit(this, collision);
		EffectSpawner.Instance.SpawnBulletImpact(transform.position);
		GameObject.Destroy(gameObject);
	}

}

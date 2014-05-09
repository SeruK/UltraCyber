using UnityEngine;
using System.Collections;

public class BlockTurbo : MonoBehaviour
{
	public int x;
	public int y;

	public delegate void Derp(BlockTurbo turbo);

	public event Derp DestroyMe;

	private bool dead;

	void OnEnable()
	{
		dead = false;
	}

	public void KillMe()
	{
		if (dead)
			return;

		dead = true;

		if (DestroyMe != null)
			DestroyMe(this);
	}

	public void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == 12 || coll.gameObject.layer == 11)
		{
			var bullet = coll.GetComponent<Bullet>();
			if (bullet)
			{
				bullet.Die();
			}
			KillMe();
			//Invoke("KillMe", 0.001f);
		}
	}

	public void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == 12 || coll.gameObject.layer == 11)
		{
			var bullet = coll.GetComponent<Bullet>();
			if (bullet)
			{
				bullet.Die();
			}
			KillMe();
			//Invoke("KillMe", 0.001f);
		}
	}
}

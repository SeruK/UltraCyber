using UnityEngine;
using System.Collections;

public class BlockTurbo : MonoBehaviour
{
	public int x;
	public int y;

	public delegate void Derp(BlockTurbo turbo);

	public event Derp DestroyMe;

	public void KillMe()
	{
		if (DestroyMe != null)
			DestroyMe(this);
	}

	public void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == 12 || coll.gameObject.layer == 11)
			Invoke("KillMe", 0.001f);
	}
}

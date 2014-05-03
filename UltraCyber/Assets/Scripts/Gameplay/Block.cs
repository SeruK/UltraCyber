using UnityEngine;
using System.Collections;

public enum BlockType {
	SOMETYPE,
	ANOTHERTYPE
}

public class Block : MonoBehaviour {


	public BlockType blockType;


	void OnCollisionEnter2D(Collision2D collider)
	{

		if(transform.position.y > collider.transform.position.y)
		Destroy ();
	}



	public void Destroy() {
		gameObject.SetActive(false);
		EffectSpawner.Instance.SpawnExplosion(transform.position);
	}


}

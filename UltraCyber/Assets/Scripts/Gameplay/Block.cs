using UnityEngine;
using System.Collections;

public enum BlockType {
	SOMETYPE,
	ANOTHERTYPE
}

public class Block : MonoBehaviour {


	public BlockType blockType;

	public int haxIndex = 0;
//	void OnCollisionEnter2D(Collision2D collider)
//	{
//
//		if(transform.position.y > collider.gameObject.transform.position.y)
//			Destroy ();
//	}
//
//	void OnCollisionStay2D(Collision2D collision) {
//			//collision.transform.position = 
//
//	}



	public void Destroy() {
		Debug.Log ("DESTROYING A BLOCK!");
		//gameObject.transform.localScale = Vector3.one * 10f;
		gameObject.SetActive(false);
		EffectSpawner.Instance.SpawnExplosion(transform.position);
	}


}

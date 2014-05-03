﻿using UnityEngine;
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

	void OnCollisionStay2D(Collision2D collision) {


	}



	public void Destroy() {
		gameObject.SetActive(false);
		EffectSpawner.Instance.SpawnExplosion(transform.position);
	}


}

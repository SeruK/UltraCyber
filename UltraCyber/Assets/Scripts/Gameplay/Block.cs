using UnityEngine;
using System.Collections;

public enum BlockType {
	SOMETYPE,
	ANOTHERTYPE
}

public class Block : MonoBehaviour {


	public BlockType blockType;


	void OnCollisionEnter(Collision collision)
	{
		//dostuff
	}


}

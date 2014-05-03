using System.Collections.Generic;
using UnityEngine;


public class BlockRow : MonoBehaviour
{
	public List<Block> blocks = new List<Block>();

	public Block AddBlock(Block block)
	{
		block.transform.parent = transform;
		return block;
	}


		void OnCollisionEnter2D(Collision2D collision)
		{
	
			if(transform.position.y > collision.gameObject.transform.position.y)
			{
				//Debug.Log (collision.transform.localPosition.x);
				int blockToDestroyIndex =  Mathf.RoundToInt((collision.transform.position.x)); //magic?
				Debug.Log (blockToDestroyIndex);
				List<Block> connected = gameObject.GetComponent<BlockCounter>().connectedBlocks;

				foreach(Block bloxor in connected) {
					
					if(bloxor.haxIndex == blockToDestroyIndex)
						bloxor.Destroy();

				}

			}
		}

}

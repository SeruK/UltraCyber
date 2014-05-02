using System.Collections.Generic;
using UnityEngine;


public class BlockRow : MonoBehaviour
{
	//public List<Block> blocks = new List<Block>();

	public void AddBlock(Block block)
	{
		block.transform.parent = transform;
//		blocks.Add(block);
	}

}

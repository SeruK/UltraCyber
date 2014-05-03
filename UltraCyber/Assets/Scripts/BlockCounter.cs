using UnityEngine;	
using System.Collections.Generic;
public class BlockCounter : MonoBehaviour
{
	public List<Block> connectedBlocks = new List<Block>();
	public void AddConnectedBlocks(List<Block> konnectedBlocks) {

		foreach(Block bloxie in konnectedBlocks) {
			connectedBlocks.Add(bloxie);
		}

	}
			
			
				
}


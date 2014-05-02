using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockData {
}

public class ItemData {
}

public class MapData {
	private List <Vector2> spawnpoins = new List<Vector2>();
}

public class MapLoader : MonoBehaviour {

	private List<GameObject> rowBlocks = new List<GameObject>();

	private List<Block> greenBlocks = new List<Block>(); //pooled

	[SerializeField]
	private GameObject GreenBlock;

	[SerializeField]
	private GameObject RedBlock;

	public List<Texture2D> blockImages = new List<Texture2D>();

	void Start () {

		//START BY PPOOLING LIKE 30 of each type!! but..later.

		foreach (Texture2D blockRowie in blockImages) {
			rowBlocks.Add(CreateBlockRow(blockRowie));

			rowBlocks[rowBlocks.Count-1].transform.parent = transform;
			rowBlocks[rowBlocks.Count-1].gameObject.SetActive(false);
		}

		TestSpawn();
	}

	public void TestSpawn() {

		float startY = 0;
		while(startY < 200) {

			GameObject testRow = (GameObject)GameObject.Instantiate(rowBlocks[1]);
			testRow.gameObject.SetActive(true);
			testRow.transform.position = new Vector2(0,startY);
			testRow.transform.parent = transform; //get outta here.
			startY += 2;

		}
	}

	public GameObject CreateBlockRow(Texture2D blockRowImage) {

		BlockRow newRow = new GameObject("ROW").AddComponent<BlockRow>();

		for(int x=0;x<blockRowImage.width;++x)
		{
			Debug.Log ("PIXEL");
			Vector2 bajs = new Vector2(x, 0);
			Color color = blockRowImage.GetPixel(x,0);
			Debug.Log(color);
				
			if(color == Color.green) {

			GameObject greenie = GameObject.Instantiate(GreenBlock) as GameObject;
			greenie.transform.localPosition = bajs;
			newRow.AddBlock(greenie.GetComponentInChildren<Block>());
			}					
//			else if(color == Color.blue) 
//			{
//			
//			}					
			else if(color == Color.red) 
			{
				GameObject reddie = GameObject.Instantiate(RedBlock) as GameObject;
				reddie.transform.localPosition = bajs;
				newRow.AddBlock(reddie.GetComponentInChildren<Block>());
			}
//			else if(color == Color.black) 
//			{
//					
//			}
		}

		return newRow.gameObject;
	}

}
	











﻿using UnityEngine;
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


	private int width = 11; //put in config.. ffs.

	private List<GameObject> rowBlocks = new List<GameObject>();

	private List<Block> greenBlocks = new List<Block>(); //pooled

	[SerializeField]
	private Texture2D StartBlock;

	[SerializeField]
	private GameObject GreenBlock;

	[SerializeField]
	private List<GameObject> BackGroundTiles;


	[SerializeField]
	private GameObject RightWall;

	[SerializeField]
	private GameObject LeftWall;

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
		RightWall.gameObject.SetActive(false);
		LeftWall.gameObject.SetActive(false);
		TestSpawn();
	}

	public void TestSpawn() {

		float startY = 0;
		GameObject startBlock = CreateBlockRow(StartBlock);
		startBlock.transform.position = new Vector2(0,startY);
		startBlock.transform.parent = transform; //get outta here.

		while(startY < 200) {

			for(int bgX=0;bgX<11;++bgX) {

				GameObject bgTile = (GameObject)GameObject.Instantiate(BackGroundTiles[0]);
				bgTile.transform.position = new Vector3(bgX, startY, 1);
				bgTile.transform.parent = transform;
			}

			if(startY%2 == 0) {
				GameObject testRow = (GameObject)GameObject.Instantiate(rowBlocks[UnityEngine.Random.Range(0,rowBlocks.Count)]);
				testRow.gameObject.SetActive(true);
				testRow.transform.position = new Vector2(0,startY);
				testRow.transform.parent = transform; //get outta here.
			}
			startY += 1;
			Vector2 rightWallPos = new Vector2(11, startY);
			
			GameObject rightWall = (GameObject)GameObject.Instantiate(RightWall);
			rightWall.gameObject.SetActive(true);
			rightWall.transform.position = rightWallPos;
			rightWall.transform.parent = transform; //get outta here.


			Vector2 leftWallPos = new Vector2(-1, startY);

			GameObject leftWall = (GameObject)GameObject.Instantiate(LeftWall);
			leftWall.gameObject.SetActive(true);
			leftWall.transform.position = leftWallPos;
			leftWall.transform.parent = transform; //get outta here.

		}
	}

	public GameObject CreateBlockRow(Texture2D blockRowImage) {

		BlockRow newRow = new GameObject("ROW").AddComponent<BlockRow>();


		for(int x=0;x<blockRowImage.width;++x)
		{
			Vector2 bajs = new Vector2(x, 0);
			Color color = blockRowImage.GetPixel(x,0);

			int inRow = 0;


			if(color == Color.green) {
				++inRow;
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
				++inRow;
				GameObject reddie = GameObject.Instantiate(RedBlock) as GameObject;
				reddie.transform.localPosition = bajs;
				newRow.AddBlock(reddie.GetComponentInChildren<Block>());
			}
			else {

				//build collider.
			}
//			else if(color == Color.black) 
//			{
//					
//			}
		}

		return newRow.gameObject;
	}

}
	











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


	//MAKE SINGLETON AND ABLE TO MAKE AAAALLL BE NONKINEMATIC; VERRY CFUNNNSY!

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
		GameObject startBlock = CreateBlockRow(StartBlock, "ronka");
		startBlock.transform.position = new Vector2(0,startY);
		startBlock.transform.parent = transform; //get outta here.

		while(startY < 400) {

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

	public GameObject CreateBlockRow(Texture2D blockRowImage, string name="ROW") {

		BlockRow newRow = new GameObject(name).AddComponent<BlockRow>();

		int inRow = 0;
		List<Block> connectedBlocks = new List<Block>();
		Vector2 startPos = Vector2.zero;

		for(int x=0;x<blockRowImage.width;++x)
		{
			Vector2 bajs = new Vector2(x, 0);
			Color color = blockRowImage.GetPixel(x,0);

			if(color == Color.green) {

				if(inRow == 0)
					startPos = bajs;

				++inRow;

			GameObject greenie = GameObject.Instantiate(GreenBlock) as GameObject;
			greenie.transform.localPosition = bajs;
			connectedBlocks.Add(newRow.AddBlock(greenie.GetComponentInChildren<Block>()));
			connectedBlocks[connectedBlocks.Count-1].haxIndex= x;
			
			}					
			else if(color == Color.red) 
			{
				if(inRow == 0)
					startPos = bajs;

				++inRow;

				GameObject reddie = GameObject.Instantiate(RedBlock) as GameObject;
				reddie.transform.localPosition = bajs;
				connectedBlocks.Add(newRow.AddBlock(reddie.GetComponentInChildren<Block>()));
				connectedBlocks[connectedBlocks.Count-1].haxIndex= x;
			}
			else {

				if(inRow > 0) {
					BlockRow subRow = new GameObject("SUBROW").AddComponent<BlockRow>();
					subRow.transform.parent = newRow.transform;
					BoxCollider2D boxCollider = subRow.gameObject.AddComponent<BoxCollider2D>();
					subRow.gameObject.AddComponent<Rigidbody2D>();
					boxCollider.center = startPos + new Vector2((inRow * 0.5f) - 0.5f,0.35f);
					boxCollider.size = new Vector2(inRow,0.3f);
					boxCollider.attachedRigidbody.isKinematic = true;

					BlockCounter counter = subRow.gameObject.AddComponent<BlockCounter>();
					counter.AddConnectedBlocks(connectedBlocks);
					counter.connectedBlocks[connectedBlocks.Count-1].haxIndex= x;
					//Helper.CreateDebugSphere(startPos + new Vector2(inRow * 0.5f,0), Color.blue);
				}
			
				inRow = 0;
				connectedBlocks.Clear ();
				//build collider.
			}
//			else if(color == Color.black) 
//			{

			//H IH IHIHIHIHAHHAAXXX
			if(x == blockRowImage.width-1 && inRow > 0) {
				BlockRow subRow = new GameObject("SUBROW").AddComponent<BlockRow>();
				subRow.transform.parent = newRow.transform;
				BoxCollider2D boxCollider = subRow.gameObject.AddComponent<BoxCollider2D>();
				subRow.gameObject.AddComponent<Rigidbody2D>();
				boxCollider.center = startPos + new Vector2((inRow * 0.5f) - 0.5f,0.35f);
				boxCollider.size = new Vector2(inRow,0.3f);
				boxCollider.attachedRigidbody.isKinematic = true;

				BlockCounter counter = subRow.gameObject.AddComponent<BlockCounter>();
				counter.AddConnectedBlocks(connectedBlocks);
				counter.connectedBlocks[connectedBlocks.Count-1].haxIndex= x;
				//Helper.CreateDebugSphere(startPos + new Vector2(inRow * 0.5f,0), Color.blue);
			} //bajs orkar klockan e maassa 
//					
//			}
		}

		return newRow.gameObject;
	}

}
	











using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapLoaderTurbo : MonoBehaviour
{
	public enum BlockType
	{
		Empty,
		Filled
	}

	public Config config;
	
	private List<List<BlockTurbo>> rowBlocks = new List<List<BlockTurbo>>();
	private List<List<BlockType>> blockTypes = new List<List<BlockType>>();

	[SerializeField]
	private Texture2D StartBlock;
	
	[SerializeField]
	private GameObject GreenBlock;

	public GameObject LeftWall;
	public GameObject RightWall;
	public GameObject BackdropPrefab;

	[SerializeField]
	private List<GameObject> BackGroundTiles;

	public List<GameObject> SideTiles;

	public Texture2D[] RowTextures = {};

//	void OnEnable()
//	{
//		Recreate();
//	}
//	
//	void OnDisable()
//	{
//		Clear();
//	}

	public void DestroyBlock(BlockTurbo block)
	{
		if (block == null)
			return;

		int y = block.y;
		blockTypes[y][block.x] = BlockType.Empty;
		DestroyBlockRow(y);
		CreateBlockRow(y, blockTypes[y]);
	}

	public void Clear()
	{
		if (rowBlocks != null)
		{
			for (int y = 0; y < rowBlocks.Count; ++y)
			{
				DestroyBlockRow(y);
			}

			rowBlocks.Clear();
		}

		if (blockTypes != null)
			blockTypes.Clear();

//		if (SideTiles != null)
//		{
//			for (int i = 0; i < SideTiles.Count; ++i)
//			{
//				Destroy(SideTiles[i])
//			}
//			SideTiles.Clear();
//		}
//
//		if (BackGroundTiles != null)
//		{
//			for (int i = 0; i < BackGroundTiles.Count; ++i)
//			{
//				Destroy(BackGroundTiles[i]);
//			}
//			BackGroundTiles.Clear();
//		}
	}

	public void Recreate()
	{
		Clear();

		if (SideTiles != null && SideTiles.Count <= 0)
		{
			SideTiles = new List<GameObject>();

			for (int y = 1; y < config.mapHeight; ++y)
			{
				var bl = Instantiate(LeftWall) as GameObject;
				bl.transform.position = new Vector2(-2.0f, y);
				bl = Instantiate(RightWall) as GameObject;
				bl.transform.position = new Vector2(config.mapWidth - 1, y);
			}
		}

		if (BackGroundTiles != null && BackGroundTiles.Count <= 0)
		{
			BackGroundTiles = new List<GameObject>();

			for (int y = 0; y < config.mapHeight; ++y)
			{
				for (int x = 0; x < config.mapWidth; ++x)
				{
					var bl = Instantiate(BackdropPrefab) as GameObject;
					bl.transform.position = new Vector2((float)x - 1.0f, y);
					BackGroundTiles.Add(bl);
				}
			}
		}

		rowBlocks = new List<List<BlockTurbo>>();

		for (int y = 0; y < config.mapHeight; ++y)
		{
			var bloList = new List<BlockTurbo>();
			rowBlocks.Add(bloList);

			var typesList = new List<BlockType>();

			if (y == 0)
			{
				for (int x = 0; x < config.mapWidth; ++x)
				{
					typesList.Add(BlockType.Filled);
				}
			}
			else if (y % 2 == 1)
			{
				for (int x = 0; x < config.mapWidth; ++x)
				{
					typesList.Add(BlockType.Empty);
				}
			}
			else
			{
				var tex = RowTextures[Random.Range(0, RowTextures.Length)];
				typesList = ReadBlockRow(tex);
			}

			CreateBlockRow(y, typesList);
			blockTypes.Add(typesList);
		}
	}

	List<BlockType> ReadBlockRow(Texture2D tex)
	{
		List<BlockType> blos = new List<BlockType>();

		for (int x = 0; x < config.mapWidth; ++x)
		{
			Color color = tex.GetPixel(x % config.mapWidth, 0);
			if (color == Color.green)
			{
				blos.Add(BlockType.Filled);
			}
			else
			{
				blos.Add(BlockType.Empty);
			}
		}

		return blos;
	}

	void DestroyBlockRow(int y)
	{
		List<BlockTurbo> row = rowBlocks[y];

		for (int x = 0; x < row.Count; ++x)
		{
			var blo = row[x];

			if (blo && blo.gameObject)
				Destroy(blo.gameObject);
		}

		row.Clear();
	}

	void CreateBlockRow(int y, List<BlockType> blockTypes)
	{
		List<BlockTurbo> row = rowBlocks[y];

		row.Clear();

		BoxCollider2D collider = null;
		int w = 0;
		for (int x = 0; x < config.mapWidth; ++x)
		{
			BlockType bt = blockTypes[x];
			BlockTurbo block = CreateBlock(bt, x, y);

			if (!collider)
			{
				w = 0;
				if (block)
				{
					collider = block.gameObject.AddComponent<BoxCollider2D>();
					collider.center = new Vector2(0.0f, 0.25f);
					collider.size = new Vector2(1.0f, 0.5f);
				}
			}
			else
			{
				if (bt == BlockType.Empty)
				{
					collider = null;
					w = 0;
				}
				else
				{
					++w;
					
					collider.center = new Vector2(((float)w) * 0.5f, 0.25f);
					collider.size = new Vector2((float)(w + 1), 0.5f);
				}
			}

			if (block)
			{
				row.Add(block);
			}
		}
	}

	BlockTurbo CreateBlock(BlockType blockType, int x, int y)
	{
		BlockTurbo turb = null;

		if (blockType == BlockType.Filled)
		{
			var go = Instantiate(GreenBlock) as GameObject;
			go.transform.position = new Vector2((float)x - 1.0f, (float)y);
			var blo = go.GetComponent<BlockTurbo>();
			blo.x = x;
			blo.y = y;
			turb = blo;

			turb.DestroyMe += (BlockTurbo turbBlock) => {
				DestroyBlock(turbBlock);
			};
		}

		return turb;
	}
}

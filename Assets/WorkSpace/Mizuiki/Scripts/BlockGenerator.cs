using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [Header("ブロックのデータベース")]
    [SerializeField] private BlockDataBase m_blockDataBase = null;

    [Header("マップオブジェクト")]
    [SerializeField] private GameObject m_mapObject = null;
    [Header("マップの目隠し")]
    [SerializeField] private GameObject m_mapBlind = null;


	private void Awake()
	{
		// ブロックデータベースがない
		if (m_blockDataBase == null)
		{
            Debug.Log(gameObject.name + "にブロックデータベースを設定してね");

			//m_blockDataBase = AssetDatabase.LoadAssetAtPath<BlockDataBase>("Assets/DataBase/Block/BlockDataBase.asset");
		}
	}

	// Start is called before the first frame update
	void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// ブロックを生成
    /// </summary>
    /// <param name="type">生成するブロックの種類</param>
    /// <param name="position">生成する座標</param>
    /// <param name="parent">親</param>
    /// <param name="isBrightness">明るさをつけるかどうか</param>
    public GameObject GenerateBlock(BlockData.BlockType type, Vector2 position, Transform parent = null, bool isBrightness = false)
    {
		// ブロックのデータ取得
		BlockData data = MyFunction.GetBlockData(m_blockDataBase, type);

        // データがない
        if (data == null)
            return null;

        // プレハブが設定されていない
        if (!data.Prefab)
        {
            Debug.Log(data.name + "にブロックのプレハブを設定してね");

            return null;
            //data.prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Block.prefab");
        }

		// 生成したブロックを設定する用
		GameObject obj;

        // 親が指定されている
        if (parent)
        {
            // 親を設定して生成
            obj = Instantiate(data.Prefab, position, Quaternion.identity, parent);
        }
        // 親の指定はない
        else
        {
            // 指定座標に生成
            obj = Instantiate(data.Prefab, position, Quaternion.identity);
        }

        // データの設定
        if (obj.TryGetComponent(out Block block))
        {
            // データ
            block.BlockData = data;
            // 耐久
            block.Endurance = data.Endurance;
            // 破壊不可
            block.DontBroken = data.DontBroken;
            // 光源レベル
            block.LightLevel = data.LightLevel;
        }

		// 画像の設定
		if (data.Sprite)
		{
			if (obj.TryGetComponent(out SpriteRenderer sprite))
			{
				sprite.sprite = data.Sprite;
			}
		}

		//明るさの追加
		if (isBrightness)
        {
            obj.AddComponent<ChangeBrightness>();
        }


        if (m_mapObject)
        {
            // マップオブジェクトの生成
            GameObject mapObj = Instantiate(m_mapObject, obj.transform);
            // 色の設定
            mapObj.GetComponent<SpriteRenderer>().color = data.Color;
            mapObj.GetComponent<MapObject>().BlockColor = data.Color;
            // 表示順の設定
            mapObj.GetComponent<SpriteRenderer>().sortingOrder = data.Order;
            // スプライトの設定
            mapObj.GetComponent<MapObject>().ParentSprite = obj.gameObject.GetComponent<SpriteRenderer>();

        }
		if (m_mapBlind)
		{
			// マップの目隠し生成
			//Instantiate(m_mapBlind, block.transform);
		}


        return obj;
	}
}
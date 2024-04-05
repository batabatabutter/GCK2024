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
		// ブロックデータベースがなければ取得する
		if (m_blockDataBase == null)
		{
			m_blockDataBase = AssetDatabase.LoadAssetAtPath<BlockDataBase>("Assets/DataBase/Block/BlockDataBase.asset");
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

		// 生成したブロックを設定する用
		GameObject obj;

        // プレハブが設定されていない場合はアセット参照でブロックを持ってくる
        if (!data.prefab)
        {
            data.prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Block.prefab");
        }

        if (parent)
        {
            // 親を設定して生成
            obj = Instantiate(data.prefab, position, Quaternion.identity, parent);
        }
        else
        {
            // 指定座標に生成
            obj = Instantiate(data.prefab, position, Quaternion.identity);
        }

        // データの設定
        if (obj.TryGetComponent(out Block block))
        {
            // データ
            block.BlockData = data;
            // 耐久
            block.Endurance = data.endurance;
            // 破壊不可
            block.DontBroken = data.dontBroken;
            // 光源レベル
            block.LightLevel = data.lightLevel;
        }

		// 画像の設定
		if (data.sprite)
		{
			if (obj.TryGetComponent(out SpriteRenderer sprite))
			{
				sprite.sprite = data.sprite;
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
            mapObj.GetComponent<SpriteRenderer>().color = data.color;
            mapObj.GetComponent<MapObject>().BlockColor = data.color;
            // 表示順の設定
            mapObj.GetComponent<SpriteRenderer>().sortingOrder = data.order;
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
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


    // Start is called before the first frame update
    void Start()
    {
        // ブロックのデータベースがなければアセット参照
        if (m_blockDataBase == null)
        {
            m_blockDataBase = AssetDatabase.LoadAssetAtPath<BlockDataBase>("Assets/DataBase/Block/BlockDataBase.asset");
        }
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

        // 生成したブロックを設定する用
        GameObject block = null;

        if (parent)
        {
            // 親を設定して生成
            block = Instantiate(data.prefab, position, Quaternion.identity, parent);
        }
        else
        {
            // 指定座標に生成
            block = Instantiate(data.prefab, position, Quaternion.identity);
        }

        // ブロックの画像設定
        if (block.TryGetComponent(out SpriteRenderer sprite))
        {
            if (data.sprite)
            {
				sprite.sprite = data.sprite;
			}
		}

        //明るさの追加
        if(isBrightness)
        {
            block.AddComponent<ChangeBrightness>();
        }


        if (m_mapObject)
        {
            // マップオブジェクトの生成
            GameObject mapObj = Instantiate(m_mapObject, block.transform);
            // 色の設定
            mapObj.GetComponent<SpriteRenderer>().color = data.color;
            mapObj.GetComponent<MapObject>().BlockColor = data.color;
            // 表示順の設定
            mapObj.GetComponent<SpriteRenderer>().sortingOrder = data.order;
            // スプライトの設定
            mapObj.GetComponent<MapObject>().ParentSprite = block.gameObject.GetComponent<SpriteRenderer>();

        }
		if (m_mapBlind)
		{
			// マップの目隠し生成
			//Instantiate(m_mapBlind, block.transform);
		}


        return block;
	}
}
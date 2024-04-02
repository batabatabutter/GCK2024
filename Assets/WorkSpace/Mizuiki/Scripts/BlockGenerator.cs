using System.Collections;
using System.Collections.Generic;
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
    public void GenerateBlock(BlockData.ToolType type, Vector2 position, Transform parent = null)
    {
        // ブロックのデータ取得
        BlockData data = m_blockDataBase.block[(int)type];

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
            mapObj.GetComponent<MapObject>().ParentSprite = gameObject.GetComponent<SpriteRenderer>();

        }
		if (m_mapBlind)
		{
			// マップの目隠し生成
			//Instantiate(m_mapBlind, block.transform);
		}

	}
}
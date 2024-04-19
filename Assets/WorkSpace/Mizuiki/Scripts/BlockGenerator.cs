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

    [Header("地面")]
    [SerializeField] private GameObject m_ground = null;


	private void Awake()
	{
		// ブロックデータベースがない
		if (m_blockDataBase == null)
		{
            Debug.Log(gameObject.name + "にブロックデータベースを設定してね");
		}
	}

    /// <summary>
    /// ブロックを生成(typeにOVERを設定すると地面のみが生成される)
    /// </summary>
    /// <param name="type">生成するブロックの種類</param>
    /// <param name="position">生成する座標</param>
    /// <param name="parent">親</param>
    /// <param name="isBrightness">明るさをつけるかどうか</param>
    public GameObject GenerateBlock(BlockData.BlockType type, Vector2 position, Transform parent = null, bool isBrightness = false)
    {
		// 地面を生成
		GameObject ground = CreateObject(parent, m_ground, position);
 
		// ブロックのデータ取得
		BlockData data = MyFunction.GetBlockData(m_blockDataBase, type);

        // データがない
        if (data == null)
            return ground;

        // プレハブが設定されていない
        if (!data.Prefab)
        {
            Debug.Log(data.name + "にブロックのプレハブを設定してね");

            return null;
        }

		// 生成したブロックを設定する用
		GameObject obj;
        // 地面が生成されている
        if (ground)
        {
            // 地面を親に設定して生成
            obj = CreateObject(ground.transform, data.Prefab, position);
        }
        // 地面はない
        else
        {
            // 親を設定して生成
            obj = CreateObject(parent, data.Prefab, position);
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

        // データの設定
        if (!obj.TryGetComponent(out Block block))
            return obj;

        // データ
        block.BlockData = data;
        // 耐久
        block.Endurance = data.Endurance;
        // 破壊不可
        block.DontBroken = data.DontBroken;
        // 光源レベル
        block.LightLevel = data.LightLevel;

        if (m_mapObject)
        {
            // マップオブジェクトの生成
            GameObject mapObj = Instantiate(m_mapObject, obj.transform);
            MapObject map = mapObj.GetComponent<MapObject>();
            // 色の設定
            mapObj.GetComponent<SpriteRenderer>().color = data.Color;
            map.BlockColor = data.Color;
            // 表示順の設定
            mapObj.GetComponent<SpriteRenderer>().sortingOrder = data.Order;
            // スプライトの設定
            map.ParentSprite = obj.gameObject.GetComponent<SpriteRenderer>();
            // 親の設定
            map.Parent = block;
        }

        return obj;
	}

    // オブジェクトを生成する
    private GameObject CreateObject(Transform parent, GameObject gameObject, Vector2 position)
    {
        // 生成するオブジェクト
        GameObject obj;
        // 親が指定されている
        if (parent)
        {
            obj = Instantiate(gameObject, position, Quaternion.identity, parent.transform);
        }
        // 親が指定されていない
        else
        {
            obj = Instantiate(gameObject, position, Quaternion.identity);
        }
        // 生成したオブジェクトを返す
        return obj;
    }
}
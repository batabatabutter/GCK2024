using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [Header("チャンク管理クラス")]
    [SerializeField] private ChunkManager m_chunkManager = null;

    [Header("ブロックのデータベース")]
    [SerializeField] private BlockDataBase m_blockDataBase = null;

    [Header("マップオブジェクト")]
    [SerializeField] private GameObject m_mapObject = null;

    [Header("地面")]
    [SerializeField] private GameObject m_ground = null;

    //[Header("処理不可軽減用プレハブ")]
    //[SerializeField] private GameObject m_pross = null;
    //[SerializeField] private bool m_prossFlag = false;

    [Header("影(ブロックの目隠し)")]
    [SerializeField] private GameObject m_shadow = null;
    // 影の親
    private GameObject m_shadowParent = null;

	//ブロックの親
	private GameObject m_blockParent = null;

	//[Header("光源処理をするか否か")]
	//[SerializeField] private bool m_isBrightness = false;
	//[Header("光源処理をするオブジェクト")]
	//[SerializeField] private GameObject m_lightObject = null;

	//  プレイヤーの座標
	private Transform m_playerTr;



    private void Awake()
	{
		// ブロックデータベースがない
		if (m_blockDataBase == null)
		{
            Debug.Log(gameObject.name + "にブロックデータベースを設定してね");
		}

        // ブロックの親生成
        m_blockParent = new GameObject("Block");
        // 影の親生成
        m_shadowParent = new GameObject("Shadow");
        m_shadowParent.SetActive(false);
	}

    /// <summary>
    /// ブロックを生成(typeにOVERを設定すると地面のみが生成される)
    /// </summary>
    /// <param name="type">生成するブロックの種類</param>
    /// <param name="position">生成する座標</param>
    /// <param name="parent">親</param>
    /// <param name="isBlockBrightness">ブロック明るさをつけるかどうか</param>
    /// <param name="isGroundBrightness">地面明るさをつけるかどうか</param>
    public GameObject GenerateBlock(BlockData.BlockType type, Vector2 position/*, Transform parent = null*/)
    {
        ////  処理軽減
        //Transform pross = null;
        //if (m_prossFlag)
        //{
        //    pross = Instantiate(m_pross, position, Quaternion.identity).transform;
        //    pross.parent = parent;
        //    parent = pross.transform;
        //}

        // 親を取得
        Transform blockParent = m_blockParent.transform;
        Transform shadowParent = m_shadowParent.transform;

        // チャンクマネージャーがある
        if (m_chunkManager)
        {
            // 現在チャンクを親にする
            ChunkManager.Chunk chunk = m_chunkManager.GetChunk(position);
            // ブロックチャンクの設定
            blockParent = chunk.blockChunk.transform;
            // ブロックチャンクの親設定
            blockParent.parent = m_blockParent.transform;
            // 影チャンクの設定
            shadowParent= chunk.shadowChunk.transform;
            // 影チャンクの親設定
            shadowParent.parent = m_shadowParent.transform;
		}

		// 地面を生成
		GameObject ground = Instantiate(m_ground, position, Quaternion.identity, blockParent.transform);
        //ground.GetComponent<ObjectAffectLight>().BrightnessFlag = m_isBrightness;

        // 影(ブロックの目隠し)を生成
        Instantiate(m_shadow, position, Quaternion.identity, shadowParent.transform);

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
            //obj.GetComponent<Block>().Ground = ground.GetComponent<Ground>();
        }
        // 地面はない
        else
        {
            // 親を設定して生成
            obj = CreateObject(blockParent.transform, data.Prefab, position);
        }

        // データの設定
        if (!obj.TryGetComponent(out Block block))
            return obj;

		// 画像の設定
		if (data.Sprite)
		{
            block.SetSprite(data.Sprite);
		}

        // 名前の設定
        block.name = data.Type.ToString() + "_BLOCK";

        // データ
        block.BlockData = data;
        // 耐久
        block.Endurance = data.Endurance;
        // 破壊不可
        block.DontBroken = data.DontBroken;
        // 憑依可能
        block.CanPossess = data.CanPossess;
        //// 光源レベル
        //block.LightLevel = data.LightLevel;

        // 色の設定
        if (obj.TryGetComponent(out SpriteRenderer blockSprite))
        {
            blockSprite.color = data.Color;
        }

        ////  光源処理
        //// 光源の設定
        //if (m_isBrightness)
        //{
        //    // 光源処理用のオブジェクト生成
        //    GameObject light = Instantiate(m_lightObject, block.transform);

        //    // 光源コライダー生成
        //    light.GetComponent<ObjectLight>().FlashLight(block.LightLevel);
        //}

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
            //// マップオブジェクトを設定
            //block.MapObject = map;
        }

        //if (m_prossFlag && pross)
        //{
        //    var p = pross.GetComponent<ProcessChild>();
        //    p.Scripts = new List<MonoBehaviour>(pross.GetComponentsInChildren<MonoBehaviour>().Skip(1));
        //    p.Collider2Ds = new List<Collider2D>(pross.GetComponentsInChildren<Collider2D>().Skip(1));
        //    p.Change(false);
        //}

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

    //  プレイヤー座標系設定
    public void SetPlayerTransform(Transform player)
    {
        m_chunkManager.Player = player;
    }
}
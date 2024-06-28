using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSaw : MonoBehaviour
{
	[System.Serializable]
	public struct ToolLevel
	{
		public MiningData.MiningType type;
		public int level;
	}
	// 採掘方向のRay
	public struct MiningRay
	{
		public Vector2 direction;
		public Vector2 origin;
		public float length;

		public readonly Vector2 MiningPos()
		{
			return origin + (direction * length);
		}
	}

	[Header("丸のこの種類")]
	[SerializeField] private MiningData.MiningType m_type;
	[Header("丸のこの移動速度")]
	[SerializeField] private float m_circularSawSpeed = 1.0f;
	[Header("丸のこの回転速度")]
	[SerializeField] private float m_circularSawRotate = 100.0f;

	[Header("強化段階の区切り")]
	[SerializeField] private int m_stageDelimiter = 10;

	[Header("スプライトの設定先")]
	[SerializeField] private SpriteRenderer m_spriteRenderer = null;

	[Header("採掘データベース")]
	[SerializeField] private MiningDataBase m_dataBase = null;
	private readonly Dictionary<MiningData.MiningType, MiningData> m_miningDatas = new();

	[Header("ツールのレベル")]
	[SerializeField] private ToolLevel[] m_toolLevel = null;
	[Tooltip("インスペクターの値を反映させる")]
	[SerializeField] private bool m_enabled = false;
	[Header("アイテム")]
	[SerializeField] private PlayerItem m_playerItem = null;

	[Header("アニメーター")]
	[SerializeField] private Animator m_animator = null;

	// レベルの辞書
	private Dictionary<MiningData.MiningType, int> m_toolLevels = new();

	// プレイヤー
	private Transform m_player = null;
	// 採掘範囲
	private float m_miningRange = 2.0f;


	private void Awake()
	{
		// スプライトレンダーがなければ設定
		if (m_spriteRenderer == null)
		{
			m_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		// ツールの基本情報
		foreach (MiningData data in m_dataBase.MiningData)
		{
			// 上書き防止
			if (m_miningDatas.ContainsKey(data.Type))
				continue;

			m_miningDatas[data.Type] = data;
		}

		// インスペクター設定有効
		if (m_enabled)
		{
			// ツールのレベル
			foreach (ToolLevel toolLevel in m_toolLevel)
			{
				// 上書き防止
				if (m_toolLevels.ContainsKey(toolLevel.type))
					continue;

				m_toolLevels[toolLevel.type] = toolLevel.level;
			}
		}

		// 親の設定
		m_player = transform.parent;

	}

	private void Start()
	{
		// セーブデータ取得
		SaveDataReadWrite saveData = SaveDataReadWrite.m_instance;

		if (saveData)
		{
			// 装備設定
			SetType(saveData.MiningType);
			// 採掘レベル
			MiningLevels = saveData.MiningLevel;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.G))
		{
			m_animator.SetTrigger("LevelUp");
		}
	}

	// 動かす
	public void Move(Transform player)
	{
		// マウスの位置を取得
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// 移動後の位置
		Vector3 afterPos = mousePos;

		// プレイヤーからの距離が採掘範囲より大きい
		if (Vector3.Distance(player.position, afterPos) > m_miningRange)
		{
			// 採掘範囲に収めた位置を設定
			afterPos = player.position + (afterPos - player.position).normalized * m_miningRange;
		}

		// 丸のこからマウスへのベクトル
		Vector3 circularSawToMining = afterPos - transform.position;
		// 丸のことマウスの距離
		float distance = circularSawToMining.magnitude;

		// 採掘位置へのベクトル正規化
		circularSawToMining.Normalize();

		// 距離が 1f の移動量以内ならそのまま
		if (distance <= m_circularSawSpeed * Time.deltaTime)
		{
			//afterPos = mousePos;
		}
		else
		{
			afterPos = transform.position + (m_circularSawSpeed * Time.deltaTime * circularSawToMining);
		}

		// 座標を設定
		transform.position = afterPos;
	}

	// 回す
	public void Rotate(float speed)
	{
		transform.localEulerAngles += m_circularSawRotate * speed * Time.deltaTime * Vector3.back;
	}

	// 丸のこの位置設定
	public void SetPosition(Vector3 miningPoint)
	{
		transform.position = miningPoint;
	}

	// 採掘範囲設定
	public void SetRange(float range, float size)
	{
		// 範囲の設定
		m_miningRange = range;

		// スケール
		float scale = Mathf.Max(size, 1.0f);

		// 丸のこのサイズ設定
		transform.localScale = Vector3.one * scale;


	}

	// 採掘値取得
	public MiningData.MiningValue GetMiningValue()
	{
		// 基本データ取得
		MiningData miningData = m_miningDatas[m_type];
		// 基礎値取得
		MiningData.MiningValue value = miningData.Value;
		// 強化レベル取得
		int level = m_toolLevels[m_type];

		// 強化ランク
		int rank = 0;

		// 強化ランクが 0 より大きい間ループ
		while (m_stageDelimiter <= 0)
		{
			// 加算量
			int lv = level % m_stageDelimiter;
			level -= m_stageDelimiter;

			// 採掘値加算
			value += miningData.Upgrades[rank].Value * lv;

			// ランクが強化段階以上
			if (rank >= miningData.Upgrades.Length - 1)
				continue;

			// ランクアップ
			rank++;
		}

		return value;
	}
	// 採掘用のRay取得
	public MiningRay GetMiningRay(Transform player)
	{
		// プレイヤーの位置から丸のこの位置へのベクトル
		Vector2 playerToCircular = transform.position - player.position;
		// プレイヤーから丸のこまでの距離
		float length = playerToCircular.magnitude;
		// ベクトル正規化
		playerToCircular.Normalize();

		MiningRay miningRay = new()
		{
			direction = playerToCircular,
			origin = player.position,
			length = length,
		};

		return miningRay;
	}






	// 必要素材の取得
	private Items[] GetNeedMaterials(int addLevel)
	{
		// 必要素材
		Items[] items = new Items[0];

		for (int i = 0; i < addLevel; i++)
		{
			// 強化ランクの取得
			int r = GetRank(m_toolLevels[m_type] + i);

			// 必要素材取得
			Items[] cost = m_miningDatas[m_type].Upgrades[r].Cost;
			// [items] と [cost] を結合するための入れ物
			Items[] dst = new Items[items.Length + cost.Length];

			// 必要素材の追加
			Array.Copy(items, dst, items.Length);
			Array.Copy(cost, 0, dst, items.Length, cost.Length);
			items = dst;
		}

		return items;
	}

	// 強化ランクの取得
	private int GetRank(int add)
	{
		// 区切りごとのランクにする
		int rank = add / m_stageDelimiter;

		// 上限を超えないようにクランプ
		rank = Mathf.Clamp(rank, 0, m_miningDatas[m_type].Upgrades.Length - 1);

		return rank;
	}

	// 強化レベルアップ
	public void Upgrade(int level = 1)
	{
		// 必要素材の取得
		Items[] items = GetNeedMaterials(level);

		// ツールのアップグレードができない
		if (!m_playerItem.CheckCreate(items))
		{
			Debug.Log("アップグレードできないよ");
			return;
		}

		Debug.Log("アップグレード : " + m_type + " : " + m_toolLevels[m_type] + "->" + (m_toolLevels[m_type] + level));

		// エフェクト出す
		m_animator.transform.position = new Vector3(0.0f, transform.localScale.y / 2.0f, 0.0f);
		m_animator.SetTrigger("LevelUp");

		// ツールのレベル加算
		m_toolLevels[m_type] += level;

		// 素材の消費
		m_playerItem.ConsumeMaterials(items);

		// データ保存
		SaveDataReadWrite.m_instance.MiningLevel = m_toolLevels;
		SaveDataReadWrite.m_instance.Items = m_playerItem.Items;
		//SaveDataReadWrite.m_instance.Save();
	}

	// のこの種類設定
	public void SetType(MiningData.MiningType type)
	{
		// 種類の設定
		m_type = type;
		// スプライトの設定
		m_spriteRenderer.sprite = m_miningDatas[type].Sprite;

		//Debug.Log(type + "に変更");
	}
	public void SetType(string typeStr)
	{
		// 大文字に変換
		typeStr = typeStr.ToUpper();
		// 種類の設定
		if (Enum.IsDefined(typeof(MiningData.MiningType), typeStr))
		{
			SetType(Enum.Parse<MiningData.MiningType>(typeStr));
		}
		else
		{
			Debug.Log(typeStr + "は[" + typeof(MiningData.MiningType) + "]に存在しません");
		}
	}


	// 種類
	public MiningData.MiningType MiningType
	{
		get { return m_type; }
	}

	// レベル設定
	public Dictionary<MiningData.MiningType, int> MiningLevels
	{
		set { m_toolLevels = value; }
	}

}

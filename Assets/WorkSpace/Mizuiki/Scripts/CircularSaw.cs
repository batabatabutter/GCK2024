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
	// レベルの辞書
	private Dictionary<MiningData.MiningType, int> m_toolLevels = new();

	[Header("アイテム")]
	[SerializeField] private PlayerItem m_playerItem = null;



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
	}

	// 回す
	public void Rotate(float speed)
	{
		transform.localEulerAngles += m_circularSawRotate * speed * Time.deltaTime * Vector3.back;
	}

	// 丸のこの位置取得
	public Vector3 SetPosition(Vector3 miningPoint)
	{
		// 丸のこから採掘位置へのベクトル
		Vector3 circularSawToMining = miningPoint - transform.position;
		// 丸のこと採掘位置の距離
		float distance = circularSawToMining.magnitude;

		// 距離が 1f の移動量以内ならそのまま採掘地点を返す
		if (distance <= m_circularSawSpeed * Time.deltaTime)
			return miningPoint;

		// 採掘位置へのベクトル正規化
		circularSawToMining.Normalize();

		transform.position += m_circularSawSpeed * Time.deltaTime * circularSawToMining;

		// 丸のこの位置を返す
		return transform.position + (m_circularSawSpeed * Time.deltaTime * circularSawToMining);
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
		rank = Mathf.Clamp(rank, 0, m_miningDatas[m_type].Upgrades.Length);

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

		// ツールのレベル加算
		m_toolLevels[m_type] += level;

		// 素材の消費
		m_playerItem.ConsumeMaterials(items);

		// データ保存
		SaveDataReadWrite.m_instance.MiningLevel = m_toolLevels;
		SaveDataReadWrite.m_instance.Items = m_playerItem.Items;
		SaveDataReadWrite.m_instance.Write();
	}

	// のこの種類設定
	public void SetType(MiningData.MiningType type)
	{
		// 種類の設定
		m_type = type;
		// スプライトの設定
		m_spriteRenderer.sprite = m_miningDatas[type].Sprite;

		Debug.Log(type + "に変更");
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

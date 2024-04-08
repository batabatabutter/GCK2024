using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
	[Header("所持品")]
	[SerializeField] private Dictionary<ItemData.Type, int> m_items = new();
	[Header("最大数")]
	[SerializeField] private int m_maxCount = 99;

	[Header("アイテムの検知範囲(半径)")]
	[SerializeField] private float m_detectionRange = 3.0f;

	[Header("アイテムのデータベース")]
	[SerializeField] private ItemDataBase m_itemDataBase = null;

	[Header("デバッグ----------")]
	[SerializeField] private bool m_debug = false;
	[SerializeField] private Text m_text = null;

	// Start is called before the first frame update
	void Start()
    {
		// コライダーの追加
		CircleCollider2D col = gameObject.AddComponent<CircleCollider2D>();
		// 半径の設定
		col.radius = m_detectionRange;
		// トリガーに設定
		col.isTrigger = true;

		// 所持アイテム数の初期化
		foreach(ItemData itemData in m_itemDataBase.item)
		{
			m_items[itemData.type] = 0;

			// デバッグがオンになっていたら所持数をカンストさせる
			if (m_debug)
			{
				m_items[itemData.type] = m_maxCount;
			}

		}
	}

	// Update is called once per frame
	void Update()
    {
        if (m_debug)
		{
			// テキストがある
			if (m_text != null)
			{
				m_text.text = "";

				foreach(KeyValuePair<ItemData.Type, int> item in m_items)
				{
					m_text.text += item.Key.ToString() + " : " + item.Value.ToString() + "\n";
				}

			}
		}
    }


	// アイテムを見つけた
	private void OnTriggerStay2D(Collider2D collision)
	{
		// タグがアイテム以外
		if (!collision.CompareTag("Item"))
			return;

		// アイテムスクリプトの取得
		if (!collision.TryGetComponent(out Item item))
			return;

		// アイテムの種類
		ItemData.Type itemType = item.ItemType;

		// 拾えない
		if (!CheckAcquirable(itemType))
			return;

		// プレイヤーのトランスフォームを設定する
		item.SetPlayerTransform(transform);

	}


	// 拾えるか確認
	public bool CheckAcquirable(ItemData.Type itemType)
	{
		// 所持数が最大数に達していない
		if (m_items[itemType] < m_maxCount)
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// アイテムを拾う
	/// </summary>
	/// <param name="type">アイテムの種類</param>
	/// <param name="count">アイテムの数</param>
	/// <returns>拾った数</returns>
	public int PicUp(ItemData.Type type, int count)
	{
		// 所持数が最大
		if (m_items[type] >= m_maxCount)
			return 0;

		// 拾う数
		int picCount = m_maxCount - m_items[type];

		// 拾える数がアイテムのスタック数より大きい
		if (picCount > count)
		{
			// アイテムのスタック分拾う
			picCount = count;
		}

		// アイテムを拾う
		m_items[type] += picCount;

		// 拾った数を返す
		return picCount;
	}

	// アイテムを消費する
	public void ConsumeMaterials(ToolData data, int value = 1)
	{
		for (int i = 0; i < data.itemMaterials.Count; i++)
		{
			// アイテムの種類取得
			ItemData.Type type = data.itemMaterials[i].type;

			// アイテムが存在しない
			if (!m_items.ContainsKey(type))
			{
				Debug.Log(type + "が存在しない");
				continue;
			}

			// [type] を [count] 消費する
			m_items[type] -= data.itemMaterials[i].count * value;
		}
	}

	// アイテムの所持数取得
	public int GetItemCount(ItemData.Type type)
	{
		// アイテムが存在しない
		if (!m_items.ContainsKey(type))
		{
			Debug.Log(type + "が存在しない");
			return　0;
		}
		// 所持数を返す
		return m_items[type];
	}




	public Dictionary<ItemData.Type, int> Items
	{
		get { return m_items; }
	}


}

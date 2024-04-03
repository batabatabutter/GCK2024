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
	//[Header("アイテムの吸い込み速度(/s)")]
	//[SerializeField] private float m_suctionSpeed = 0.5f;
	//[Header("アイテムを拾いあげる範囲(半径)")]
	//[SerializeField] private float m_picupRange = 1.0f;


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
		for (ItemData.Type type = ItemData.Type.STONE; type < ItemData.Type.OVER; type++)
		{
			m_items[type] = 0;

			// デバッグがオンになっていたら所持数をカンストさせる
			if (m_debug)
			{
				m_items[type] = m_maxCount;
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

				for (ItemData.Type type = ItemData.Type.STONE; type < ItemData.Type.OVER; type++)
				{
					m_text.text += type.ToString() + " : " + m_items[type] + "\n";
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
			// [type] を [count] 消費する
			m_items[data.itemMaterials[i].type] -= data.itemMaterials[i].count * value;
		}
	}

	// アイテムの所持数取得
	public int GetItemCount(ItemData.Type type)
	{
		return m_items[type];
	}




	public Dictionary<ItemData.Type, int> Items
	{
		get { return m_items; }
	}


}

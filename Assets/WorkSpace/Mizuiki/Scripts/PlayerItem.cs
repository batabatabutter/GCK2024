using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
	[Header("所持品")]
	[SerializeField] private Dictionary<Item.Type, int> m_items = new();
	[Header("最大数")]
	[SerializeField] private int m_maxCount = 99;

	[Header("アイテムの検知範囲(半径)")]
	[SerializeField] private float m_detectionRange = 3.0f;
	[Header("アイテムの吸い込み速度(/s)")]
	[SerializeField] private float m_suctionSpeed = 0.5f;
	[Header("アイテムを拾いあげる範囲(半径)")]
	[SerializeField] private float m_picupRange = 1.0f;


	// Start is called before the first frame update
	void Start()
    {
		// コライダーの追加
		CircleCollider2D col = gameObject.AddComponent<CircleCollider2D>();
		// 半径の設定
		col.radius = m_detectionRange;
		// トリガーに設定
		col.isTrigger = true;

		for (Item.Type type = Item.Type.STONE; type < Item.Type.OVER; type++)
		{
			m_items[type] = 0;
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }


	private void OnTriggerEnter2D(Collider2D collision)
	{
		// タグがアイテム以外
		if (!collision.CompareTag("Item"))
			return;

		// アイテムスクリプトの取得
		if (!collision.TryGetComponent<Item>(out Item item))
			return;

		// アイテムの種類
		Item.Type itemType = item.ItemType;

		// 拾えない
		if (!CheckAcquirable(itemType))
			return;

		// 距離
		float distance = Vector3.Distance(transform.position, collision.transform.position);

		// 拾う
		if (distance < m_picupRange)
		{
			// 拾えるだけ拾う
			m_items[itemType] += item.Picup(m_maxCount - m_items[itemType]);
			Debug.Log("picup");
		}
    }

	private void OnTriggerStay2D(Collider2D collision)
	{
		// タグがアイテム以外
		if (!collision.CompareTag("Item"))
			return;

		// アイテムからプレイヤーへのベクトル
		Vector3 itemToPlayer = transform.position - collision.transform.position;
		// 正規化
		itemToPlayer.Normalize();

		// プレイヤーに近づかせる
		collision.transform.position = collision.transform.position + (itemToPlayer * m_suctionSpeed * Time.deltaTime);
	}


	// 拾えるか確認
	public bool CheckAcquirable(Item.Type itemType)
	{
		// 所持数が最大数に達していない
		if (m_items[itemType] < m_maxCount)
		{
			return true;
		}

		return false;
	}

	// アイテムの所持数取得
	public int GetItemCount(Item.Type type)
	{
		return m_items[type];
	}




	public Dictionary<Item.Type, int> Items
	{
		get { return m_items; }
	}


}

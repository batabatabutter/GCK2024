using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Item : MonoBehaviour
{
    [System.Serializable]
    public enum Type
    {
        STONE,  // 岩
        COAL,   // 石炭

        OVER
    }

    [Header("アイテムの種類")]
    [SerializeField] private Type m_itemType;

    [Header("スタック数")]
    [SerializeField] private int m_count;

    [Header("プレイヤーが拾いあげるまでの時間")]
    [SerializeField] private float m_picupTime = 1.0f;

    // アイテムの元の位置
    private Vector3 m_originalPos = Vector3.zero;
    // プレイヤーのトランスフォーム
    private Transform m_player = null;
    // アイテムの移動時間
    private float m_moveTime = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        // リジッドボディがなければつける
        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.isKinematic = true;
        }

        // 元の位置を設定する
        m_originalPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        // スタック数が0だったら消す
        if (m_count <= 0)
        {
            Destroy(gameObject);
        }
        
        // アイテムの移動
        if (m_moveTime > 0.0f)
        {
            m_moveTime -= Time.deltaTime;

            // プレイヤーのトランスフォームがある
            if (m_player != null)
            {
                // 割合
                float t = m_moveTime / m_picupTime;
                // 座標の設定
                transform.position = Vector3.Lerp(m_player.position, m_originalPos, t);
            }
        }

    }

    /// <summary>
    /// ドロップ
    /// </summary>
    /// <param name="count">ドロップ数</param>
    public void Drop(int count)
    {
        m_count = count;
    }

    /// <summary>
    /// 拾う
    /// </summary>
    /// <param name="count">拾える数</param>
    /// <returns>拾う数</returns>
    public int Picup(int count)
    {
        // 拾う数
		int picUpCount = m_count;

		// 全て拾える
		if (m_count <= count)
        {
            m_count = 0;
        }
        else
        {
            // 拾えるだけ拾う
            m_count -= count;
            picUpCount = count;
        }

        // 拾う数を返す
        return picUpCount;
    }

	// トランスフォームの設定
	public void SetPlayerTransform(Transform player, bool overwrite = false)
	{
		// トランスフォーム上書き
		if (overwrite)
		{
			m_player = player;

			return;
		}

		// トランスフォームが設定されていない
		if (m_player == null)
		{
			m_player = player;
            m_moveTime = m_picupTime;
		}

        // 元の位置を設定
        m_originalPos = transform.position;

	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		// スタック数が 0 以下
		if (m_count <= 0)
			return;

		// アイテムに当たった
		if (collision.CompareTag("Item"))
        {
            // アイテムをくっつける
            MargeItem(collision.GetComponent<Item>());

            return;
        }

        // プレイヤーに当たった
        if (collision.CompareTag("Player"))
        {
            // 拾われる
            PickedUp(collision.GetComponent<Player>());

            return;
        }
	}


    // アイテムをくっつける
    private void MargeItem(Item item)
    {
        // アイテムスクリプトが存在しない
        if (item == null)
            return;

        // アイテムの種類が違う
        if (item.ItemType != m_itemType)
            return;

        // くっつける
        m_count += item.Count;

        // くっつけたアイテムを削除
        item.Count = 0;

    }

    // 拾われる
    private void PickedUp(Player player)
    {
        // プレイヤースクリプトが存在しない
        if (player == null)
            return;

        // 所持アイテムスクリプトの取得
        if (player.transform.Find("Item").TryGetComponent(out PlayerItem playerItem))
        {
            // 拾った数が返ってくる
            m_count -= playerItem.PicUp(m_itemType, m_count);
        }

        m_player = null;
    }




	public Type ItemType
    {
        get { return m_itemType; }
    }

    public int Count
    {
        get { return m_count; }
        set { m_count = value; }
    }
}

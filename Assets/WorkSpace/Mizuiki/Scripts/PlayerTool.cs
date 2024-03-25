using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTool : MonoBehaviour
{
	public class ToolContainer
	{
		public ToolData.ToolType type;      // ツールの情報
		public bool m_isRecast;				// リキャスト中
		public float recastTime;            // リキャスト時間
	}

	[Header("ツールのデータベース")]
	[SerializeField] private ToolDataBase m_dataBase = null;

	[Header("アイテム")]
	[SerializeField] private PlayerItem m_playerItem;

	[Header("設置ツール")]
	[SerializeField] private Dictionary<ToolData.ToolType, ToolContainer> m_tools = new();


	// Start is called before the first frame update
	void Start()
    {
		// アイテムがなければ取得
		if (m_playerItem == null)
		{
			if (TryGetComponent(out PlayerItem item))
			{
				m_playerItem = item;
			}
		}

		// ツールの作成
		for (ToolData.ToolType type = ToolData.ToolType.TOACH; type < ToolData.ToolType.OVER; type++)
		{
			m_tools[type] = new ToolContainer();
		}
    }

    // Update is called once per frame
    void Update()
    {
		// ツールの更新
		for (ToolData.ToolType type = ToolData.ToolType.TOACH; type < ToolData.ToolType.OVER; type++)
		{
			// リキャスト中
			if (m_tools[type].m_isRecast)
			{
				// 時間経過
				m_tools[type].recastTime -= Time.deltaTime;
			}
			// リキャスト時間が明けている
			if (m_tools[type].recastTime <= 0.0f)
			{
				// リキャストのフラグをオフにする
				m_tools[type].m_isRecast = false;
			}
		}


	}


	// 使用可能
	public bool Available(ToolData.ToolType type)
	{
		// リキャスト中
		if(m_tools[type].m_isRecast)
		{
			// 使用不可能
			return false;
		}
		// 使用可能
		return true;
	}

	// ツールを使用する
	public void UseTool(ToolData.ToolType type, Vector3 position)
	{
		// 設置ツール
		if (m_dataBase.tool[(int)type].objectPrefab)
		{
			// アイテムを置く
			GameObject tool = Instantiate(m_dataBase.tool[(int)type].objectPrefab, position, Quaternion.identity);
			// アクティブにする(念のため)
			tool.SetActive(true);
			// リキャスト時間の設定
			m_tools[type].recastTime = m_dataBase.tool[(int)type].recastTime;
			m_tools[type].m_isRecast = true;
		}

		// 素材を消費する
		m_playerItem.ConsumeMaterials(m_dataBase.tool[(int)type]);

	}

	// ツールを作成できるかチェック
	public bool CheckCreate(ToolData.ToolType type)
	{
		ToolData data = m_dataBase.tool[(int)type];

		if (data != null)
		{
			return CheckCreate(data);
		}

		// 選択ツールが存在しない
		return false;
	}
	private bool CheckCreate(ToolData data)
	{
		// 素材の種類分ループ
		for (int i = 0; i < data.itemMaterials.Count; i++)
		{
			ItemData.Type type = data.itemMaterials[i].type;
			int count = data.itemMaterials[i].count;

			// 所持アイテム数が必要素材数未満
			if (m_playerItem.Items[type] < count)
			{
				// 作成できない
				return false;
			}

		}
		// 必要素材数所持している
		return true;
	}



	// ツールのリキャスト時間
	public float RecastTime(ToolData.ToolType type)
	{
		return m_tools[type].recastTime;
	}

}

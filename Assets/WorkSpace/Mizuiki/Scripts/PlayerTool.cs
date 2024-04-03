using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTool : MonoBehaviour
{
	public class ToolContainer
	{
		//public ToolData data	= null;      // ツールの情報
		public bool available	= true;		// 使用可能
		public bool isRecast	= false;	// リキャスト中
		public float recastTime = 0.0f;     // リキャスト時間
	}

	[Header("ツールのデータベース")]
	[SerializeField] private ToolDataBase m_dataBase = null;

	[Header("アイテム")]
	[SerializeField] private PlayerItem m_playerItem;

	[Header("設置ツール")]
	[SerializeField] private Dictionary<ToolData.ToolType, ToolContainer> m_tools = new();

	// ツール更新用の空のオブジェクト
	//private GameObject m_toolObject = null;
	private Dictionary<ToolData.ToolType, Tool> m_toolScripts = new();


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
		for (ToolData.ToolType type = 0; type < ToolData.ToolType.OVER; type++)
		{
			// 新たなツールの作成
			m_tools[type] = new ToolContainer();
			//// データベースの情報を取得
			//m_tools[type].data = m_dataBase.tool[(int)type];

			// ツール更新用
			if (m_dataBase.tool[(int)type].tool)
			{
				m_toolScripts[type] = Instantiate(m_dataBase.tool[(int)type].tool, transform);
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
		// ツールの更新
		for (ToolData.ToolType type = 0; type < ToolData.ToolType.OVER; type++)
		{
			// リキャスト中
			if (m_tools[type].isRecast)
			{
				// 時間経過
				m_tools[type].recastTime -= Time.deltaTime;
			}
			// リキャスト時間が明けている
			if (m_tools[type].recastTime <= 0.0f)
			{
				// リキャストのフラグをオフにする
				m_tools[type].isRecast = false;
				// 使用可能にする
				m_tools[type].available = true;
			}
		}


	}


	// 使用可能
	public bool Available(ToolData.ToolType type)
	{
		// リキャスト中
		if(m_tools[type].available)
		{
			// 使用可能
			return true;
		}
		// 使用不可能
		return false;
	}

	// ツールを使用する
	public void UseTool(ToolData.ToolType type, Vector3 position)
	{
		// 呼び出す関数が登録されている
		if (m_dataBase.tool[(int)type].tool)
		{
			// ツール使用の処理を呼び出す
			m_toolScripts[type].UseTool(gameObject);

			// リキャスト時間の設定
			m_tools[type].recastTime = m_dataBase.tool[(int)type].recastTime;

			// 使用不可能にする
			m_tools[type].available = false;

		}
		// 設置ツール
		else if (m_dataBase.tool[(int)type].objectPrefab)
		{
			// ツールの設置
			Put(m_dataBase.tool[(int)type], position);
			// リキャスト時間の設定
			m_tools[type].recastTime = m_dataBase.tool[(int)type].recastTime;
			m_tools[type].isRecast = true;
		}

		// 素材を消費する
		m_playerItem.ConsumeMaterials(m_dataBase.tool[(int)type]);

	}

	// 設置する
	public void Put(ToolData data, Vector3 position, bool con = false)
	{
		// プレハブが設定されていなければ返す
		if (!data.objectPrefab)
			return;

		// アイテムを置く
		GameObject tool = Instantiate(data.objectPrefab, position, Quaternion.identity);
		// アクティブにする(念のため)
		tool.SetActive(true);

		// 素材を消費する
		if (con)
		{
			m_playerItem.ConsumeMaterials(data);
		}

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
	public bool CheckCreate(ToolData data, int value = 1)
	{
		// 素材の種類分ループ
		for (int i = 0; i < data.itemMaterials.Count; i++)
		{
			// アイテムの種類
			ItemData.Type type = data.itemMaterials[i].type;
			// 必要数
			int count = data.itemMaterials[i].count * value;

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

	// 素材の消費
	public void ConsumeMaterials(ToolData data, int value = 1)
	{
		m_playerItem.ConsumeMaterials(data, value);
	}


	// ツールのリキャスト時間の取得
	public float RecastTime(ToolData.ToolType type)
	{
		return m_tools[type].recastTime;
	}

	// ツールの分類を取得
	public ToolData.ToolCategory GetCategory(ToolData.ToolType type)
	{
		return m_dataBase.tool[(int)type].category;//m_tools[type].data.category;
	}

	// ツールのリキャスト状態の設定
	public void SetRecast(bool recast, ToolData.ToolType type)
	{
		m_tools[type].isRecast = recast;
	}

}

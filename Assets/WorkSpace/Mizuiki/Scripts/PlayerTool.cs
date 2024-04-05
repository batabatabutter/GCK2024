using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTool : MonoBehaviour
{
	public class ToolContainer
	{
		public ToolData data = null;		// ツールの情報
		public bool create		= true;		// 作成可能
		public bool available	= true;		// 使用可能
		public bool isRecast	= false;	// リキャスト中
		public float recastTime = 0.0f;     // リキャスト時間
	}

	[Header("ツールのデータベース")]
	[SerializeField] private ToolDataBase m_dataBase = null;

	[Header("アイテム")]
	[SerializeField] private PlayerItem m_playerItem;

	[Header("ツール")]
	[SerializeField] private Dictionary<ToolData.ToolType, ToolContainer> m_tools = new();

	// ツール更新用のオブジェクト
	private Dictionary<ToolData.ToolType, Tool> m_toolScripts = new();

	// レアツールを選択
	private bool m_rare = false;

	// 選択ツール
	private ToolData.ToolType m_toolType = 0;
	// 選択レアツール
	private ToolData.ToolType m_toolTypeRare = ToolData.ToolType.RARE + 1;

	[Header("デバッグ---------------------------")]
	[SerializeField] private bool m_debug = false;
	[SerializeField] private Text m_text = null;



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
		foreach (ToolData toolData in m_dataBase.tool)
		{
			// ツールの種類
			ToolData.ToolType type = toolData.type;

			// 上書き防止
			if (m_tools.ContainsKey(type))
				continue;

			// 新たなツールの作成
			m_tools[type] = new()
			{
				// データベースの情報を設定
				data = toolData
			};

			// サポートツール更新用
			if (toolData.category == ToolData.ToolCategory.SUPPORT)
			{
				if (toolData.prefab)
				{
					m_toolScripts[type] = Instantiate(toolData.prefab.GetComponent<Tool>(), transform);
				}
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
		// ツールの更新
		foreach(ToolContainer tool in m_tools.Values)
		{
			// ツールの種類取得
			ToolData.ToolType type = tool.data.type;

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

		// デバッグ
		if (m_debug)
		{
			if (m_text != null)
			{
				if (m_rare)
				{
					m_text.text = m_toolTypeRare.ToString();
				}
				else
				{
					m_text.text = m_toolType.ToString();
				}
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

	// ツール変更
	public void ChangeTool(int val)
	{
		// RAREを取得
		ToolData.ToolType rare = ToolData.ToolType.RARE;

		// ツールの種類のリスト取得
		List<ToolData.ToolType> typeList = new(m_tools.Keys);

		// レアツール
		if (m_rare)
		{
			// RARE 以下の値を削除
			typeList.RemoveAll(type => type <= rare);

			// 要素数が 0 なら処理しない
			if (typeList.Count == 0)
				return;

			// 現在選択中のツールのインデックス
			int index = typeList.IndexOf(m_toolTypeRare);

			// 変更後の値
			int change = index - val;

			// 変更後が 0 未満
			while (change < 0)
			{
				// 先頭からはみ出した分
				change += typeList.Count;
			}
			// 変更後が範囲外
			while (change >= typeList.Count)
			{
				change -= typeList.Count;
			}

			// 変更後の値を設定
			m_toolTypeRare = typeList[change];

		}
		// 通常ツール
		else
		{
			// RARE 以上の値を削除
			typeList.RemoveAll(type => type >= rare);

			// 要素数が 0 なら処理しない
			if (typeList.Count == 0)
				return;

			// 現在選択中のツールのインデックス
			int index = typeList.IndexOf(m_toolType);

			// 変更後の値
			int change = index + val;

			// 変更後が 0 未満
			while (change < 0)
			{
				// 先頭からはみ出した分
				change += typeList.Count;

			}
			// 変更後が範囲外
			while (change >= typeList.Count)
			{
				// 末尾からはみ出した分
				change -= typeList.Count;
			}

			// 変更後の値を設定
			m_toolType = typeList[change];
		}

	}

	// ツール切り替え
	public void SwitchTool()
	{
		m_rare = !m_rare;

	}

	// ツールを使用する
	public void UseTool(Vector3 position)
	{
		if (m_rare)
		{
			UseTool(m_toolTypeRare, position);
		}
		else
		{
			UseTool(m_toolType, position);
		}
	}
	public void UseTool(ToolData.ToolType type, Vector3 position)
	{
		// ツールが存在しない
		if (!m_tools.ContainsKey(type))
		{
            Debug.Log("ツールが存在しないよ");
			return;
		}

		// 選択されているアイテムが作成できない
		if (!CheckCreate(type))
		{
			Debug.Log("素材不足");
			return;
		}

		// クールタイム中なら設置できない
		if (!Available(type))
		{
			Debug.Log("クールタイム中");
			return;
		}

		// ツールのデータ取得
		ToolData data = m_tools[type].data;
		// ツールの分類
		switch (data.category)
		{
			case ToolData.ToolCategory.PUT:			// 設置型
				// ツールの設置
				Put(data, position);
				// リキャスト時間の設定
				m_tools[type].recastTime = data.recastTime;
				m_tools[type].isRecast = true;
				break;

			case ToolData.ToolCategory.SUPPORT:		// 適応型
				// ツール使用の処理を呼び出す
				m_toolScripts[type].UseTool(gameObject);
				// リキャスト時間の設定
				m_tools[type].recastTime = data.recastTime;
				// 使用不可能にする
				m_tools[type].available = false;
				break;
		}

		// 素材を消費する
		m_playerItem.ConsumeMaterials(data);

	}

	// 設置する
	public void Put(ToolData data, Vector3 position, bool con = false)
	{
		// プレハブが設定されていなければ返す
		if (!data.prefab)
			return;

		// アイテムを置く
		GameObject tool = Instantiate(data.prefab, position, Quaternion.identity);
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
		// ツールのデータ取得
		ToolData data = m_tools[type].data;

		// データがあれば作成可能かチェック
		if (data)
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

			// アイテムが存在しない
			if (!m_playerItem.Items.ContainsKey(type))
			{
				Debug.Log("アイテムが存在しない");
				return false;
			}

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


	// ツールの分類を取得
	public ToolData.ToolCategory GetCategory(ToolData.ToolType type)
	{
		return m_tools[type].data.category;
	}
	// ツールの作成可否を取得
	public bool GetCreate(ToolData.ToolType type)
	{
		return m_tools[type].create;
	}
	// ツールのリキャスト時間の取得
	public float RecastTime(ToolData.ToolType type)
	{
		return m_tools[type].recastTime;
	}

	// ツールのリキャスト状態の設定
	public void SetRecast(bool recast, ToolData.ToolType type)
	{
		m_tools[type].isRecast = recast;
	}

	// 選択ツール
	public ToolData.ToolType ToolType
	{
		get { return m_toolType; }
	}
	// 選択レアツール
	public ToolData.ToolType ToolTypeRare
	{
		get { return m_toolTypeRare; }
	}

}

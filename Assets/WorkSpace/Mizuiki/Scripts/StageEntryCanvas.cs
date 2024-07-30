using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageEntryCanvas : CheckCanvas
{
	[Header("---------- 入口 ----------")]

	[Header("ステージ番号")]
	[SerializeField] private int m_stageNum = 0;

	[Header("シーンマネージャ"), Tooltip("シーン切り替え用")]
	[SerializeField] private StageSelectManager m_stageSelectManager = null;

	[Header("テキスト")]
	[SerializeField] private Text m_text;

	[Header("レベル変更ボタン")]
	[SerializeField] private Image m_buttonLevelDown = null;
	[SerializeField] private Image m_buttonLevelUp = null;

	[Header("レベル表示テキスト")]
	[SerializeField] private InputField m_inputFieldLevel = null;


	private void OnEnable()
	{
		// 最大レベル
		int maxLevel = SaveDataReadWrite.m_instance.GetDungeonLevel(m_stageNum);
		SetStageLevel(maxLevel);
	}

	private void Start()
	{
		ChangeStageLevel();
	}

	// 決定
	public override void Decision()
	{
		base.Decision();

		// シーン切り替え
		m_stageSelectManager.ChangeScene(m_stageNum);
	}

	// ステージレベル変更
	public void ChangeStageLevel()
	{
		ChangeStageLevel(0);
	}
	public void ChangeStageLevel(int change)
	{
		// 現在のステージレベル
		int now = int.Parse(m_inputFieldLevel.text);
		// 変動量を加算
		now += change;
		// レベル設定
		SetStageLevel(now);
	}
	// ステージレベル設定
	public void SetStageLevel(int level)
	{
		// 最大レベル
		int maxLevel = MyFunction.MAX_STAGE_LEVEL;
		if (SaveDataReadWrite.m_instance)
		{
			maxLevel = Mathf.Min(SaveDataReadWrite.m_instance.GetDungeonLevel(m_stageNum));
		}
		// ステージレベルでクランプ(最大値を超えないように)
		level = Mathf.Clamp(level, 1, maxLevel);
		// ステージレベル書き換え
		m_inputFieldLevel.text = level.ToString();

		// 表示切替
		ChangeArrow(level, maxLevel);
	}

	// ステージ番号
	public int StageNum
	{
		get { return m_stageNum; }
		set { m_stageNum = value; }
	}
	public string Text
	{
		set { m_text.text = value; }
	}



	// 矢印の表示変更
	private void ChangeArrow(int level, int max)
	{
		if (level == 1)
		{
			// レベル減少ボタン非表示
			m_buttonLevelDown.gameObject.SetActive(false);
		}
        else
        {
			// レベル減少ボタン表示
			m_buttonLevelDown.gameObject.SetActive(true);
        }

		if (level == MyFunction.MAX_STAGE_LEVEL)
		{
			// レベル増加ボタン非表示
			m_buttonLevelUp.gameObject.SetActive(false);
		}
		else
		{
			// レベル増加ボタン表示
			m_buttonLevelUp.gameObject.SetActive(true);
		}

		if (level < max)
		{
			// まだ上がれる
			m_buttonLevelUp.color = Color.white;
		}
		else
		{
			// 今はここまで
			m_buttonLevelUp.color = Color.gray;
		}

    }

}

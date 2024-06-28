using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCanvas : MonoBehaviour
{
	[Header("自分自身のキャンバス")]
	[SerializeField] private Canvas m_canvas = null;


	private void Awake()
	{
		// キャンバスが設定されていなければ取得
		if (m_canvas == null)
		{
			m_canvas = GetComponent<Canvas>();
		}
		// 非表示
		SetEnabled(false);
	}

	// 決定
	public virtual void Decision()
	{

	}

	// キャンセル
	public virtual void Cancel()
	{
		// キャンバス非表示
		SetEnabled(false);
	}

	// キャンバス表示
	public virtual void SetEnabled(bool enabled)
	{
		m_canvas.gameObject.SetActive(enabled);
	}


}

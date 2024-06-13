using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
	[Header("ツール")]
	[SerializeField] private List<Tool> m_tools = new List<Tool>();


	public void AddTool(Tool tool)
	{
		m_tools.Add(tool);
	}

	public void SetEnabled(bool enabled)
	{
		// null はリストから削除
		m_tools.RemoveAll(t => t == null);

		foreach(Tool tool in m_tools)
		{
			tool.enabled = enabled;
		}
	}

}

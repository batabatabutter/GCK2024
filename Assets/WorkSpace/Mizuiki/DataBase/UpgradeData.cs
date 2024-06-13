
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData_", menuName = "CreateDataBase/Upgrade/UpgradeData")]
public class UpgradeData : ScriptableObject
{
	[Header("������")]
	[SerializeField] private MiningData.MiningValue value = MiningData.MiningValue.Zero();
	[Header("�K�v�f��")]
	[SerializeField] private Items[] cost;

	public MiningData.MiningValue Value => value;
	public Items[] Cost => cost;
}
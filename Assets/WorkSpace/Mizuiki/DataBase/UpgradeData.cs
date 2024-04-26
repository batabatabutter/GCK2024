
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData_", menuName = "CreateDataBase/Upgrade/UpgradeData")]
public class UpgradeData : ScriptableObject
{
	[Header("‘‰Á—Ê")]
	[SerializeField] private PlayerMining.MiningValue value = PlayerMining.MiningValue.Zero();
	[Header("•K—v‘fÞ")]
	[SerializeField] private Items[] cost;

	public PlayerMining.MiningValue Value => value;
	public Items[] Cost => cost;
}
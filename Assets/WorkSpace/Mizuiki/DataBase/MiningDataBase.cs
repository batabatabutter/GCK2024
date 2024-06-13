using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiningDataBase", menuName = "CreateDataBase/Mining/MiningDataBase")]
public class MiningDataBase : ScriptableObject
{
	[Header("çÃå@íl")]
	[SerializeField] private List<MiningData> miningData = new();



	public List<MiningData> MiningData => miningData;

}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "CreateEnemyData")]

public class EnemyData : ScriptableObject
{
    //今は使わない
    [Header("HP")]
    public int hp;

    [Header("攻撃間隔")]
    public float coolTime;

    //[Header("ドロップアイテム")]
    public List<BlockData.DropItems> dropItems;


}

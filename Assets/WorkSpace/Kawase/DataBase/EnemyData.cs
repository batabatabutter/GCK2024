using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "CreateEnemyData")]

public class EnemyData : ScriptableObject
{
    //プレハブ
    public GameObject prefab;
    //系統
    public Enemy.System system;

    //今は使わない
    [Header("HP")]
    public int hp;

    [Header("攻撃間隔")]
    public float coolTime;

    //[Header("ドロップアイテム")]
    public List<BlockData.DropItems> dropItems;

}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "CreateEnemyData")]

public class EnemyData : ScriptableObject
{
    //���͎g��Ȃ�
    [Header("HP")]
    public int hp;

    [Header("�U���Ԋu")]
    public float coolTime;

    //[Header("�h���b�v�A�C�e��")]
    public List<BlockData.DropItems> dropItems;


}

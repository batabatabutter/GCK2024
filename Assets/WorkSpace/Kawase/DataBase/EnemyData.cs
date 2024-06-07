using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "CreateEnemyData")]

public class EnemyData : ScriptableObject
{
    //�v���n�u
    public GameObject prefab;
    //�n��
    public Enemy.System system;

    //���͎g��Ȃ�
    [Header("HP")]
    public int hp;

    [Header("�U���Ԋu")]
    public float coolTime;

    [Header("�U���͈͂̔��a")]
    public float radius;

    [Header("����������")]
    [SerializeField] private AudioClip genereateSE;
    [Header("���S������")]
    [SerializeField] private AudioClip deathSE;

    //[Header("�h���b�v�A�C�e��")]
    public List<BlockData.DropItems> dropItems;

    public AudioClip GenerateSE => genereateSE;
    public AudioClip DeathSE => deathSE;
}

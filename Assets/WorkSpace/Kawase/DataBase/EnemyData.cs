using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "CreateEnemyData")]

public class EnemyData : ScriptableObject
{
    //ƒvƒŒƒnƒu
    public GameObject prefab;
    //Œn“
    public Enemy.System system;

    //¡‚Íg‚í‚È‚¢
    [Header("HP")]
    public int hp;

    [Header("UŒ‚ŠÔŠu")]
    public float coolTime;

    [Header("UŒ‚”ÍˆÍ‚Ì”¼Œa")]
    public float radius;

    [Header("¶¬‰¹º")]
    [SerializeField] private AudioClip genereateSE;
    [Header("€–S‰¹º")]
    [SerializeField] private AudioClip deathSE;

    //[Header("ƒhƒƒbƒvƒAƒCƒeƒ€")]
    public List<BlockData.DropItems> dropItems;

    public AudioClip GenerateSE => genereateSE;
    public AudioClip DeathSE => deathSE;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : Block
{
    [Header("ダンジョンのサイズ")]
    [SerializeField] private Vector3Int m_dungeonSize = new(5, 5, 1);

    [Header("ダンジョンの基準位置")]
    [SerializeField] private Vector3 m_dungeonPosition = new(0.0f, 0.0f, 0.0f);

    [Header("核の位置")]
    [SerializeField] private Vector3 m_dungeonCorePosition = new(0.0f, 0.0f, 0.0f);

    //[Header("プレイヤー")]
    //[SerializeField] private Player m_player = null;


    // Start is called before the first frame update
    void Start()
    {
        // ダンジョンのサイズの当たり判定を作る
         //Collider2D dungeonRange = gameObject.AddComponent<Collider2D>();
        //dungeonRange;

    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーの上かつ空いてるグリッドの上に岩を降らせる


    }


	// ダンジョンの範囲内にプレイヤーがいる



	/// <summary>
	/// ダンジョンのサイズ
	/// </summary>
	public Vector3Int Size
    {
        get { return m_dungeonSize; }
        set { m_dungeonSize = value; }
    }
    /// <summary>
    /// ダンジョンの基準位置(左下座標)
    /// </summary>
    public Vector3 Position
    {
        get { return m_dungeonPosition; }
        set { m_dungeonPosition = value; }
    }
    /// <summary>
    /// 核の位置
    /// </summary>
    public Vector3 CorePosition
    {
        get { return m_dungeonCorePosition; }
        set { m_dungeonCorePosition = value; }
    }
}

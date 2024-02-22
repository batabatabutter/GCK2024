using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : Block
{
    [Header("�_���W�����̃T�C�Y")]
    [SerializeField] private Vector3Int m_dungeonSize = new(5, 5, 1);

    [Header("�_���W�����̊�ʒu")]
    [SerializeField] private Vector3 m_dungeonPosition = new(0.0f, 0.0f, 0.0f);

    [Header("�j�̈ʒu")]
    [SerializeField] private Vector3 m_dungeonCorePosition = new(0.0f, 0.0f, 0.0f);

    //[Header("�v���C���[")]
    //[SerializeField] private Player m_player = null;


    // Start is called before the first frame update
    void Start()
    {
        // �_���W�����̃T�C�Y�̓����蔻������
         //Collider2D dungeonRange = gameObject.AddComponent<Collider2D>();
        //dungeonRange;

    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[�̏ォ�󂢂Ă�O���b�h�̏�Ɋ���~�点��


    }


	// �_���W�����͈͓̔��Ƀv���C���[������



	/// <summary>
	/// �_���W�����̃T�C�Y
	/// </summary>
	public Vector3Int Size
    {
        get { return m_dungeonSize; }
        set { m_dungeonSize = value; }
    }
    /// <summary>
    /// �_���W�����̊�ʒu(�������W)
    /// </summary>
    public Vector3 Position
    {
        get { return m_dungeonPosition; }
        set { m_dungeonPosition = value; }
    }
    /// <summary>
    /// �j�̈ʒu
    /// </summary>
    public Vector3 CorePosition
    {
        get { return m_dungeonCorePosition; }
        set { m_dungeonCorePosition = value; }
    }
}

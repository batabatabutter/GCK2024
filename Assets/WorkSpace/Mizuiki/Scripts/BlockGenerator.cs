using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [Header("�u���b�N�̃f�[�^�x�[�X")]
    [SerializeField] private BlockDataBase m_blockDataBase = null;

    [Header("�}�b�v�I�u�W�F�N�g")]
    [SerializeField] private GameObject m_mapObject = null;
    [Header("�}�b�v�̖ډB��")]
    [SerializeField] private GameObject m_mapBlind = null;


	private void Awake()
	{
		// �u���b�N�f�[�^�x�[�X���Ȃ�
		if (m_blockDataBase == null)
		{
            Debug.Log(gameObject.name + "�Ƀu���b�N�f�[�^�x�[�X��ݒ肵�Ă�");

			//m_blockDataBase = AssetDatabase.LoadAssetAtPath<BlockDataBase>("Assets/DataBase/Block/BlockDataBase.asset");
		}
	}

	// Start is called before the first frame update
	void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// �u���b�N�𐶐�
    /// </summary>
    /// <param name="type">��������u���b�N�̎��</param>
    /// <param name="position">����������W</param>
    /// <param name="parent">�e</param>
    /// <param name="isBrightness">���邳�����邩�ǂ���</param>
    public GameObject GenerateBlock(BlockData.BlockType type, Vector2 position, Transform parent = null, bool isBrightness = false)
    {
		// �u���b�N�̃f�[�^�擾
		BlockData data = MyFunction.GetBlockData(m_blockDataBase, type);

        // �f�[�^���Ȃ�
        if (data == null)
            return null;

        // �v���n�u���ݒ肳��Ă��Ȃ�
        if (!data.Prefab)
        {
            Debug.Log(data.name + "�Ƀu���b�N�̃v���n�u��ݒ肵�Ă�");

            return null;
            //data.prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Block.prefab");
        }

		// ���������u���b�N��ݒ肷��p
		GameObject obj;

        // �e���w�肳��Ă���
        if (parent)
        {
            // �e��ݒ肵�Đ���
            obj = Instantiate(data.Prefab, position, Quaternion.identity, parent);
        }
        // �e�̎w��͂Ȃ�
        else
        {
            // �w����W�ɐ���
            obj = Instantiate(data.Prefab, position, Quaternion.identity);
        }

        // �f�[�^�̐ݒ�
        if (obj.TryGetComponent(out Block block))
        {
            // �f�[�^
            block.BlockData = data;
            // �ϋv
            block.Endurance = data.Endurance;
            // �j��s��
            block.DontBroken = data.DontBroken;
            // �������x��
            block.LightLevel = data.LightLevel;
        }

		// �摜�̐ݒ�
		if (data.Sprite)
		{
			if (obj.TryGetComponent(out SpriteRenderer sprite))
			{
				sprite.sprite = data.Sprite;
			}
		}

		//���邳�̒ǉ�
		if (isBrightness)
        {
            obj.AddComponent<ChangeBrightness>();
        }


        if (m_mapObject)
        {
            // �}�b�v�I�u�W�F�N�g�̐���
            GameObject mapObj = Instantiate(m_mapObject, obj.transform);
            // �F�̐ݒ�
            mapObj.GetComponent<SpriteRenderer>().color = data.Color;
            mapObj.GetComponent<MapObject>().BlockColor = data.Color;
            // �\�����̐ݒ�
            mapObj.GetComponent<SpriteRenderer>().sortingOrder = data.Order;
            // �X�v���C�g�̐ݒ�
            mapObj.GetComponent<MapObject>().ParentSprite = obj.gameObject.GetComponent<SpriteRenderer>();

        }
		if (m_mapBlind)
		{
			// �}�b�v�̖ډB������
			//Instantiate(m_mapBlind, block.transform);
		}


        return obj;
	}
}
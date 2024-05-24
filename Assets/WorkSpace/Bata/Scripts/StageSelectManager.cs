using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    [Header("�X�e�[�W�Z���N�g�u���b�N")]
    [SerializeField] private GameObject m_stageSelectBlock;

    [Header("�X�e�[�W���")]
    [SerializeField] private DungeonDataBase m_dungeonDataBase;

    [Header("�u���b�N�Ԃ̌���")]
    [SerializeField] private Vector3 m_offset;

    [Header("�n�ʉ摜")]
    [SerializeField] private GameObject m_groundObj;

    [Header("�{�[�_�[�I�u�W�F�N�g")]
    [SerializeField] private GameObject m_borderObj;

    [Header("�ړ��͈�")]
    [SerializeField] private Vector3 m_minPos;
    [SerializeField] private Vector3 m_maxPos;
    [Header("�I�t�Z�b�g")]
    [SerializeField] private float m_borderOffset;

    // Start is called before the first frame update
    void Start()
    {
        //  �e�쐬
        GameObject parent = new GameObject("Grounds");
        for (int x = (int)(m_minPos.x - m_borderOffset); x < (int)(m_maxPos.x + m_borderOffset); x++)
        {
            for (int y = (int)(m_minPos.y - m_borderOffset); y < (int)(m_maxPos.y + m_borderOffset); y++)
            {
                //  ����
                var gr = Instantiate(m_groundObj, parent.transform);
                gr.transform.position = new Vector3(x, y, 0);
            }
        }

        //  �e�쐬
        parent = new GameObject("Borders");
        for (int x = (int)m_minPos.x; x < (int)m_maxPos.x; x++)
        {
            //  ����
            GameObject br = Instantiate(m_borderObj, parent.transform);
            br.transform.position = new Vector3(x, m_minPos.y, 0);
            br = Instantiate(m_borderObj, parent.transform);
            br.transform.position = new Vector3(x, m_maxPos.y, 0);
        }
        for (int y = (int)m_minPos.y; y < (int)m_maxPos.y; y++)
        {
            //  ����
            GameObject br = Instantiate(m_borderObj, parent.transform);
            br.transform.position = new Vector3(m_minPos.x, y, 0);
            br = Instantiate(m_borderObj, parent.transform);
            br.transform.position = new Vector3(m_maxPos.x, y, 0);
        }

        //  �e�쐬
        parent = new GameObject("StageSelectBlock");
        //  �_���W���������u���b�N����
        for (int i = 0; i < m_dungeonDataBase.dungeonDatas.Count; i++)
        {
            var bl = Instantiate(m_stageSelectBlock, parent.transform);
            bl.GetComponent<StageSelectBlock>().SetStageNum(i);
            Vector3 pos = bl.transform.position;
            pos.x = i * bl.transform.localScale.x;
            bl.transform.position = pos + m_offset * i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

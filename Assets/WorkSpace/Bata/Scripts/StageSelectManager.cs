using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    [Header("�X�e�[�W�Z���N�g�u���b�N")]
    [SerializeField] GameObject m_stageSelectBlock;

    [Header("�X�e�[�W���")]
    [SerializeField] DungeonDataBase m_dungeonDataBase;

    [Header("�u���b�N�Ԃ̌���")]
    [SerializeField] Vector3 m_offset;

    // Start is called before the first frame update
    void Start()
    {
        GameObject parent = new GameObject("StageSelectBlock");

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

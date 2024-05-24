using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    [Header("ステージセレクトブロック")]
    [SerializeField] GameObject m_stageSelectBlock;

    [Header("ステージ情報")]
    [SerializeField] DungeonDataBase m_dungeonDataBase;

    [Header("ブロック間の隙間")]
    [SerializeField] Vector3 m_offset;

    // Start is called before the first frame update
    void Start()
    {
        GameObject parent = new GameObject("StageSelectBlock");

        //  ダンジョン数分ブロック生成
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

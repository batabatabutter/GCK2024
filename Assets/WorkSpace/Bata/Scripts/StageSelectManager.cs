using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
	[Header("ステージ番号")]
	[SerializeField] StageNumScriptableObject m_stageNumObj;

    [Header("プレイヤー")]
    [SerializeField] private GameObject m_player = null;

	[Header("ステージセレクトブロック")]
    [SerializeField] private GameObject m_stageSelectBlock;

    [Header("ステージ情報")]
    [SerializeField] private DungeonDataBase m_dungeonDataBase;

    [Header("ブロック間の隙間")]
    [SerializeField] private float m_offset;
    [Header("ブロックの中心")]
    [SerializeField] private Vector3 m_blockCenter = Vector3.zero;

    [Header("地面画像")]
    [SerializeField] private GameObject m_groundObj;

    [Header("ボーダーオブジェクト")]
    [SerializeField] private GameObject m_borderObj;

    [Header("移動範囲")]
    [SerializeField] private Vector3 m_minPos;
    [SerializeField] private Vector3 m_maxPos;
    [Header("オフセット")]
    [SerializeField] private float m_borderOffset;

    // Start is called before the first frame update
    void Start()
    {
        //  地面の親作成
        GameObject parent = new GameObject("Grounds");
        parent.transform.parent = transform;
        for (int x = (int)(m_minPos.x - m_borderOffset); x < (int)(m_maxPos.x + m_borderOffset); x++)
        {
            for (int y = (int)(m_minPos.y - m_borderOffset); y < (int)(m_maxPos.y + m_borderOffset); y++)
            {
                //  生成
                var gr = Instantiate(m_groundObj, parent.transform);
                gr.transform.position = new Vector3(x, y, 0);
            }
        }

        //  範囲枠の親作成
        parent = new GameObject("Borders");
		parent.transform.parent = transform;
		for (int x = (int)m_minPos.x; x < (int)m_maxPos.x; x++)
        {
            //  生成
            GameObject br = Instantiate(m_borderObj, parent.transform);
            br.transform.position = new Vector3(x, m_minPos.y, 0);
            br = Instantiate(m_borderObj, parent.transform);
            br.transform.position = new Vector3(x, m_maxPos.y, 0);
        }
        for (int y = (int)m_minPos.y; y < (int)m_maxPos.y; y++)
        {
            //  生成
            GameObject br = Instantiate(m_borderObj, parent.transform);
            br.transform.position = new Vector3(m_minPos.x, y, 0);
            br = Instantiate(m_borderObj, parent.transform);
            br.transform.position = new Vector3(m_maxPos.x, y, 0);
        }

        //  選択項目の親作成
        parent = new GameObject("StageSelectBlock");
		parent.transform.parent = transform;
		// 一つ分の角度
		float degree = 360.0f / m_dungeonDataBase.dungeonDatas.Count;
        //  ダンジョン数分ブロック生成
        for (int i = 0; i < m_dungeonDataBase.dungeonDatas.Count; i++)
        {
            // 選択肢生成
            var bl = Instantiate(m_stageSelectBlock, parent.transform);
            // ステージ番号設定
            bl.GetComponent<StageSelectBlock>().SetStageNum(i);
            // マネージャ設定
            bl.GetComponent<StageSelectBlock>().StageSelectManager = this;
            // コアのスプライト設定
            bl.GetComponent<SpriteRenderer>().sprite = m_dungeonDataBase.dungeonDatas[i].CoreSprite;
            // 角度(右回りにするためにマイナスをつける)
            float angle = -degree * i * Mathf.Deg2Rad;
            // 配置場所
            Vector3 current = Vector3.up * m_offset;
            Vector3 pos = Vector3.zero;
            // ステージ分の角度回転
            pos.x = (current.x * Mathf.Cos(angle)) - (current.y * Mathf.Sin(angle));
            pos.y = (current.x * Mathf.Sin(angle)) + (current.y * Mathf.Cos(angle));
            // 中心座標分移動
            pos += m_blockCenter;
            // 座標設定
            bl.transform.position = MyFunction.RoundHalfUp(pos);
        }
    }

    public void ChangeScene(int stageNum)
    {
		// ステージ番号設定
		m_stageNumObj.stageNum = stageNum;
		// 装備設定
		SaveDataReadWrite.m_instance.MiningType = m_player.GetComponent<PlayerMining>().CircularSaw.MiningType;
		// セーブ
		SaveDataReadWrite.m_instance.Save();
		// シーン読み込み
		SceneManager.LoadScene("PlayScene");

	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeBrightness : MonoBehaviour
{
	//明るさの最大値
	static private int MAX_BRIGHTNESS = 7;
    public int GetMAXBRIGHTBESS() { return MAX_BRIGHTNESS; }


	//LightList
	public List<ObjectLight> m_lightList = new();
    public List<Vector3> m_lightPositionList = new();

    //  プレイヤーの座標
    private Transform m_playerTr;
    //  光源当たり判定
    //private Collider[] m_colliders;
    //  衝突判定起動中か
    //private bool m_colldiersFlag = true;
    //  距離光源
    private const float DISTANCE_LIGHT = 17.0f;
    [Header("ブロック")]
    [SerializeField] private Block m_block;

    // Start is called before the first frame update
    void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //黒くする
        ChangeBlack();
        //  ブロック情報取得
        //m_block = GetComponent<Block>();

        //  衝突判定取得
        //m_colliders = GetComponents<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        //光源の処理をしない
        if (m_block)
        {
            if (m_block.LightLevel > 0)
            {
                return;
            }
        }

        //  プレイヤーとの距離が一定以上離れていたら処理しない
        if (m_playerTr == null)
        {
            Debug.Log("Error:BlockにPlyer座標が入ってない：" + this);
            return;
        }
        if (Vector2.Distance(transform.position, m_playerTr.position) > DISTANCE_LIGHT) return;
        
        //if (Vector2.Distance(transform.position, m_playerTr.position) > DISTANCE_LIGHT)
        //{
        //    //  判定がついているなら
        //    if (m_colldiersFlag)
        //    {
        //        //  判定を消滅
        //        m_colldiersFlag = false;
        //        foreach (Collider collider in m_colliders)
        //        {
        //            collider.enabled = false;
        //        }
        //    }
        //    return;
        //}
        //else
        //{
        //    //  判定がついていないなら
        //    if (!m_colldiersFlag)
        //    {
        //        //  判定つける
        //        m_colldiersFlag = true;
        //        foreach (Collider collider in m_colliders)
        //        {
        //            collider.enabled = true;
        //        }
        //    }
        //}

		//ライトリストの管理
		for (int i = 0; i < m_lightList.Count; i++)
        {
            //無くなったら消す
            if (m_lightList[i] == null)
            {
                RemoveLightList(i);
            }
            //離れたら消す(3は適当に大きめにしたdeleteLength的な)
            else if (Mathf.Abs(Vector3.Distance(MyFunction.RoundHalfUp(m_lightList[i].gameObject.transform.position), gameObject.transform.position)) > m_lightList[i].LightLevel + 3)
            {
                RemoveLightList(i);
            }
        }

        //色の変更
        ChangeColor();


    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 当たったものがライトではない
        if (collision.gameObject.layer != LayerMask.NameToLayer("Light"))
            return;

        // ライトのレベルが 0 以下
        if (collision.gameObject.GetComponent<ObjectLight>().LightLevel <= 0)
            return;

		// 光源の追加
		if (!CheckForObjectInList(collision.gameObject))
		{
			AddLightList(collision.gameObject);
		}

		////自身がブロック
		//if (m_block)
  //      {
  //          //新規光源なら追加する
  //          if (!CheckForObjectInList(collision.gameObject))
  //          {
  //              AddLightList(collision.gameObject);
  //          }
  //      }
  //      //自身がブロックじゃない時(実質アイテムの処理？)
  //      else
  //      {
		//	//新規光源なら追加する
		//	if (!CheckForObjectInList(collision.gameObject))
		//	{
		//		AddLightList(collision.gameObject);
		//	}
		//}
	}

    private void ChangeColor()
    {
        ////光源はこの処理をしない
        //if (m_block)
        //{
        //    if (m_block.LightLevel > 0)
        //    {
        //        return;
        //    }
        //}

        ////HSVのカラーの吐き出し用
        //float h = 0.0f;
        //float s = 0.0f;
        //float v = 0.0f;


        //例外処理
        if ((m_lightList.Count == 0 || !m_lightList.Any()/* || m_lightList[0] == null || gameObject == null*/))
        {
            ChangeBlack();
            return;
        }
        else
        {
            List<float> lightListV = new();

            for (int i = 0; i < m_lightList.Count; i++)
            {
                if (m_lightList[i] == null)
                    continue;

                float lightLength = Mathf.Ceil(Vector3.Distance(MyFunction.RoundHalfUp(m_lightList[i].transform.position), this.transform.position));

                lightListV.Add(m_lightList[i].LightLevel - lightLength);


            }

            if (m_block && !m_block.IsDestroyed() && lightListV.Count() != 0)
            {
                m_block.ReceiveLightLevel = Math.Max((int)lightListV.Max(), 0);
            }
        }
    }

    // 自身を暗くする
    private void ChangeBlack()
    {
        if (m_block)
        {
			m_block.ReceiveLightLevel = 0;
		}
	}

    // 引数のオブジェクトが既にある(Lightプレハブ)
    bool CheckForObjectInList(GameObject obj)
    {
        // リスト内の各オブジェクトをチェック
        foreach (ObjectLight item in m_lightList)
        {
            if (item == null)
                continue;

            // 同じオブジェクトが見つかった場合はtrueを返す
            if (item.gameObject == obj)
            {
                return true;
            }
        }
        // 同じオブジェクトが見つからなかった場合はfalseを返す
        return false;
    }


    private void AddLightList(GameObject lightObj)
    {
        m_lightList.Add(lightObj.GetComponent<ObjectLight>());
        m_lightPositionList.Add(lightObj.transform.position);
    }
    private void RemoveLightList(int num)
    {
        m_lightList.RemoveAt(num);
        m_lightPositionList.RemoveAt(num);
    }

    //  プレイヤー座標系設定
    public void SetPlayerTransform(Transform tr) { m_playerTr = tr; }
    public Transform GetPlayerTransform() { return m_playerTr; }

    public Block Block
    {
        get { return m_block; }
        set { m_block = value; }
    }
}

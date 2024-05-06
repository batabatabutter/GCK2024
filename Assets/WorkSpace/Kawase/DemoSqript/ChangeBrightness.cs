using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class ChangeBrightness : MonoBehaviour
{
	//明るさの最大値
	private static readonly int MAX_BRIGHTNESS = 7;
	public int GetMAXBRIGHTBESS() { return MAX_BRIGHTNESS; }
    //  距離光源
    private static readonly float DISTANCE_LIGHT = 17.0f;

    //LightList
    public List<ObjectLight> m_lightList = new();
    public HashSet<ObjectLight> m_lights = new HashSet<ObjectLight>();
    //  現在の受け取りライトレベル
    private int m_nowLightLevel = 0;

    //  プレイヤーの座標
    private Transform m_playerTr;

    //  光源
    private ObjectLight m_objlight;
    //  光源衝突判定
    private Collider[] m_lightColliders;
    //  光源衝突判定フラグ
    private bool m_lightColidersFlag = true;

	[Header("ブロック")]
	[SerializeField] private /*Block*/ObjectAffectLight m_affectLight;

	// Start is called before the first frame update
	void Start()
	{
		//黒くする
		ChangeBlack();

        //  光源取得
        if (gameObject.TryGetComponent(out ObjectLight objLight))
        {
            m_objlight = objLight;
            m_nowLightLevel = m_objlight.LightLevel;
            m_lightColliders = GetComponents<Collider>();
        }

        // ライトの影響を受けるオブジェクトの取得
        if (gameObject.TryGetComponent(out ObjectAffectLight affectLight))
		{
			m_affectLight = affectLight;
		}
	}

	// Update is called once per frame
	void Update()
	{
        //  光の強さがMAXなら処理しない
        if (m_objlight)
        {
            if (m_objlight.LightLevel >= MAX_BRIGHTNESS)
            {
                m_affectLight.ReceiveLightLevel = MAX_BRIGHTNESS;
                return;
            }
        }

        //  光源レベル
        int lv = 0;
        if (m_objlight) lv = m_objlight.LightLevel;

        //  プレイヤーとの距離が一定以上離れていたら処理しない
        if (m_playerTr == null)
        {
            if (Time.frameCount % 60 == 0) // 60FPS固定なら、
            {
                Debug.Log("Error:BlockにPlyer座標が入ってない：" + this);
            }
        }
        else if (Vector2.Distance(transform.position, m_playerTr.position) > DISTANCE_LIGHT + lv)
        {
            //  光源がある場合の追加処理
            if (m_objlight && m_lightColidersFlag)
            {
                //  衝突判定を消す
                foreach (Collider col in m_lightColliders) { col.gameObject.SetActive(false); }
                m_lightColidersFlag = false;
            }
            return;
        }
        else
        {
            //  光源がある場合の処理
            if (m_objlight && !m_lightColidersFlag)
            {
                //  衝突判定をつける
                foreach (Collider col in m_lightColliders) { col.gameObject.SetActive(true); }
                m_lightColidersFlag = true;
            }
        }

        ChangeColor();
    }

    void OnTriggerEnter2D(Collider2D collision)
	{
        //  光が最大以上なら処理しない
        if (m_objlight)
            if (m_objlight.LightLevel >= MAX_BRIGHTNESS) return;

        // 当たったものがライトではない
        if (collision.gameObject.layer != LayerMask.NameToLayer("Light"))
			return;

        //  ライトコンポ
        var li = collision.gameObject.GetComponent<ObjectLight>();

        // ライトのレベルが 0 以下
        if (li.LightLevel <= 0)
			return;

        //  ライトのレベルが自分の光源レベル以下
        if (m_objlight)
            if (li.LightLevel <= m_objlight.LightLevel) return;

        // 光源の追加
  //      if (!CheckForObjectInList(collision.gameObject))
		//{
  //          m_lightList.Add(li);
  //      }
        if (!m_lights.Contains(li))
        {
            m_lights.Add(li);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //  光が最大以上なら処理しない
        if (m_objlight)
            if (m_objlight.LightLevel >= MAX_BRIGHTNESS) return;

        // 当たったものがライトではない
        if (collision.gameObject.layer != LayerMask.NameToLayer("Light"))
            return;

        // ライトのレベルが 0 以下
        if (collision.gameObject.GetComponent<ObjectLight>().LightLevel <= 0)
            return;

        // HashSetに変換して処理をする。
        //var mainHashSet = new HashSet<ObjectLight>(m_lightList);
        //foreach (var light in m_lightList)
        //{
        //    if (light == null)
        //        mainHashSet.Remove(light);
        //}
        //m_lightList = mainHashSet.ToList();
        foreach (var light in m_lights)
        {
            if (light == null || light.IsDestroyed())
                m_lights.Remove(light);
        }

        ////ライトリストの管理
        //for (int i = 0; i < m_lightList.Count; i++)
        //{
        //    //無くなったら消す
        //    if (m_lightList[i] == null)
        //    {
        //        RemoveLightList(i);
        //    }
        //    //離れたら消す(3は適当に大きめにしたdeleteLength的な)
        //    else if (Vector3.Distance(MyFunction.RoundHalfUp(m_lightList[i].gameObject.transform.position), gameObject.transform.position) > m_lightList[i].LightLevel + 3)
        //    {
        //        RemoveLightList(i);
        //    }
        //}
    }

    private void ChangeColor()
    {
        // 光源レベルの計算
        int receiveLightLv = m_nowLightLevel;

        foreach(var light in m_lights)
        {
            // 明るさ最大なら処理終了
            if (receiveLightLv >= MAX_BRIGHTNESS)
                break;

            // nullならスキップ
            if (light == null)
                continue;

            // 受けたレベルが自分以下ならスキップ
            if (receiveLightLv >= light.LightLevel)
                continue;

            // 光源距離計算
            float lightLength = Mathf.Ceil(Vector3.Distance(MyFunction.RoundHalfUp(light.transform.position), transform.position));

            receiveLightLv = Mathf.Max(receiveLightLv, light.LightLevel - (int)lightLength);
        }

        //foreach (ObjectLight light in m_lightList)
        //{
        //    // 明るさ最大なら処理終了
        //    if (receiveLightLv >= MAX_BRIGHTNESS)
        //        break;

        //    // nullならスキップ
        //    if (light == null)
        //        continue;

        //    // 受けたレベルが自分以下ならスキップ
        //    if (receiveLightLv >= light.LightLevel)
        //        continue;

        //    // 光源距離計算
        //    float lightLength = Mathf.Ceil(Vector3.Distance(MyFunction.RoundHalfUp(light.transform.position), transform.position));

        //    receiveLightLv = Mathf.Max(receiveLightLv, light.LightLevel - (int)lightLength);
        //}

        // 光源レベル設定
        m_nowLightLevel = receiveLightLv;

        // ブロックが存在するなら
        if (m_affectLight && !m_affectLight.IsDestroyed())
        {
            m_affectLight.ReceiveLightLevel = m_nowLightLevel;
        }
    }

    //   private void ChangeColor()
    //{
    //	//例外処理
    //	if ((m_lightList.Count == 0 || !m_lightList.Any()/* || m_lightList[0] == null || gameObject == null*/))
    //	{
    //		ChangeBlack();
    //		return;
    //	}
    //	else
    //	{
    //           //  総合光レベル
    //           int receiveLightLv = 0;
    //           if (m_objlight) receiveLightLv = m_objlight.LightLevel;

    //           for (int i = 0; i < m_lightList.Count; i++)
    //		{
    //               //  明るさ最大なら処理終了
    //               if (receiveLightLv >= MAX_BRIGHTNESS) break;

    //               //  nullならスキップ
    //               if (m_lightList[i] == null) continue;

    //               //  受けたレベルが自分以下ならスキップ
    //               if (receiveLightLv >= m_lightList[i].LightLevel) continue;

    //               //  光源距離計算
    //               float lightLength = Mathf.Ceil(Vector3.Distance(MyFunction.RoundHalfUp(m_lightList[i].transform.position), this.transform.position));

    //               receiveLightLv = math.max(receiveLightLv, m_lightList[i].LightLevel - (int)lightLength);
    //           }

    //           //  光源レベル設定
    //           m_nowLightLevel = receiveLightLv;

    //           //  ブロックが存在するなら
    //           if (m_affectLight && !m_affectLight.IsDestroyed())
    //           {
    //               m_affectLight.ReceiveLightLevel = m_nowLightLevel;
    //           }
    //	}
    //}

    // 自身を暗くする
    private void ChangeBlack()
	{
		if (m_affectLight)
		{
			m_affectLight.ReceiveLightLevel = 0;
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
	}
	private void RemoveLightList(int num)
	{
		m_lightList.RemoveAt(num);
	}

	//  プレイヤー座標系設定
	public void SetPlayerTransform(Transform tr) { m_playerTr = tr; }
	public Transform GetPlayerTransform() { return m_playerTr; }

	public /*Block*/ObjectAffectLight AffectLight
	{
		get { return m_affectLight; }
		set { m_affectLight = value; }
	}
}

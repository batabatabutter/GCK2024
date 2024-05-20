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

    //LightList
    public HashSet<ObjectLight> m_lights = new HashSet<ObjectLight>();

    //  プレイヤーの座標
    private Transform m_playerTr;

    [Header("ブロック")]
    [SerializeField] private ObjectAffectLight m_affectLight;

    // Start is called before the first frame update
    void Start()
    {
        //黒くする
        ChangeBlack();

        //  フラグオフなら処理しない
        if (m_affectLight)
            if (m_affectLight.BrightnessFlag == false)
            {
                ChangeWhite();
                Destroy(gameObject);
            }
    }

    // Update is called once per frame
    void Update()
    {
        //  光の強さがMAXなら処理しない
        if (m_affectLight.LightLevel >= MAX_BRIGHTNESS)
        {
            m_affectLight.ReceiveLightLevel = MAX_BRIGHTNESS;
            return;
        }

        List<ObjectLight> removeList = new List<ObjectLight>();
        foreach (var light in m_lights)
        {
            if (light == null || light.IsDestroyed())
                removeList.Add(light);
        }
        foreach (var light in removeList)
        {
            m_lights.Remove(light);
        }
        removeList.Clear();

        ChangeColor();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //  光が最大以上なら処理しない
        if (m_affectLight.LightLevel >= MAX_BRIGHTNESS) return;

        //  ライトコンポ
        var li = collision.gameObject.GetComponent<ObjectLight>();

        //  ライトのレベルが 0 以下か自分の光源レベル以下
        if (li.LightLevel <= 0 || li.LightLevel <= m_affectLight.LightLevel) return;

        // 光源の追加
        if (!m_lights.Contains(li))
        {
            m_lights.Add(li);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //  光が最大以上なら処理しない
        if (m_affectLight.LightLevel >= MAX_BRIGHTNESS) return;

        // 当たったものがライトではない
        if (collision.gameObject.layer != LayerMask.NameToLayer("Light"))
            return;

        //  ライトコンポ
        var li = collision.gameObject.GetComponent<ObjectLight>();
        m_lights.Remove(li);
    }

    private void ChangeColor()
    {
        // 光源レベルの計算
        int receiveLightLv = 0;
        if (m_affectLight) receiveLightLv = m_affectLight.LightLevel;

        foreach (var light in m_lights)
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

        // ブロックが存在するなら
        if (m_affectLight)
            if (!m_affectLight.IsDestroyed())
            {
                m_affectLight.ReceiveLightLevel = receiveLightLv;
            }
    }

    // 自身を暗くする
    private void ChangeBlack()
    {
        if (m_affectLight)
        {
            m_affectLight.ReceiveLightLevel = 0;
        }
    }

    private void ChangeWhite()
    {
        if (m_affectLight)
        {
            m_affectLight.ReceiveLightLevel = MAX_BRIGHTNESS;
        }
    }

    //// 引数のオブジェクトが既にある(Lightプレハブ)
    //bool CheckForObjectInList(GameObject obj)
    //{
    //    // リスト内の各オブジェクトをチェック
    //    foreach (ObjectLight item in m_lightList)
    //    {
    //        if (item == null)
    //            continue;

    //        // 同じオブジェクトが見つかった場合はtrueを返す
    //        if (item.gameObject == obj)
    //        {
    //            return true;
    //        }
    //    }
    //    // 同じオブジェクトが見つからなかった場合はfalseを返す
    //    return false;
    //}

    //  プレイヤー座標系設定
    public void SetPlayerTransform(Transform tr) { m_playerTr = tr; }
    public Transform GetPlayerTransform() { return m_playerTr; }

    public /*Block*/ObjectAffectLight AffectLight
    {
        get { return m_affectLight; }
        set { m_affectLight = value; }
    }
}

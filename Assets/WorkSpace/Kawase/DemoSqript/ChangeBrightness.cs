using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeBrightness : MonoBehaviour
{
    //とりあえず色があるもの
    SpriteRenderer spriteRenderer;
    //明るさの最大値
    const int MAX_BRIGHTNESS = 7;

    //LightList
    public List<GameObject> m_lightList = new List<GameObject>();
    public List<Vector3> m_lightPositionList = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //黒くする
        ChangeBlack();
        //光源は光る
        FlashLight();

    }

    // Update is called once per frame
    void Update()
    {
        //光源の処理をしない
        if(GetComponent<Block>())
        {
            if (GetComponent<Block>().LightLevel > 0)
            {
                return;
            }
        }

        //ライトリストの管理
        for (int i = 0; i < m_lightList.Count; i++)
        {
            //無くなったら消す
            if (m_lightList[i] == null)
            {
                RemoveLightList(i);
            }
            //離れたら消す(3は適当に大きめにしたdeleteLength的な)
            else if (Mathf.Abs(Vector3.Distance(m_lightList[i].gameObject.transform.position, gameObject.transform.position)) > m_lightList[i].GetComponent<Block>().LightLevel + 3)
            {
                RemoveLightList(i);
            }
        }

        //色の変更
        ChangeColor();

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //もし自身がブロックで
        if (GetComponent<Block>())
        {
            //ツール（松明）かブロック　で　自身が光源じゃない　かつ　光源レベルが０より大きい
            if ((collision.gameObject.layer == 3 || collision.CompareTag("Block")) && GetComponent<Block>().LightLevel < 1 && collision.gameObject.GetComponent<Block>().LightLevel > 0)
            {
                //新規光源なら追加する
                if (!CheckForObjectInList(collision.gameObject))
                {
                    AddLightList(collision.gameObject);
                }
            }
        }
        //自身がブロックじゃない時
        else if ((collision.gameObject.layer == 3 || collision.CompareTag("Block")) && collision.GetComponent<ChangeBrightness>())
        {
            //新規光源なら追加する
            if (!CheckForObjectInList(collision.gameObject))
            {
                AddLightList(collision.gameObject);
            }
        }
    }

    private void ChangeColor()
    {
        //光源はこの処理をしない
        if (GetComponent<Block>())
        {
            if (GetComponent<Block>().LightLevel > 0)
            {
                return;
            }
        }

        //HSVのカラーの吐き出し用
        float h = 0.0f;
        float s = 0.0f;
        float v = 0.0f;


        //例外処理
        if ((m_lightList.Count == 0 || !m_lightList.Any() || m_lightList[0] == null || gameObject == null))
        {
            return;
        }
        else
        {
            List<float> lightListV = new List<float>();

            for (int i = 0; i < m_lightList.Count; i++)
            {
                if (m_lightList[i] == null)
                    continue;

                float lightLength = Mathf.Ceil(Vector3.Distance(m_lightList[i].transform.position, this.transform.position));

                lightListV.Add(m_lightList[i].GetComponent<Block>().LightLevel - lightLength);


            }
            //なんでかわからんけど2回やんないとバグる
            for (int i = 0; i < 2; i++)
            {
                //(vの明度が０～１００なので)１００を基準にした一メモリを算出
                int rate = 100 / MAX_BRIGHTNESS;
                //多分０．０～１．０の間なのかな？
                v = (rate * lightListV.Max()) * 0.01f;
                //hsvをrgbに変換
                spriteRenderer.color = Color.HSVToRGB(h, s, v);

            }


            if (GetComponent<Block>())
            {
                GetComponent<Block>().ReceiveLightLevel = (int)lightListV.Max();
            }
        }
    }

    private void ChangeBlack()
    {
        //HSVのカラーの吐き出し用
        float h = 0.0f;
        float s = 0.0f;
        float v = 0.0f;
        //rgbをhsvに変換
        Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

        v = 0.0001f;

        //hsvをrgbに変換
        spriteRenderer.color = Color.HSVToRGB(h, s, v);


    }
    private void FlashLight()
    {
        //ブロックスクリプト餅なら
        if (GetComponent<Block>())
        {
            int lightLevel = GetComponent<Block>().LightLevel;
            //明るさレベルを持ってるか
            if (lightLevel > 0)
            {
                //一瞬コライダー展開
                StartCoroutine(AddCricleColToDelete());

                //HSVのカラーの吐き出し用
                float h = 0.0f;
                float s = 0.0f;
                float v = 0.0f;
                //rgbをhsvに変換
                Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

                //(vの明度が０～１００なので)１００を基準にした一メモリを算出
                int rate = 100 / MAX_BRIGHTNESS;


                //多分０．０～１．０の間なのかな？
                v = (rate * lightLevel) * 0.01f;

                //hsvをrgbに変換
                spriteRenderer.color = Color.HSVToRGB(h, s, v);

            }
        }
    }


    private IEnumerator AddCricleColToDelete()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        //もしなかったら
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;

        }
        //円のコライダー
        CircleCollider2D circleCol = gameObject.AddComponent<CircleCollider2D>();

        //明るさレベルで大きさ指定
        circleCol.radius = GetComponent<Block>().LightLevel;
        circleCol.isTrigger = true;

        yield return new WaitForSeconds(0.0f);

        //Destroy(circleCol);

    }


    bool CheckForObjectInList(GameObject obj)
    {
        // リスト内の各オブジェクトをチェック
        foreach (GameObject item in m_lightList)
        {
            // 同じオブジェクトが見つかった場合はtrueを返す
            if (item == obj)
            {
                return true;
            }
        }
        // 同じオブジェクトが見つからなかった場合はfalseを返す
        return false;
    }


    private void AddLightList(GameObject lightObj)
    {
        m_lightList.Add(lightObj);
        m_lightPositionList.Add(lightObj.transform.position);
    }
    private void RemoveLightList(int num)
    {
        m_lightList.RemoveAt(num);
        m_lightPositionList.RemoveAt(num);
    }
}

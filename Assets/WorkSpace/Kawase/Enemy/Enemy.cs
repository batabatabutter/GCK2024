using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("敵データベース")]
    [SerializeField] EnemyData m_enemyData;

    [Header("アイテムのデータベース")]
    [SerializeField] private ItemDataBase m_itemDataBase = null;


    //攻撃間隔
    float m_attackCoolTime;

    //ドロップアイテム
    List<BlockData.DropItems> m_dropItems;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_attackCoolTime = m_enemyData.coolTime;

        m_dropItems = m_enemyData.dropItems;

    }

    // Update is called once per frame
    protected virtual void Update()
    {

        if (m_attackCoolTime < 0)
        {
            //攻撃
            Attack();

            m_attackCoolTime = m_enemyData.coolTime;
        }
        else
        {
            m_attackCoolTime -= Time.deltaTime;
        }


    }

    public virtual void Attack()
    {

    }
    public virtual void Dead()
    {
        DropItem();

        Destroy(gameObject);
    }

    public virtual void DropItem()
    {
        //アイテム落とす

        foreach (BlockData.DropItems dropItem in m_dropItems)
        {
            // 0 ~ 1乱数取得
            float random = Random.value;

            // 乱数がドロップ確率より大きい
            if (dropItem.rate < random)
                continue;

            // アイテムのデータを取得
            ItemData data = MyFunction.GetItemData(m_itemDataBase, dropItem.type);

            // データがない場合はドロップしない
            if (data == null)
                continue;

            // アイテムのゲームオブジェクトを生成
            GameObject obj = Instantiate(data.prefab, transform.position, Quaternion.identity);

            // 明るさの概念を追加
            obj.AddComponent<ChangeBrightness>();

            // 名前を変える
            obj.name = "Material_" + dropItem.type.ToString();

            // アイテムがドロップしたときの処理
            if (obj.TryGetComponent(out Item item))
            {
                // 種類の設定
                item.ItemType = dropItem.type;
                // ドロップ数の設定
                item.Drop(dropItem.count);
            }


        }


    }
}
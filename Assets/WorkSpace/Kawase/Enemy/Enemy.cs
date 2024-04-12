using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// 一番親アダムイブ
/// </summary>
public class Enemy : MonoBehaviour
{
    //種類
    public enum Type
    {
        Dorinoko,
        Iwarun,
        Hotarun,

        OverID
    }
    //系統
    public enum System
    {
        Dwell,//宿り型


        OverID
    }

    [Header("敵データベース")]
    [SerializeField] EnemyData m_enemyData;

    [Header("アイテムのデータベース")]
    [SerializeField] private ItemDataBase m_itemDataBase = null;

    [Header("障害物レイヤー")]
    [SerializeField] private LayerMask m_blockLayer;

    //プレイヤー
    private GameObject m_player = null;

    public GameObject Player
    {
        get
        {
            return m_player;
        }
        set
        {
            m_player = value;
        }
    }


    //攻撃間隔
    float m_attackCoolTime;

    //ドロップアイテム
    List<BlockData.DropItems> m_dropItems;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_attackCoolTime = m_enemyData.coolTime;

        m_dropItems = m_enemyData.dropItems;


        // サークルコライダー2Dがアタッチされていない場合、追加する
        if (GetComponent<CircleCollider2D>() == null)
        {
            // サークルコライダー2Dを追加する
            CircleCollider2D circleCollider = gameObject.AddComponent<CircleCollider2D>();

            // コライダーの半径を設定する
            circleCollider.radius = m_enemyData.radius;

            // コライダーがトリガーとして動作する
            circleCollider.isTrigger = true; 
        }
        else
        {
            GetComponent<CircleCollider2D>().radius = m_enemyData.radius;
        }


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
            float length = m_enemyData.radius;
            Transform startTransform = transform;

            if (m_player && Vector3.Distance(startTransform.position, m_player.transform.position) < length)
            {
                Vector3 direction = m_player.transform.position - startTransform.position;

                RaycastHit hit;
                // レイを飛ばす
                if (!Physics.Raycast(startTransform.position, direction, out hit, length, m_blockLayer))
                {
                    // 線をデバッグ表示
                    Debug.DrawLine(startTransform.position, m_player.transform.position, Color.red);
                    // ここで処理を行う（ブロックされていない場合の処理）
                    m_attackCoolTime -= Time.deltaTime;
                }
            }

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
            GameObject obj = Instantiate(data.Prefab, transform.position, Quaternion.identity);

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


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_player = collision.gameObject;

        }
    }
}
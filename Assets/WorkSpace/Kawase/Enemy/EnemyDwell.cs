using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// enemyを親にした宿り型の敵
/// </summary>
public class EnemyDwell : Enemy
{
    GameObject m_dwellBlock;
    // プロパティ
    public GameObject DwellBlock
    {
        get
        {
            return m_dwellBlock;
        }
        set
        {
            m_dwellBlock = value;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (base.Player != null)
        {
            RotationToPlayer();
        }

        //宿り先が死んだら死ぬ
        if (!m_dwellBlock)
        {
           base.Dead();
        }
    }

    protected void RotationToPlayer()
    {
        Transform target = base.Player.transform;
        // ターゲットの方向ベクトルを計算
        Vector3 direction = target.position - transform.position;

        // 方向ベクトルを正規化
        direction.Normalize();

        // 方向ベクトルから角度を計算
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 角度を四捨五入して、0°、90°、180°、270°のいずれかに丸める
        angle = Mathf.Round(angle / 90) * 90 - 90;

        // オブジェクトを回転
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

}
